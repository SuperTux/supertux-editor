//  $Id$
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
