//  $Id$
using System;
using DataStructures;
using Drawing;
using Lisp;
using LispReader;
using System.Collections.Generic;

[SupertuxObject("tilemap", "images/engine/editor/tilemap.png")]
public sealed class Tilemap : VirtualObject, IPathObject {
	public TileBlock Tiles = new TileBlock();

	[LispChild("z-pos")]
	public int ZPos = 0;

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

	private Path path = new Path(new Vector(0, 0));
	[LispChild("path", Optional = true, Default = null)]
	public Path Path {
		get {
			return path;
		}
		set {
			path = value;
		}
	}

	public uint Width {
		get {
			return Tiles.Width;
		}
	}
	public uint Height {
		get {
			return Tiles.Height;
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

	public override void Draw(DrawingContext context) {
		Tileset tileset = Tileset.CurrentTileset;
		int  tile_width  = (int) tileset.TILE_WIDTH;
		int  tile_height = (int) tileset.TILE_HEIGHT;
		/* TODO
		uint start_x     = (uint) Math.Max(0, cliprect.Left / tile_width - 1);
		uint start_y     = (uint) Math.Max(0, cliprect.Top / tile_height - 1);
		uint end_x       = (uint) Math.Max(0, Math.Min(Tiles.Width, cliprect.Right / tile_width + 1));
		uint end_y       = (uint) Math.Max(0, Math.Min(Tiles.Height, cliprect.Bottom / tile_height + 1));
		*/
		uint start_x = 0;
		uint start_y = 0;
		uint end_x   = Tiles.Width;
		uint end_y   = Tiles.Height;
		Tile Tile;

		for (uint y = start_y; y < end_y; ++y) {
			for (uint x = start_x; x < end_x; ++x) {
				Tile = tileset.Get(Tiles[x, y]);
				if (Tile == null) {
					LogManager.Log(LogLevel.Warning,
					               "Tile {0} is null?! The tile with id {0} at {1},{2} is probably invalid.",
					               Tiles[x, y], x, y);
					continue;
				}

				Surface surface = Tile.GetEditorSurface();
				if(surface == null)
					continue;

				context.DrawSurface(surface,
				                    new Vector(x * tile_width, y * tile_height),
				                    ZPos);
			}
		}
	}
}
