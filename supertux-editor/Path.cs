using System;
using System.Collections.Generic;
using DataStructures;
using LispReader;

[LispRoot("path")]
public class Path
{
	[LispChild("mode")]
	public string Mode;
	
	[LispChilds(Name = "node", Type = typeof(PathNode), ListType = typeof(PathNode))]
	public List<PathNode> Nodes = new List<PathNode>();
	
	public class PathNode
	{
		[LispChild("x")]
		public float X;
		[LispChild("y")]
		public float Y;
		[LispChild("time")]
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
