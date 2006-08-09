using System;
using DataStructures;
using SceneGraph;
using Lisp;
using LispReader;
using System.Collections.Generic;

[SupertuxObject("tilemap", "images/engine/editor/tilemap.png")]
public class Tilemap : TileBlock, IGameObject {
	[LispChild("z-pos")]
	public int ZPos = 0;

	[LispChild("solid")]
	public bool Solid = false;

	[CustomTooltip(ToolTipStrings.ScriptingName)]
	[LispChild("name", Optional = true, Default = "")]
	public string Name = "";

	[LispChild("speed")]
	public float Speed = 1.0f;

	//TODO: Make this work
	private Path path;
	[LispChild("path", Optional = true, Default = null)]
	public Path Path {
		get {
			return path;
		}
		set {
			path = value;
		}
	}

	public Tilemap() : base() {
	}

}

