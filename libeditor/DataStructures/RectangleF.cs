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

		public Vector Center
		{
			get
			{
				return new Vector((Left + Right) / 2.0f,
						  (Top + Bottom) / 2.0f);
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

/* EOF */
