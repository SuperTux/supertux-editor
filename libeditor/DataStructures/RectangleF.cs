//  $Id$
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

		public RectangleF(Vector v1, Vector v2)
		{
			Top =    (v1.Y < v2.Y)?v1.Y:v2.Y;
			Left =   (v1.X < v2.X)?v1.X:v2.X;
			Bottom = (v1.Y > v2.Y)?v1.Y:v2.Y;
			Right =  (v1.X > v2.X)?v1.X:v2.X;
		}

		public void Move(Vector vec)
		{
			Left += vec.X;
			Right += vec.X;
			Top += vec.Y;
			Bottom += vec.Y;
		}

		public void MoveTo(Vector vec)
		{
			Move(new Vector(vec.X - Left, vec.Y - Top));
		}

		public bool Contains(Vector v)
		{
			return v.X >= Left && v.X < Right && v.Y >= Top && v.Y < Bottom;
		}

		public bool Contains(RectangleF rect)
		{
			return rect.Left >= Left && rect.Right <= Right && rect.Top >= Top && rect.Bottom <= Bottom;
		}

		public static bool operator ==(RectangleF r1, RectangleF r2) {
			return r1.Left == r2.Left && r1.Top == r2.Top && r1.Right == r2.Right && r1.Bottom == r2.Bottom;
		}

		public static bool operator !=(RectangleF r1, RectangleF r2) {
			return r1.Left != r2.Left || r1.Top != r2.Top || r1.Right != r2.Right || r1.Bottom != r2.Bottom;
		}

		public static explicit operator Gdk.Rectangle(RectangleF r) {
			return new Gdk.Rectangle((int) r.Left, (int) r.Top, (int) r.Width, (int) r.Height);
		}

		public override bool Equals(object obj) {
			if (!(obj is RectangleF))
				return false;
			RectangleF rect = (RectangleF)obj;
			return this == rect;
		}

		public override int GetHashCode() {
			return base.GetHashCode();
		}

	}

}
