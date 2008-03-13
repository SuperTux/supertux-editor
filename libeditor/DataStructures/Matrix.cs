//  $Id$
using System;

namespace DataStructures
{
	public struct Matrix
	{
		public float[] Elements;
		
		Matrix(float r11, float r12, float r13, float r14,
		       float r21, float r22, float r23, float r24,
			   float r31, float r32, float r33, float r34,
			   float r41, float r42, float r43, float r44) {
			Elements = new float[16];
			Elements[0] = r11;
			Elements[1] = r12;
			Elements[2] = r13;
			Elements[3] = r14;
			Elements[4] = r21;
			Elements[5] = r22;
			Elements[6] = r23;
			Elements[7] = r24;
			Elements[8] = r31;
			Elements[9] = r32;
			Elements[10] = r33;
			Elements[11] = r34;
			Elements[12] = r41;
			Elements[13] = r42;
			Elements[14] = r43;
			Elements[15] = r44;
		}

		Matrix(Matrix other) {
			Elements = new float[16];
			for(int i = 0; i < 16; ++i) {
				Elements[i] = other.Elements[i];
			}
		}

		Matrix Identity() {
			return new Matrix(
				1, 0, 0, 0,
				0, 1, 0, 0,
				0, 0, 1, 0,
				0, 0, 0, 1);
		}

		public Matrix Multiply(Matrix mult) {
			Matrix result = Identity();
			for (int x=0; x<4; x++) {
				for (int y=0; y<4; y++)	{
					result.Elements[x+y*4] =
						Elements[x]    * mult.Elements[y*4] +
						Elements[x+4]  * mult.Elements[y*4+1] +
						Elements[x+8]  * mult.Elements[y*4+2] +
						Elements[x+12] * mult.Elements[y*4+3];
				}
			}

			return result;
		}

		public Matrix Translate(float x, float y, float z) {
			Matrix multmatrix = Identity();
			multmatrix.Elements[12] = x;
			multmatrix.Elements[13] = y;
			multmatrix.Elements[14] = z;
			return Multiply(multmatrix);
		}

		public Matrix Rotate(float angle, float x, float y, float z) {
			double len = x*x + y*y + z*z;
			if (len != 1.0)
			{
				float len2 = (float) Math.Sqrt(len);
				x /= len2;
				y /= len2;
				z /= len2;
			}

			float c = (float) Math.Cos(angle*3.14159265/180);
		  	float s = (float) Math.Sin(angle*3.14159265/180);

			Matrix mult = Identity();

			mult.Elements[0]  = x*x*(1-c) + c;
			mult.Elements[1]  = y*x*(1-c) + z*s;
			mult.Elements[2]  = x*z*(1-c) - y*s;

			mult.Elements[4]  = x*y*(1-c) - z*s;
			mult.Elements[5]  = y*y*(1-c) + c;
			mult.Elements[6]  = y*z*(1-c) + x*s;

			mult.Elements[8]  = x*z*(1-c) + y*s;
			mult.Elements[9]  = y*z*(1-c) - x*s;
			mult.Elements[10] = z*z*(1-c) + c;

			return Multiply(mult);
		}
	}
}
