using System;
using System.Collections.Generic;
using DataStructures;
using LispReader;

[LispRoot("path")]
public class Path
{
	// we should support enums here...
	/// Can be: oneshot, pingpong and circular
	[LispChild("mode", Optional = true, Default = "circular")]
	public string Mode = "circular";
	
	[LispChilds(Name = "node", Type = typeof(Node), ListType = typeof(Node))]
	public List<Node> Nodes = new List<Node>();
	
	public class Node
	{
		[LispChild("x")]
		public float X;
		[LispChild("y")]
		public float Y;
		[LispChild("time", Optional = true, Default = 1f)]
		public float Time = 1f;
		
		public Vector Pos {
			get {
				return new Vector(X, Y);
			}
			set {
				X = value.X;
				Y = value.Y;
			}
		}
	}
	
	public Path()
	{
	}

	public void Move(Vector offset)
	{
		foreach(Node node in Nodes) {
			node.X += offset.X;
			node.Y += offset.Y;
		}
	}

	/// <summary>
	/// Rotates path nodes, e.g. from 0,1,2,3 to 1,2,3,0
	/// </summary>
	public void Shift(int delta) {
		// make sure we have at least one node for shifting
		if (Nodes.Count < 1) return;

		// care for positive deltas
		for (int i = 1; i <= delta; i++) {
			Node node = Nodes[0];
			Nodes.RemoveAt(0);
			Nodes.Add(node);
		}

		// care for negative deltas
		for (int i = -1; i >= delta; i--) {
			Node node = Nodes[Nodes.Count-1];
			Nodes.RemoveAt(Nodes.Count-1);
			Nodes.Insert(0, node);
		}
	}
}

/// <summary>Implemented by objects that contain a path</summary>
public interface IPathObject {
	Path Path {
		get;
		set;
	}	
}
