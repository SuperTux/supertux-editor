//  $Id$
using SceneGraph;
using DataStructures;
using System;

/// <summary>
///	A SceneGraph Node which draws a tilemap
/// </summary>
public sealed class TilemapNode : Node {
	private Field<int> Field;
	private Tileset Tileset;

	public TilemapNode(Field<int> Field, Tileset Tileset) {
		this.Field = Field;
		this.Tileset = Tileset;
	}

	public void Draw(Gdk.Rectangle cliprect) {
		uint start_x = (uint) Math.Max(0, cliprect.X / 32 - 1);
		uint start_y = (uint) Math.Max(0, cliprect.Y / 32 - 1);
		//HACK: Couldn't find any "inrange" or such
		uint end_x = (uint) Math.Max(0, Math.Min(Field.Width, (cliprect.X + cliprect.Width) / 32 + 1));
		uint end_y = (uint) Math.Max(0, Math.Min(Field.Height, (cliprect.Y + cliprect.Height) / 32 + 1));
		Tile Tile;
		for (uint y = start_y; y < end_y; ++y) {
			for (uint x = start_x; x < end_x; ++x) {
				Tile = Tileset.Get(Field[x, y]);
				if (Tile != null)
					Tile.DrawEditor(new Vector(x * 32, y * 32));
				else
					LogManager.Log(LogLevel.Warning,
					               "Tile {0} is null?! The tile with id {0} at {1},{2} is probably invalid.",
					               Field[x, y], x, y);
			}
		}
	}
}
