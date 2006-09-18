using System;
using System.Text;
using DataStructures;
using Lisp;
using LispReader;
using System.Collections.Generic;

[LispRoot("tileblock")]
public class TileBlock : Field<int>, ICustomLispSerializer {
	public int TileListFirstTile = -1;
	public int TileListW, TileListH;

	public TileBlock() : base() {
	}

	public TileBlock(uint Width, uint Height, int FillValue) : base(Width, Height, FillValue) {
	}

	/// <summary>
	/// Clone Subset of other TileBlock
	/// </summary>
	public TileBlock(TileBlock Other, int startX, int startY, uint width, uint height) : base(Other, startX, startY, width, height) {
	}

	public void Draw(Vector Pos, Tileset Tileset) {
		Vector CurrentPos = Pos;
		for(uint y = 0; y < Height; ++y) {
			for(uint x = 0; x < Width; ++x) {
				int TileId = this[x, y];
				Tile Tile = Tileset.Get(TileId);
				Tile.DrawEditor(CurrentPos);
				CurrentPos.X += Tileset.TILE_HEIGHT;
			}
			CurrentPos.X = Pos.X;
			CurrentPos.Y += Tileset.TILE_WIDTH;
		}
	}

	public void ApplyToTilemap(FieldPos pos, Tilemap Tilemap, bool skipNull) {
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
				if ((skipNull) && (this[x, y] == 0) && (Width > 1 || Height > 1)) continue;
				Tilemap[(uint) (pos.X + x), (uint) (pos.Y + y)] = this[x, y];
			}
		}
	}

	public void ApplyToTilemap(FieldPos pos, Tilemap Tilemap) {
		ApplyToTilemap(pos, Tilemap, true);
	}

	public void CustomLispRead(Properties Props) {
		uint Width = 0;
		uint Height = 0;
		Props.Get("width", ref Width);
		Props.Get("height", ref Height);
		if(Width == 0 || Height == 0) throw new LispException("Width or Height of TileBlock invalid");

		List<int> Tiles = new List<int>();
		Props.GetIntList("tiles", Tiles);
		if(Tiles.Count != (int) (Width * Height)) throw new LispException("TileCount != Width*Height: " + Tiles.Count + " != " + (int)Width + "*" + (int)Height);

		Assign(Tiles, Width, Height);
	}

	public void CustomLispWrite(Writer Writer) {
		Writer.Write("width", Width);
		Writer.Write("height", Height);
		Writer.WriteVerbatimLine("(tiles");
		for (uint y = 0; y < Height; ++y) {
			StringBuilder line = new StringBuilder();
			for (uint x = 0; x < Width; ++x) {
				if(x != 0)
					line.Append(" ");
				line.Append(this[x, y]);
			}
			Writer.WriteVerbatimLine(line.ToString());
		}
		Writer.WriteVerbatimLine(")");
	}

	public void FinishRead() {
	}

}
