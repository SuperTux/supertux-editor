//  SuperTux Editor
//  Copyright (C) 2006 Matthias Braun <matze@braunis.de>
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

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
		circular,
		/// <summary>
		///  Moves randomly among the nodes
		/// </summary>
		unordered
	}

	/// Can be: oneshot, pingpong and circular
	[LispChild("mode", Optional = true, Default = Modes.circular)]
	public Modes Mode = Modes.circular;

	[LispChilds(Name = "node", Type = typeof(Node), ListType = typeof(Node))]
	public List<Node> Nodes = new List<Node>();

	public class Node : ICloneable
	{
		[PropertyProperties(Tooltip = "X position of node")]
		[LispChild("x")]
		public float X;
		[PropertyProperties(Tooltip = "Y position of node")]
		[LispChild("y")]
		public float Y;

		// TODO: Is this tooltip valid for other path modes than circular?
		[PropertyProperties(Tooltip = "Seconds it will take to go to the node after this one.")]
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

		public object Clone() {
			return MemberwiseClone();
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

	/// <summary>If true it is possible to remove the path from the object.</summary>
	bool PathRemovable {
		get;
	}
}

/* EOF */
