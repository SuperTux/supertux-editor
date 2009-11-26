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

namespace DataStructures
{

	public struct Vector
	{
		public float X;
		public float Y;

		public Vector(float X, float Y)
		{
			this.X = X;
			this.Y = Y;
		}

		public float Norm()
		{
			return (float) Math.Sqrt(X*X + Y*Y);
		}

		public Vector Unit()
		{
			return this / Norm();
		}

		public static Vector operator +(Vector v1, Vector v2)
		{
			return new Vector(v1.X + v2.X, v1.Y + v2.Y);
		}

		public static Vector operator -(Vector v1, Vector v2)
		{
			return new Vector(v1.X - v2.X, v1.Y - v2.Y);
		}

		public static Vector operator -(Vector v)
		{
			return new Vector(-v.X, -v.Y);
		}

		public static Vector operator *(Vector v1, float s)
		{
			return new Vector(v1.X * s, v1.Y * s);
		}

		/// <summary>scalar product</summary>
		public static float operator *(Vector v1, Vector v2)
		{
			return v1.X * v2.X + v1.Y * v2.Y;
		}

		public static Vector operator /(Vector v1, float s)
		{
			return new Vector(v1.X / s, v1.Y / s);
		}

		public static bool operator ==(Vector v1, Vector v2)
		{
			return v1.X == v2.X && v1.Y == v2.Y;
		}

		public static bool operator !=(Vector v1, Vector v2)
		{
			return v1.X != v2.X || v1.Y != v2.Y;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Vector))
				return false;
			Vector ov = (Vector) obj;
			return this == ov;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string ToString()
		{
			return "[" + X + ";" + Y + "]";
		}
	}

}

/* EOF */
