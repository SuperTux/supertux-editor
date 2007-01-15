using System;
using System.Collections.Generic;

namespace DataStructures
{

	/**
	 * This class represents a dynamic 2-dimensional array
	 */
	public class Field<T> {
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

	    public uint Width {
	        get {
	            return width;
	        }
	    }

	    public uint Height {
	        get {
	            return height;
	        }
	    }

	    public T this[uint X, uint Y] {
	        get {
	            return Elements[(int) (Y * width + X)];
	        }
	        set {
	            Elements[(int) (Y * width + X)] = value;
	        }
	    }

		public T this[int X, int Y] {
	        get {
	            return Elements[Y * (int) width + X];
	        }
	        set {
	            Elements[Y * (int) width + X] = value;
	        }
	    }

	    public T this[FieldPos Pos] {
	        get {
	            return this[(uint) Pos.X, (uint) Pos.Y];
	        }
	        set {
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
	                if(y < Width && x < Height)
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
	}

}
