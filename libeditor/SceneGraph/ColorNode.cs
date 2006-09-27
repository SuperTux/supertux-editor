//  $Id$
using System;
using OpenGl;
using Drawing;

namespace SceneGraph
{

	public class ColorNode : Node
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

		public void Draw()
		{
			if(Child == null)
				return;

			gl.Color4f(Color.Red, Color.Green, Color.Blue, Color.Alpha);
			Child.Draw();
			gl.Color4f(0, 0, 0, 1f);
		}
	}

}
