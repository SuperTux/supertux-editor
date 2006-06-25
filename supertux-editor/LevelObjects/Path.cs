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
}

/// <summary>Implemented by objects that contain a path</summary>
public interface IPathObject {
	Path Path {
		get;
		set;
	}	
}
