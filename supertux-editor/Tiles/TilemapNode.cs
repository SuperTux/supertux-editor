//  $Id$
using SceneGraph;
using DataStructures;
using System;

/// <summary>
///	A SceneGraph Node which draws a tilemap
/// </summary>
public sealed class TilemapNode : Node {
	private Tilemap tilemap;
	private Tileset Tileset;

	public TilemapNode(Tilemap tilemap, Tileset Tileset) {
		this.tilemap = tilemap;
		this.Tileset = Tileset;
	}

	public void Draw(Gdk.Rectangle cliprect) {
		cliprect.X -= (int)tilemap.X;
		cliprect.Y -= (int)tilemap.Y;

		uint start_x = (uint) Math.Max(0, cliprect.X / 32 - 1);
		uint start_y = (uint) Math.Max(0, cliprect.Y / 32 - 1);
		//HACK: Couldn't find any "inrange" or such
		uint end_x = (uint) Math.Max(0, Math.Min(tilemap.Width, (cliprect.X + cliprect.Width) / 32 + 1));
		uint end_y = (uint) Math.Max(0, Math.Min(tilemap.Height, (cliprect.Y + cliprect.Height) / 32 + 1));
		Tile Tile;
		for (uint y = start_y; y < end_y; ++y) {
			for (uint x = start_x; x < end_x; ++x) {
				Tile = Tileset.Get(tilemap[x, y]);
				if (Tile != null)
					Tile.DrawEditor(new Vector(x * 32 + tilemap.X,
									y * 32 + tilemap.Y));
				else
					LogManager.Log(LogLevel.Warning,
					               "Tile {0} is null?! The tile with id {0} at {1},{2} is probably invalid.",
					               tilemap[x, y], x, y);
			}
		}
	}
}
