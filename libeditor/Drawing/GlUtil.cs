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
using OpenGl;
using OpenGlUtil;

namespace Drawing
{

	public static class GlUtil
	{
		/// <summary>set to false if there's no valid opengl context active</summary>
		public static bool ContextValid;

		public static void Assert(string message)
		{
			uint error = gl.GetError();
			if(error != gl.NO_ERROR) {
				throw new Exception("OpenGL error while '" + message + "': "
				                    + glu.ErrorString(error) + " (" + error + ")");
			}
		}
	}

}

/* EOF */
