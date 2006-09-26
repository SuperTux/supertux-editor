using System;
using DataStructures;
using SceneGraph;
using Lisp;
using LispReader;
using System.Collections.Generic;

[SupertuxObject("tilemap", "images/engine/editor/tilemap.png")]
public sealed class Tilemap : TileBlock, IGameObject, IPathObject {
	[LispChild("z-pos")]
	public int ZPos = 0;

	[PropertyProperties(Tooltip = "If selected Tux will interact with tiles in this tilemap.")]
	[LispChild("solid")]
	public bool Solid = false;

	[PropertyProperties(Tooltip = ToolTipStrings.ScriptingName)]
	[LispChild("name", Optional = true, Default = "")]
	public string Name = "";

	[LispChild("speed")]
	public float Speed = 1.0f;

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

	public enum DrawTargets {
		/// <summary>
		/// Normal tilemap.
		/// </summary>
		normal,
		/// <summary>
		/// Used for lightmap.
		/// </summary>
		lightmap
	}

	/// <summary>
	/// Target for tilemap.
	/// </summary>
	[LispChild("draw-target", Optional = true, Default = DrawTargets.normal)]
	public DrawTargets DrawTarget = DrawTargets.normal;

	public Tilemap() : base() {
	}

}

