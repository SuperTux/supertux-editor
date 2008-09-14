//  $Id$
using System;
using DataStructures;
using SceneGraph;
using Lisp;
using LispReader;
using System.Collections.Generic;

		//tilemaps are no longer created as badguys, it is looking bad
[SupertuxObject("tilemap", "images/engine/editor/tilemap.png",
                Target = SupertuxObjectAttribute.Usage.None)]
public sealed class Tilemap : TileBlock, IGameObject, IPathObject, ILayered {
	private int ZPos = 0;
	[LispChild("z-pos")]
	public int Layer {
		get {
			return ZPos;
		}
		set {
			ZPos = value;
		}
	}

//TODO: If we want to store X coordinate to level file, we must uncomment this and add support for it
	//If you do that, please remove " else X = Y = 0;" in UpdatePos();
//	[LispChild("x", Optional = true, Default = 0.0f)]
	public float X = 0;

//TODO: If we want to store Y coordinate to level file, we must uncomment this and add support for it
	//If you do that, please remove " else X = Y = 0;" in UpdatePos();
//	[LispChild("y", Optional = true, Default = 0.0f)]
	public float Y = 0;

	[PropertyProperties(Tooltip = "If selected Tux will interact with tiles in this tilemap.")]
	[LispChild("solid")]
	public bool Solid = false;

	[PropertyProperties(Tooltip = ToolTipStrings.ScriptingName)]
	[LispChild("name", Optional = true, Default = "")]
	public string Name = String.Empty;

	[LispChild("speed", Optional = true, Default = 1.0f)]
	public float Speed = 1.0f;

	[LispChild("speed-y", Optional = true, Default = 1.0f)]
	public float SpeedY = 1.0f;

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

	[PropertyProperties(Tooltip = "Opacity of this Tilemap, ranges from 0.0 (transparent) to 1.0 (fully opaque)")]
	[LispChild("alpha", Optional = true, Default = 1.0f)]
	public float Alpha = 1.0f;

	public Tilemap() : base() {
	}

	public void UpdatePos(){
		if (path != null && path.Nodes.Count > 0){
			X = path.Nodes[0].X;
			Y = path.Nodes[0].Y;
		} else X = Y = 0;
	}
}
