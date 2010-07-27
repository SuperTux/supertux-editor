//  SuperTux Editor
//  Copyright (C) 2006 Matthias Braun <matze@braunis.de>
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Text;
using DataStructures;
using Lisp;
using LispReader;
using System.Collections.Generic;

[LispRoot("tileblock")]
public class TileBlock : ICustomLispSerializer, IComparable {
	public int TileListFirstTile = -1;
	private Field<int> field; 

	public TileBlock()
	{
		field = new Field<int>();
	}

	public TileBlock(int width, int height, int fillValue)
	{
		field = new Field<int>(width, height, fillValue);
	}

	/// <summary>
	/// Clone Subset of other TileBlock
	/// </summary>
	public TileBlock(TileBlock other, int startX, int startY, int width, int height)
	{
		field = new Field<int>(other.field, startX, startY, width, height, 0);
	}

	public int Width 
	{
		get { return field.Width; }
	}

	public int Height 
	{
		get { return field.Height; }
	}

	public void Resize(int newWidth, int newHeight, int fillValue)
	{
		field.Resize(newWidth, newHeight, fillValue);
	}

	public void Resize(int xOffset, int yOffset, int newWidth, int newHeight, int fillValue)
	{
		field.Resize(xOffset, yOffset, newWidth, newHeight, fillValue);
	}

	public bool EqualContents(TileBlock rhs)
	{
		return field.EqualContents(rhs.field);
	}

	public int this[int x, int y]
	{
		get {
			return field[x, y];
		}
		set {
			field[x, y] = value;
		}
	}

	public int this[FieldPos pos]
	{
		get {
			return field[pos];
		}
		set {
			field[pos] = value;
		}
	}

	public bool InBounds(FieldPos pos) 
	{
		return field.InBounds(pos);
	}

	public void Draw(Vector Pos, Tileset Tileset) {
		Vector CurrentPos = Pos;
		for(int y = 0; y < Height; ++y) {
			for(int x = 0; x < Width; ++x) {
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
		int StartX = Math.Max(0, -pos.X);
		int StartY = Math.Max(0, -pos.Y);
		int W = Math.Min(Tilemap.Width  - pos.X, Width);
		int H = Math.Min(Tilemap.Height - pos.Y, Height);
		for(int y = StartY; y < H; ++y) {
			for(int x = StartX; x < W; ++x) {
				if ((skipNull) && (this[x, y] == 0) && (Width > 1 || Height > 1)) continue;
				Tilemap[ pos.X + x, pos.Y + y ] = this[x, y];
			}
		}
	}

	public void ApplyToTilemap(FieldPos pos, Tilemap Tilemap) {
		ApplyToTilemap(pos, Tilemap, true);
	}

	public void CustomLispRead(Properties Props) {
		int Width = 0;
		int Height = 0;
		Props.Get("width", ref Width);
		Props.Get("height", ref Height);
		if(Width == 0 || Height == 0) throw new LispException("Width or Height of TileBlock invalid");

		List<int> Tiles = new List<int>();
		Props.GetIntList("tiles", Tiles);
		if(Tiles.Count != (int) (Width * Height)) throw new LispException("TileCount != Width*Height: " + Tiles.Count + " != " + (int)Width + "*" + (int)Height);

		field.Assign(Tiles, Width, Height);
	}

	public void CustomLispWrite(Writer Writer) {
		Writer.Write("width", Width);
		Writer.Write("height", Height);
		Writer.WriteVerbatimLine("(tiles");
		for (int y = 0; y < Height; ++y) {
			StringBuilder line = new StringBuilder();
			for (int x = 0; x < Width; ++x) {
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
			for (int i = 0; i < Math.Min(field.Elements.Count, tileblock.field.Elements.Count); i++) {
				if (field.Elements[i] == tileblock.field.Elements[i])
					continue;
				if (field.Elements[i] > tileblock.field.Elements[i])
					return 1;
				else
					return -1;
			}
			// Same data up to the last index of the smallest one at least.
			if (field.Elements.Count == tileblock.field.Elements.Count)
				return 0;
			if (field.Elements.Count > tileblock.field.Elements.Count)
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
        x = x*23+Width;
        x = x*23+Height;
        x = x*23+field.Elements.GetHashCode();
        return x;
    }

	#endregion

	internal struct StateData {
		public List<int> Elements;
		public int width;
		public int height;
		public StateData(int width, int height, List<int> Elements) {
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
		return new StateData(Width, Height, field.Elements);
	}

	/// <summary>
	/// Data for undoing
	/// </summary>
	/// <returns>Data for undoing</returns>
	internal void RestoreState(StateData state) {
		field = new Field<int>(state.Elements, state.width, state.height);
	}
}

/* EOF */
