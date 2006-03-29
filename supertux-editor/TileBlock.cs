using System;
using DataStructures;

public class TileBlock : Field<uint> {
	public int TileListFirstTile = -1;
	public int TileListW, TileListH;
	
	public void Draw(Vector Pos, Tileset Tileset) {
		Vector CurrentPos = Pos;
		for(uint y = 0; y < Height; ++y) {
			for(uint x = 0; x < Width; ++x) {
				uint TileId = this[x, y];
				Tile Tile = Tileset.Get(TileId);
				Tile.Draw(CurrentPos);
				CurrentPos.X += Tileset.TILE_HEIGHT;
			}
			CurrentPos.X = Pos.X;
			CurrentPos.Y += Tileset.TILE_WIDTH;
		}
	}

	public void ApplyToTilemap(FieldPos pos, Tilemap Tilemap) {
		if(pos.X >= Tilemap.Width)
			return;
		if(pos.Y >= Tilemap.Height)
			return;
		
		uint StartX = (uint) Math.Max(0, -pos.X);
		uint StartY = (uint) Math.Max(0, -pos.Y);
		uint W = Math.Min((uint) (Tilemap.Width - pos.X), Width);
		uint H = Math.Min((uint) (Tilemap.Height - pos.Y), Height);
		for(uint y = StartY; y < H; ++y) {
			for(uint x = StartX; x < W; ++x) {
				if(this[x, y] == 0)
					continue;
				
				Tilemap[(uint) (pos.X + x), (uint) (pos.Y + y)] = this[x, y];
			}
		}
	}
}
