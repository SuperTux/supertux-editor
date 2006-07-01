using System;
using OpenGl;
using OpenGlUtil;

namespace Drawing
{

	public class GlUtil
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
