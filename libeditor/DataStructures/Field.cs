using System;
using System.Collections.Generic;

namespace DataStructures
{

	/// <summary>This class represents a dynamic 2-dimensional array</summary>
	public class Field<T>
	{
		private List<T> Elements = new List<T>();
		private uint width;
		private uint height;

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
					int tx = x + startX;
					int ty = y + startY;
					Elements.Add(Other[tx, ty]);
				}
			}
		}

		public uint Width
		{
			get
			{
				return width;
			}
		}

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
	}
}
