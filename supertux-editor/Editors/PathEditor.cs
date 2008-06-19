//  $Id$
using System;
using System.Timers;
using System.Collections.Generic;
using Gtk;
using Gdk;
using SceneGraph;
using DataStructures;
using OpenGl;
using LispReader;
using Undo;

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
		application.EditProperties(path, "Path");
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

	public void Draw(Gdk.Rectangle cliprect)
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

	public void OnMouseButtonPress(Vector mousePos, int button, ModifierType Modifiers)
	{
		if(button == 1) {
			Path.Node node = FindNodeAt(mousePos);

			if(node == null) {
				if((Modifiers & ModifierType.ControlMask) != 0) {
					Vector pointOnEdge = new Vector(0, 0);
					int addNode = FindPath(mousePos, ref pointOnEdge);
					if(addNode >= 0) {
						node = new Path.Node();
						node.Pos = pointOnEdge;
						PathNodeAddCommand command = new PathNodeAddCommand("Added Path node", node, path, addNode+1);
						command.Do();
						UndoManager.AddCommand(command);
					}
				} else if(selectedNode == path.Nodes[path.Nodes.Count - 1]) {
					node = new Path.Node();
					// Snap?
					if( application.SnapToGrid ) {
						mousePos = new Vector((float) ((int)mousePos.X / 32) * 32,
						                      (float) ((int)mousePos.Y / 32) * 32);
					}
					node.Pos = mousePos;
					PathNodeAddCommand command = new PathNodeAddCommand("Added Path node", node, path);
					command.Do();
					UndoManager.AddCommand(command);
				} else if(selectedNode == path.Nodes[0]) {
					node = new Path.Node();
					node.Pos = mousePos;
					PathNodeAddCommand command = new PathNodeAddCommand("Added Path node", node, path, 0);
					command.Do();
					UndoManager.AddCommand(command);
				}
			}

			if(node != selectedNode) {
				selectedNode = node;
				Redraw();
			}

			if(selectedNode != null) {
				application.EditProperties(selectedNode, "Path Node");
				dragging = true;
				pressPoint = mousePos;
				originalPos = selectedNode.Pos;
			}

		} else if(button == 3) {
			if(dragging)	{
				dragging = false;
				selectedNode.Pos = originalPos;
				Redraw();
			} else	{
				Path.Node node = FindNodeAt(mousePos);
				if (node != null) 	//if we have clicked on a node..
					selectedNode = node;	//..make that node active
				PopupMenu(button);
			}
		}
	}

	public void OnMouseButtonRelease(Vector mousePos, int button, ModifierType Modifiers)
	{
		if (dragging && selectedNode.Pos != originalPos){
			PropertyChangeCommand command = new PropertyChangeCommand("Moved Path Node", new FieldOrProperty.Property(typeof(Path.Node).GetProperty("Pos")), selectedNode, selectedNode.Pos, originalPos);
			UndoManager.AddCommand(command);
		}

		dragging = false;
		moveStarted = false;
	}

	public void OnMouseMotion(Vector mousePos, ModifierType Modifiers)
	{
		if(dragging) {
			if (!moveStarted) {
				moveStarted = true;
			}
			Vector spos = originalPos + (mousePos - pressPoint);
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
			Path.Node node = FindNodeAt(mousePos);
			Vector dummy = new Vector(0, 0);
			if(node != null) {
				/*
				Cursor cursor = new Cursor();
				Pixmap pixmap = new Pixmap(GetType().Assembly.GetManifestResourceAsStream("modifier-move.png"));
				*/

				CursorChange(new Cursor(CursorType.Tcross));
			} else if ((Modifiers & ModifierType.ControlMask) != 0 && FindPath(mousePos, ref dummy) >= 0) {
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
		//Do not allow to delete already deleted node.
		deleteItem.Sensitive = path.Nodes.Count > 1 && path.Nodes.IndexOf(selectedNode) > -1;
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
		PathNodeRemoveCommand command = new PathNodeRemoveCommand("Removed Path node", selectedNode, path);
		command.Do();
		UndoManager.AddCommand(command);
		selectedNode = null;
		dragging = false;
		Redraw();
	}

	private void OnShiftLeft(object o, EventArgs args)
	{
		PathShiftCommand command = new PathShiftCommand("Path shifted backwards", path, -1);
		command.Do();
		UndoManager.AddCommand(command);
		Redraw();
	}

	private void OnShiftRight(object o, EventArgs args)
	{
		PathShiftCommand command = new PathShiftCommand("Path shifted forward", path, 1);
		command.Do();
		UndoManager.AddCommand(command);
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
