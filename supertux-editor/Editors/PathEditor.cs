using System;
using System.Collections.Generic;
using Gdk;
using SceneGraph;
using DataStructures;
using OpenGl;

public class PathEditor : IEditor
{	
	private class PathNode : IObject, Node
	{
		private Path.PathNode node;
		
		public PathNode(Path.PathNode node) {
			this.node = node;
		}
		
		public bool Resizable {
			get {
				return false;
			}
		}
		
		public void ChangeArea(RectangleF Area)
		{
			node.X = Area.Left + 10;
			node.Y = Area.Top + 10;
		}
		
		public RectangleF Area {
			get {
				return new RectangleF(node.X - 10, node.Y - 10,
				                      20, 20);
			}
		}
		
		public void Draw()
		{
			gl.Color4f(0, 0, 1, 0.7f);
			gl.Disable(gl.TEXTURE_2D);
			
			float left = node.X - 10;
			float right = node.X + 10;
			float top = node.Y - 10;
			float bottom = node.Y + 10;
			
			gl.Begin(gl.QUADS);
			gl.Vertex2f(left, top);
			gl.Vertex2f(right, top);
			gl.Vertex2f(right, bottom);
			gl.Vertex2f(left, bottom);
			gl.End();
			
			gl.Enable(gl.TEXTURE_2D);
			gl.Color4f(1, 1, 1, 1);			
		}
		
		public Node GetSceneGraphNode() {
			return this;
		}
	}
	
	private List<PathNode> pathNodes = new List<PathNode>();
	public event RedrawEventHandler Redraw;
	
	public PathEditor(Path path)
	{
	}
	
	public void Draw()
	{
		foreach(PathNode pathNode in pathNodes) {
			pathNode.Draw();
		}
	}
	
	public void OnMouseButtonPress(Vector pos, int button, ModifierType Modifiers)
	{
	}
	
	public void OnMouseButtonRelease(Vector pos, int button, ModifierType Modifiers)
	{
	}
	
	public void OnMouseMotion(Vector pos, ModifierType Modifiers)
	{
	}
}
