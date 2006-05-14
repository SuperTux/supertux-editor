using System;
using System.Timers;
using System.Collections.Generic;
using Gtk;
using Gdk;
using SceneGraph;
using DataStructures;
using OpenGl;

public class PathEditor : IEditor, IDisposable
{	
	public event RedrawEventHandler Redraw;
	private Path path;
	private Path.Node selectedNode;
	private const float NODE_SIZE = 10;
	private bool dragging;
	private Vector pressPoint;
	private Vector originalPos;
	private uint timerId;
	private ushort linepattern = 7;
	
	public PathEditor(Path path)
	{
		this.path = path;
		timerId = Timeout.Add(100, Animate);
	}
	
	private bool Animate()
	{
		linepattern = (ushort) ((linepattern >> 15) | (linepattern << 1));
		Redraw();
		return true;
	}
	
	public void Dispose()
	{
		Timeout.Remove(timerId);
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
		Path.Node node = findNodeAt(pos);
		
		if(node == null) {
			if(selectedNode == path.Nodes[0]) {
				node = new Path.Node();
				node.Pos = pos;
				path.Nodes.Insert(0, node);
			} else if(selectedNode == path.Nodes[path.Nodes.Count - 1]) {
				node = new Path.Node();
				node.Pos = pos;
				path.Nodes.Add(node);
			}
		}
		
		if(node != selectedNode) {
			selectedNode = node;			
			Redraw();
		}
		
		if(selectedNode != null) {
			dragging = true;
			pressPoint = pos;
			originalPos = selectedNode.Pos;
		}
	}
	
	public void OnMouseButtonRelease(Vector pos, int button, ModifierType Modifiers)
	{
		dragging = false;
	}
	
	public void OnMouseMotion(Vector pos, ModifierType Modifiers)
	{
		if(dragging) {
			selectedNode.Pos = originalPos + (pos - pressPoint);
			Redraw();
		}
	}
	
	private Path.Node findNodeAt(Vector pos)
	{
		foreach(Path.Node node in path.Nodes) {
			if(pos.X >= node.X - NODE_SIZE && pos.X <= node.X + NODE_SIZE
			      && pos.Y >= node.Y - NODE_SIZE && pos.Y <= node.Y + NODE_SIZE) {
				return node;
			}      
		}
		
		return null;
	}
}
