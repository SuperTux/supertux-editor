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
using System.Collections.Generic;

namespace DataStructures
{
	/// <summary>This class represents a dynamic 2-dimensional array</summary>
	public class Field<T>
	{
		protected List<T> Elements = new List<T>();
		protected uint width;
		protected uint height;

		public Field()
		{
		}

		public Field(uint Width, uint Height, T FillValue)
		{
			this.width = Width;
			this.height = Height;
			for(uint i = 0; i < Width * Height; ++i)
				Elements.Add(FillValue);
		}

		public Field(List<T> Values, uint Width, uint Height)
		{
			Assign(Values, Width, Height);
		}

		/// <summary>
		/// Clone Subset of other field
		/// </summary>
		public Field(Field<T> Other, int startX, int startY, uint width, uint height) {
			this.width = width;
			this.height = height;
			if (startX < 0) throw new ArgumentOutOfRangeException("startX");
			if (startY < 0) throw new ArgumentOutOfRangeException("startY");
			if (startX + width > Other.Width) throw new ArgumentOutOfRangeException("startX");
			if (startY + height > Other.Height) throw new ArgumentOutOfRangeException("startY");
			for (int y = 0; y < height; y++) {
				for (int x = 0; x < width; x++) {
					Elements.Add(Other[x + startX, y + startY]);
				}
			}
		}

		/// <summary>
		/// Clone Subset of other field, filling in missing values with FillValue
		/// </summary>
		public Field(Field<T> Other, int startX, int startY, uint width, uint height, T FillValue) {
			this.width = width;
			this.height = height;
			for (int y = startY; y < startY + height; y++) {
				for (int x = startX; x < startX + width; x++) {
					if(x < 0 || y < 0 || x >= Other.Width || y >= Other.Height)
						Elements.Add(FillValue);
					else
						Elements.Add(Other[x, y]);
				}
			}
		}

		/// <summary>
		/// Width of array
		/// </summary>
		public uint Width
		{
			get
			{
				return width;
			}
		}

		/// <summary>
		/// Height of array
		/// </summary>
		public uint Height
		{
			get
			{
				return height;
			}
		}

		public T this[uint X, uint Y]
		{
			get
			{
				return Elements[(int) (Y * width + X)];
			}
			set
			{
				Elements[(int) (Y * width + X)] = value;
			}
		}

		public T this[int X, int Y]
		{
			get
			{
				return Elements[Y * (int) width + X];
			}
			set
			{
				Elements[Y * (int) width + X] = value;
			}
		}

		public T this[FieldPos Pos]
		{
			get
			{
				return this[(uint) Pos.X, (uint) Pos.Y];
			}
			set
			{
				this[(uint) Pos.X, (uint) Pos.Y] = value;
			}
		}

		public void Assign(List<T> Values, uint Width, uint Height)
		{
			if(Values.Count != Width * Height)
				throw new Exception("invalid size of value list for field");
			this.width = Width;
			this.height = Height;
			Elements.Clear();
			foreach(T val in Values) {
				Elements.Add(val);
			}
		}

		public void Resize(uint NewWidth, uint NewHeight, T FillValue)
		{
			List<T> NewElements = new List<T>();
			for(uint y = 0; y < NewHeight; ++y) {
				for(uint x = 0; x < NewWidth; ++x) {
					if(x < Width && y < Height)
						NewElements.Add(this[x, y]);
					else
						NewElements.Add(FillValue);
				}
			}
			Elements = NewElements;
			width = NewWidth;
			height = NewHeight;
		}

		public List<T> GetContentsArray()
		{
			List<T> Result = new List<T>(Elements);
			return Result;
		}

		public bool InBounds(FieldPos pos) {
			if (pos.X < 0) return false;
			if (pos.Y < 0) return false;
			if (pos.X >= Width) return false;
			if (pos.Y >= Height) return false;
			return true;
		}

		public bool EqualContents(object obj) {
			if (!(obj is Field<T>)) return false;
			Field<T> other = (Field<T>)obj;
			if (this.width != other.width) return false;
			if (this.height != other.height) return false;
			if (this.Elements.Count != other.Elements.Count) return false;
			for (int i = 0; i < this.Elements.Count; i++) {
				if (!this.Elements[i].Equals(other.Elements[i])) return false;
			}
			return true;
		}

	}
}

/* EOF */
