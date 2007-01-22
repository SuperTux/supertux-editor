//  $Id$
using SceneGraph;
using DataStructures;
using System;

/// <summary>
///	A SceneGraph Node which draws a tilemap
/// </summary>
public class TilemapNode : Node {
	private Field<int> Field;
	private Tileset Tileset;

	public TilemapNode(Field<int> Field, Tileset Tileset) {
		this.Field = Field;
		this.Tileset = Tileset;
	}

	public void Draw() {
	// TODO: calculate really visible rectangle
		uint start_x = 0;
		uint start_y = 0;
		uint end_x = Field.Width;
		uint end_y = Field.Height;
		for(uint y = start_y; y < end_y; ++y) {
			for(uint x = start_x; x < end_x; ++x) {
				int TileId = Field[x, y];
				Tile Tile = Tileset.Get(TileId);
				if(Tile != null)
					Tile.DrawEditor(new Vector(x * 32, y * 32));
			}
		}
	}
}
