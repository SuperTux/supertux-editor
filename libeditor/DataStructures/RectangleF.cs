namespace DataStructures
{

	public struct RectangleF
	{
		public float Left;
		public float Top;
		public float Right;
		public float Bottom;

		public float Width
		{
			get
			{
				return Right - Left;
			}
			set
			{
				if(value >= 0)
					Right = Left + value;
				else
					Left = Right + value;
			}
		}
		public float Height
		{
			get
			{
				return Bottom - Top;
			}
			set
			{
				if(value >= 0)
					Bottom = Top + value;
				else
					Top = Bottom + value;
			}
		}

		public RectangleF(float Left, float Top, float Width, float Height)
		{
			this.Top = Top;
			this.Left = Left;
			Bottom = Top + Height;
			Right = Left + Width;
		}

		public void Move(Vector vec)
		{
			Left += vec.X;
			Right += vec.X;
			Top += vec.Y;
			Bottom += vec.Y;
		}

		public bool Contains(Vector v)
		{
			return v.X >= Left && v.X < Right && v.Y >= Top && v.Y < Bottom;
		}
	}

}
