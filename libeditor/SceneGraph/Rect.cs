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
using DataStructures;
using OpenGl;

namespace SceneGraph
{

	/// <summary>
	/// Scene Graph nodes which draws a 2 dimensional rectangle (without
	/// textures, just plain color, filled or not filled)
	/// </summary>
	public sealed class Rectangle : Node
	{
		public RectangleF Rect;
		public bool Fill;

		public Rectangle()
		{
		}

		public void Draw(Gdk.Rectangle cliprect)
		{
			gl.Disable(gl.TEXTURE_2D);
			gl.PolygonMode(gl.FRONT_AND_BACK, Fill ? gl.FILL : gl.LINE);

			gl.Begin(gl.QUADS);
			gl.Vertex2f(Rect.Left, Rect.Top);
			gl.Vertex2f(Rect.Right, Rect.Top);
			gl.Vertex2f(Rect.Right, Rect.Bottom);
			gl.Vertex2f(Rect.Left, Rect.Bottom);
			gl.End();

			gl.PolygonMode(gl.FRONT_AND_BACK, gl.FILL);
			gl.Enable(gl.TEXTURE_2D);
		}
	}

}

/* EOF */
