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

namespace Drawing
{

	public struct Color
	{
		/// <summary>red part of the color</summary>
		public float Red;
		/// <summary>green part of the color</summary>
		public float Green;
		/// <summary>blue part of the color</summary>
		public float Blue;
		/// <summary>alpha part of the color</summary>
		/// <remarks>1.0 = full opaque, 0.0 = invisible</remarks>
		public float Alpha;

		public Color(float Red, float Green, float Blue, float Alpha)
		{
			this.Red = Red;
			this.Green = Green;
			this.Blue = Blue;
			this.Alpha = Alpha;
		}

		public Color(float Red, float Green, float Blue)
		{
			this.Red = Red;
			this.Green = Green;
			this.Blue = Blue;
			this.Alpha = 1.0f;
		}

		public static bool operator ==(Color c1, Color c2) {
			return c1.Red == c2.Red && c1.Green == c2.Green && c1.Blue == c2.Blue && c1.Alpha == c2.Alpha;
		}

		public static bool operator !=(Color c1, Color c2) {
			return c1.Red != c2.Red || c1.Green != c2.Green || c1.Blue != c2.Blue || c1.Alpha != c2.Alpha;
		}

		public override bool Equals(object obj) {
			if (!(obj is Color))
				return false;
			Color color = (Color)obj;
			return this == color;
		}

		public override int GetHashCode() {
			return base.GetHashCode();
		}

	}

}

/* EOF */
