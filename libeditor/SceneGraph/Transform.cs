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
	/// A scene graph node which applies a transform to the current opengl
	/// transform (the matrix will be multiplied with the currently active
	/// tranform)
	/// </summary>
	public sealed class Transform : NodeWithChilds
	{
		public Matrix Matrix;

		public override unsafe void Draw(Gdk.Rectangle cliprect)
		{
			gl.PushMatrix();
			gl.MultMatrixf(Matrix.Elements);

			DrawChilds(cliprect);

			gl.PopMatrix();
		}
	}

}

/* EOF */
