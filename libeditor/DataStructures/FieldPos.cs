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

	public struct FieldPos
	{
		public int X;
		public int Y;

		public FieldPos(int X, int Y)
		{
			this.X = X;
			this.Y = Y;
		}

		public static bool operator ==(FieldPos p1, FieldPos p2)
		{
			return p1.X == p2.X && p1.Y == p2.Y;
		}

		public static bool operator !=(FieldPos p1, FieldPos p2)
		{
			return p1.X != p2.X || p1.Y != p2.Y;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is FieldPos))
				return false;
			FieldPos pos = (FieldPos) obj;
			return this == pos;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public FieldPos Up
		{
			get {
				return new FieldPos(X, Y-1);
			}
		}

		public FieldPos Down
		{
			get {
				return new FieldPos(X, Y+1);
			}
		}

		public FieldPos Left
		{
			get {
				return new FieldPos(X-1, Y);
			}
		}

		public FieldPos Right
		{
			get {
				return new FieldPos(X+1, Y);
			}
		}

	}

}

/* EOF */
