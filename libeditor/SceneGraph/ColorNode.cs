//  $Id$
using System;
using OpenGl;
using Drawing;

namespace SceneGraph
{

	/// <summary>
	/// Scene graph node that changes the drawing color. Note that you can make
	/// stuff (half-)transparent by changing the alpha value of the drawing
	/// color.
	/// </summary>
	public sealed class ColorNode : Node
	{
		public Color Color;
		public Node Child;

		public ColorNode()
		{
		}

		public ColorNode(Node Child, Color Color)
		{
			this.Child = Child;
			this.Color = Color;
		}

		public void Draw(Gdk.Rectangle cliprect)
		{
			if(Child == null)
				return;

			// TODO: Skip drawing at all if Color.Alpha == 0?
			gl.Color4f(Color.Red, Color.Green, Color.Blue, Color.Alpha);
			Child.Draw(cliprect);
			gl.Color4f(0, 0, 0, 1f);
		}
	}

}
