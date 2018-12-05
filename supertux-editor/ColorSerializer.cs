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
using Drawing;
using Lisp;
using LispReader;

[LispCustomSerializer(typeof(Color))]
public class ColorSerializer : ILispSerializer
{
	public object Read(List list)
	{
		if(list.Length < 4 || list.Length > 5)
			throw new LispException(String.Format("Lisp list must have 4 or 5 entries for color: {0}", list));

		Color result = new Color();
		result.Red = GetFloat(list[1]);
		result.Green = GetFloat(list[2]);
		result.Blue = GetFloat(list[3]);
		if(list.Length == 5)
			result.Alpha = GetFloat(list[4]);
		else
			result.Alpha = 1.0f;

		return result;
	}

	public void Write(Writer writer, string name, object Object)
	{
		Color color = (Color) Object;
		if(color.Alpha != 1.0f) {
			float[] vals = new float[] { color.Red, color.Green, color.Blue, color.Alpha };
			writer.Write(name, vals);
		} else {
			float[] vals = new float[] { color.Red, color.Green, color.Blue };
			writer.Write(name, vals);
		}
	}

	private static float GetFloat(object obj)
	{
		if(obj is int)
			return (float) ((int) obj);

		return (float) obj;
	}
}

/* EOF */
