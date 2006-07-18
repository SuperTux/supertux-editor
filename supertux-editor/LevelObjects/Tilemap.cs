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
	
	public Tilemap() : base() {
	}

}

