using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace OpenGl
{
	public static class glHelper
	{
		private static HashSet<string> extensions;
		/// <summary>
		/// Wrapper around glGetString(GL_EXTENSIONS).
		/// </summary>
		public static HashSet<string> Extensions {
			get {
				return extensions;
			}
		}

		static glHelper() {
			IntPtr extstr = gl.GetString(gl.EXTENSIONS);
			string exts = Marshal.PtrToStringAuto(extstr);
			extensions = new HashSet<string>(exts.Split(' '));
		}
	}
}
