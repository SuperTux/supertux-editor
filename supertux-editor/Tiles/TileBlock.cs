//  $Id$
using System;
using System.Text;
using DataStructures;
using Lisp;
using LispReader;
using System.Collections.Generic;

[LispRoot("tileblock")]
public class TileBlock : Field<int>, ICustomLispSerializer, IComparable {
	public int TileListFirstTile = -1;
	public int TileListW, TileListH;

	public TileBlock() : base() {
	}

	public TileBlock(uint Width, uint Height, int FillValue) : base(Width, Height, FillValue) {
	}

	/// <summary>
	/// Clone Subset of other TileBlock
	/// </summary>
	public TileBlock(TileBlock Other, int startX, int startY, uint width, uint height) : base(Other, startX, startY, width, height, 0) {
	}

	public void Draw(Vector Pos, Tileset Tileset) {
		Vector CurrentPos = Pos;
		for(uint y = 0; y < Height; ++y) {
			for(uint x = 0; x < Width; ++x) {
				int TileId = this[x, y];
				Tile Tile = Tileset.Get(TileId);
				if (Tile != null)
					Tile.DrawEditor(CurrentPos);
				else
					LogManager.Log(LogLevel.Warning,
					               "Tile {0} is null?! The tile with id {0} at {1},{2} is probably invalid.",
					               TileId, x, y);
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

	#region IComparable Members
	/// <summary>
	///		Compares the current instance with another object of the same type.
	/// </summary>
	/// <remarks>
	///		Only reliable to keep lists of <see cref="TileBlock"/> sorted,
	///		it may or may not be a logical sorting but that wasn't the goal either.
	/// </remarks>
	/// <param name="obj">An object to compare with this instance. </param>
	/// <returns>
	///		A 32-bit signed integer that indicates the relative order of the
	///		objects being compared. For meanings of the meaning of these values see
	///		<see cref="IComparable.CompareTo"/>.
	/// </returns>
	/// <seealso cref="IComparable.CompareTo"/>
	int IComparable.CompareTo(object obj) {
		if (obj == null) return 1;
		TileBlock tileblock = obj as TileBlock;
		if (tileblock != null) {
			for (int i = 0; i < Math.Min(this.Elements.Count, tileblock.Elements.Count); i++) {
				if (this.Elements[i] == tileblock.Elements[i])
					continue;
				if (this.Elements[i] > tileblock.Elements[i])
					return 1;
				else
					return -1;
			}
			// Same data up to the last index of the smallest one at least.
			if (this.Elements.Count == tileblock.Elements.Count)
				return 0;
			if (this.Elements.Count > tileblock.Elements.Count)
				return 1;
			else
				return -1;
		}

		throw new ArgumentException("object is not a TileBlock");
	}

    public override bool Equals(Object tb)
    {
       return this == tb;
    }

    public override int GetHashCode() {
        int x = 13;
        x = x*23+(int)width;
        x= x*23+(int)height;
        x=x*23+Elements.GetHashCode();
        return x;
    }
	
	#endregion

	internal struct StateData {
		public List<int> Elements;
		public uint width;
		public uint height;
		public StateData(uint width, uint height, List<int> Elements) {
			this.width = width;
			this.height = height;
			// This we need to clone.
			this.Elements = new List<int>(Elements);
		}
	}

	/// <summary>
	/// Data for undoing
	/// </summary>
	/// <returns>Data for undoing</returns>
	internal StateData SaveState() {
		return new StateData(Width, Height, Elements);
	}

	/// <summary>
	/// Data for undoing
	/// </summary>
	/// <returns>Data for undoing</returns>
	internal void RestoreState(StateData state) {
		width = state.width;
		height = state.height;
		Elements = new List<int>(state.Elements);
	}

}
