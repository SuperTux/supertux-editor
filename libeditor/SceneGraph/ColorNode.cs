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
using Drawing;

namespace SceneGraph {

	/// <summary>
	/// 	Scene graph node that changes the drawing color. Note that you can make
	/// 	stuff (half-)transparent by changing the alpha value of the drawing
	/// 	color.
	/// </summary>
	/// <remarks>
	/// 	If <see cref="CanSkip"/> is set and the alpha value is 0 then this scene graph node will not call
	/// 	Draw of the child at all.
	/// </remarks>
	public sealed class ColorNode : Node {
		public Color Color;
		public Node Child;
		public bool CanSkip;

		public ColorNode() {
		}

		/// <summary>
		/// 	Create a new <see cref="ColorNode"/>.
		/// </summary>
		/// <param name="Child">The child scene graph <see cref="Node"/>.</param>
		/// <param name="Color">The color to use.</param>
		public ColorNode(Node Child, Color Color) {
			this.Child = Child;
			this.Color = Color;
		}

		/// <summary>
		/// 	Create a new <see cref="ColorNode"/>.
		/// </summary>
		/// <param name="Child">The child scene graph <see cref="Node"/>.</param>
		/// <param name="Color">The color to use.</param>
		/// <param name="CanSkip">If true we can skip drawing when <see cref="Drawing.Color.Alpha"/> is 0.</param>
		public ColorNode(Node Child, Color Color, bool CanSkip) {
			this.Child = Child;
			this.Color = Color;
			this.CanSkip = CanSkip;
		}

		public void Draw(Gdk.Rectangle cliprect) {
			if (Child == null)
				return;

			// Can we skip drawing at all if Color.Alpha == 0?
			if (CanSkip && Color.Alpha == 0f) return;

			gl.Color4f(Color.Red, Color.Green, Color.Blue, Color.Alpha);
			Child.Draw(cliprect);
			gl.Color4f(0, 0, 0, 1f);
		}
	}

}

/* EOF */
