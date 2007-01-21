//  $Id$
using System;
using System.Timers;
using System.Collections.Generic;
using Gtk;
using Gdk;
using SceneGraph;
using DataStructures;
using OpenGl;

public sealed class PathEditor : EditorBase, IEditor, IEditorCursorChange, IDisposable
{
	public event RedrawEventHandler Redraw;
	public event CursorChangeHandler CursorChange;
	private Path path;
	private Path.Node selectedNode;
	private const float NODE_SIZE = 10;
	private bool dragging;
	/// <summary>
	/// Used to make sure we just do one undo snapshot when moving.
	/// </summary>
	private bool moveStarted;
	private Vector pressPoint;
	private Vector originalPos;
	private ushort linepattern = 7;
	private static bool killTimer;

	public PathEditor(IEditorApplication application, Path path)
	{
		this.application = application;
		this.path = path;
		killTimer = false;
		GLib.Timeout.Add(100, Animate);
	}

	private bool Animate()
	{
		if(killTimer)
			return false;

		linepattern = (ushort) ((linepattern >> 15) | (linepattern << 1));
		Redraw();
		return true;
	}

	public void Dispose()
	{
		killTimer = true;
	}

	public void Draw()
	{
		// draw path edges
		gl.Color4f(1, 0, 0, 0.7f);
		gl.Disable(gl.TEXTURE_2D);
		gl.Enable(gl.LINE_STIPPLE);
		gl.LineStipple(1, linepattern);

		gl.Begin(gl.LINE_STRIP);
		foreach(Path.Node node in path.Nodes) {
			gl.Vertex2f(node.X, node.Y);
		}
		gl.End();
		gl.Disable(gl.LINE_STIPPLE);
		gl.Enable(gl.TEXTURE_2D);
		gl.Color4f(1, 1, 1, 1);

		// draw path nodes
		gl.Color4f(1, 0, 0, 0.4f);
		gl.Disable(gl.TEXTURE_2D);
		gl.Begin(gl.QUADS);

		foreach(Path.Node node in path.Nodes) {
			if(node == selectedNode) {
				gl.Color4f(1, 1, 1, 0.7f);
			}

			float left = node.X - NODE_SIZE;
			float right = node.X + NODE_SIZE;
			float top = node.Y - NODE_SIZE;
			float bottom = node.Y + NODE_SIZE;

			gl.Vertex2f(left, top);
			gl.Vertex2f(right, top);
			gl.Vertex2f(right, bottom);
			gl.Vertex2f(left, bottom);
			if(node == selectedNode) {
				gl.Color4f(1, 0, 0, 0.4f);
			}
		}

		gl.End();
		gl.Enable(gl.TEXTURE_2D);
		gl.Color4f(1, 1, 1, 1);
	}

	public void OnMouseButtonPress(Vector pos, int button, ModifierType Modifiers)
	{
		if(button == 1) {
			Path.Node node = FindNodeAt(pos);

			if(node == null) {
				if((Modifiers & ModifierType.ControlMask) != 0) {
					Vector pointOnEdge = new Vector(0, 0);
					int addNode = FindPath(pos, ref pointOnEdge);
					if(addNode >= 0) {
						application.TakeUndoSnapshot("Added Path node");
						node = new Path.Node();
						node.Pos = pointOnEdge;
						path.Nodes.Insert(addNode+1, node);
					}
				} else if(selectedNode == path.Nodes[path.Nodes.Count - 1]) {
					application.TakeUndoSnapshot("Added Path node");
					node = new Path.Node();
					//Snap?
					if( application.SnapToGrid ) {
						pos = new Vector((float) ((int)pos.X / 32) * 32,
						                 (float) ((int)pos.Y / 32) * 32);
					}
					node.Pos = pos;
					path.Nodes.Add(node);
				} else if(selectedNode == path.Nodes[0]) {
					application.TakeUndoSnapshot("Added Path node");
					node = new Path.Node();
					node.Pos = pos;
					path.Nodes.Insert(0, node);
				}
			}

			if(node != selectedNode) {
				selectedNode = node;
				Redraw();
			}

			if(selectedNode != null) {
				application.EditProperties(selectedNode, "Path Node");
				dragging = true;
				pressPoint = pos;
				originalPos = selectedNode.Pos;
			}

		} else if(button == 3) {
			PopupMenu(button);
		}
	}

	public void OnMouseButtonRelease(Vector pos, int button, ModifierType Modifiers)
	{
		dragging = false;
		moveStarted = false;
	}

	public void OnMouseMotion(Vector pos, ModifierType Modifiers)
	{
		if(dragging) {
			if (!moveStarted) {
				application.TakeUndoSnapshot("Moved Path Node");
				moveStarted = true;
			}
			Vector spos = originalPos + (pos - pressPoint);
			// snap to 32pixel?
			if((Modifiers & ModifierType.ShiftMask) != 0 || application.SnapToGrid ) {
				spos = new Vector((float) ((int)spos.X / 32) * 32,
				                  (float) ((int)spos.Y / 32) * 32);
			}
			if(selectedNode.Pos != spos) {
				selectedNode.Pos = spos;
				Redraw();
			}
		} else {
			Path.Node node = FindNodeAt(pos);
			Vector dummy = new Vector(0, 0);
			if(node != null) {
				/*
				Cursor cursor = new Cursor();
				Pixmap pixmap = new Pixmap(GetType().Assembly.GetManifestResourceAsStream("modifier-move.png"));
				*/

				CursorChange(new Cursor(CursorType.Tcross));
			} else if((Modifiers & ModifierType.ControlMask) != 0 && FindPath(pos, ref dummy) >= 0) {
				CursorChange(new Cursor(CursorType.Boat));
			} else {
				CursorChange(new Cursor(CursorType.Arrow));
			}
		}
	}

	private void PopupMenu(int button)
	{
		if(selectedNode == null)
			return;

		Menu popupMenu = new Menu();

		MenuItem deleteItem = new ImageMenuItem(Stock.Delete, null);
		deleteItem.Activated += OnDelete;
		deleteItem.Sensitive = path.Nodes.Count > 1;
		popupMenu.Append(deleteItem);

		MenuItem shiftLeftItem = new ImageMenuItem(Stock.GoBack, null);
		shiftLeftItem.Activated += OnShiftLeft;
		shiftLeftItem.Sensitive = path.Nodes.Count > 1;
		popupMenu.Append(shiftLeftItem);

		MenuItem shiftRightItem = new ImageMenuItem(Stock.GoForward, null);
		shiftRightItem.Activated += OnShiftRight;
		shiftRightItem.Sensitive = path.Nodes.Count > 1;
		popupMenu.Append(shiftRightItem);

		popupMenu.ShowAll();
		popupMenu.Popup();
	}

	private void OnDelete(object o, EventArgs args)
	{
		application.TakeUndoSnapshot("Deleted Path node");
		path.Nodes.Remove(selectedNode);
		selectedNode = null;
		dragging = false;
		Redraw();
	}

	private void OnShiftLeft(object o, EventArgs args)
	{
		path.Shift(-1);
		Redraw();
	}

	private void OnShiftRight(object o, EventArgs args)
	{
		path.Shift(1);
		Redraw();
	}

	/// <summary>Returns the first node found at position <paramref name="pos"/></summary>
	private Path.Node FindNodeAt(Vector pos)
	{
		foreach(Path.Node node in path.Nodes) {
			if(pos.X >= node.X - NODE_SIZE && pos.X <= node.X + NODE_SIZE
			   && pos.Y >= node.Y - NODE_SIZE && pos.Y <= node.Y + NODE_SIZE) {
				return node;
			}
		}

		return null;
	}

	/// <summary>
	/// Checks all edges between nodes, if the distance of pos towards such an
	/// edge is smaller than 10, then return the number of the first node in that
	/// edge
	/// </summary>
	private int FindPath(Vector pos, ref Vector pointOnEdge)
	{
		int bestNode = -1;
		float bestDistance = 10;

		for(int i = 0; i < path.Nodes.Count - 1; ++i) {
			Path.Node n0 = path.Nodes[i];
			Path.Node n1 = path.Nodes[i+1];

			Vector n0_n1 = n1.Pos - n0.Pos;
			float edgeLen = n0_n1.Norm();
			Vector dir = n0_n1 / edgeLen;
			float s = (pos - n0.Pos) * dir;
			if(s <= 0 || s >= edgeLen) {
				continue;
			}

			Vector projPoint = n0.Pos + dir * s;
			float distance = (projPoint - pos).Norm();
			if(distance < bestDistance) {
				bestNode = i;
				bestDistance = distance;
				pointOnEdge = projPoint;
			}
		}

		return bestNode;
	}
}
