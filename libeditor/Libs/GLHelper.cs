using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace OpenGl
{
	public static class glHelper
	{

#if WINDOWS
		//FIXME: this is incredibly wrong
		//TODO: M$ C# has no HashSet support, plese fix this awful workaround
		//string "exts" can be static, but it won't solve the problem
		public static bool HasExtension(string name) {
			IntPtr extstr = gl.GetString(gl.EXTENSIONS);
			string exts = Marshal.PtrToStringAuto(extstr);
			return exts.Contains(name);
		}
#else
		private static HashSet<string> extensions;
		/// <summary>
		/// Wrapper around glGetString(GL_EXTENSIONS).
		/// </summary>
		public static HashSet<string> Extensions {
			get {
				return extensions;
			}
		}

		public static bool HasExtension(string name) {
			return Extensions.Contains(name);
		}

		static glHelper() {
			IntPtr extstr = gl.GetString(gl.EXTENSIONS);
			string exts = Marshal.PtrToStringAuto(extstr);
			extensions = new HashSet<string>(exts.Split(' '));
		}
#endif
	}
}
