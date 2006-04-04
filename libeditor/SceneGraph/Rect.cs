using System;
using DataStructures;
using OpenGl;

namespace SceneGraph
{

	public class Rectangle : Node
	{
		public RectangleF Rect;
		public bool Fill;

		public Rectangle()
		{
		}

		public void Draw()
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
