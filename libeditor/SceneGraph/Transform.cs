//  $Id$
using System;
using DataStructures;
using OpenGl;

namespace SceneGraph
{

	public sealed class Transform : NodeWithChilds
	{
		public Matrix Matrix;

		public override unsafe void Draw()
		{
			gl.PushMatrix();
			gl.MultMatrixf(Matrix.Elements);

			DrawChilds();

			gl.PopMatrix();
		}
	}

}
