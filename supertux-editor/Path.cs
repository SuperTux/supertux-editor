using System;
using System.Collections.Generic;

[LispRoot("path")]
public class Path
{
	[LispChild("mode")]
	public string Mode;
	
	[LispChilds(Name = "node", Type = typeof(PathNode), ListType = typeof(PathNode))]
	public List<PathNode> Nodes = new List<PathNode>();
	
	private class PathNode
	{
		[LispChild("x")]
		public float X;
		[LispChild("y")]
		public float Y;
		[LispChild("time")]
		public float Time = 1f;
	}
	
	public Path()
	{
	}
}
