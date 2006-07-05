using System;
using System.Collections.Generic;
using DataStructures;
using LispReader;

[LispRoot("path")]
public class Path
{

	/// <summary>
	/// Modes for <see cref="Path"/>.
	/// </summary>
	public enum Modes {
		/// <summary>
		/// Follows path to it's end and then stops
		/// </summary>
		oneshot,
		/// <summary>
		/// Follows path to it's end and then follow the path backwards and so on.
		/// </summary>
		pingpong,
		/// <summary>
		/// Follows path to it's end and then continues from start again.
		/// </summary>
		circular
	}

	/// Can be: oneshot, pingpong and circular
	[LispChild("mode", Optional = true, Default = Modes.circular)]
	public Modes Mode = Modes.circular;
	
	[LispChilds(Name = "node", Type = typeof(Node), ListType = typeof(Node))]
	public List<Node> Nodes = new List<Node>();
	
	public class Node
	{
		[CustomTooltip("X position of node")]
		[LispChild("x")]
		public float X;
		[CustomTooltip("Y position of node")]
		[LispChild("y")]
		public float Y;

		// TODO: Is this tooltip vaild for other path modes than circular?
		[CustomTooltip("Seconds it will take to go to the node after this one.")]
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
