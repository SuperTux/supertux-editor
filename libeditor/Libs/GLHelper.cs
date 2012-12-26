using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace OpenGl
{
	public static class GlHelper
	{
		private static HashSet<string> extensions = null;
		public static HashSet<string> Extensions {
			get {
				if (extensions == null) {
					IntPtr extstr = gl.GetString(gl.EXTENSIONS);
					if (extstr.Equals(IntPtr.Zero)) {
						LogManager.Log(LogLevel.Warning, "Couldn't receive a list of gl extensions (no context?)");
						return null;
					} else {
						string exts = Marshal.PtrToStringAuto(extstr);
						extensions = new HashSet<string>(exts.Split(' '));
						return extensions;
					}
				} else {
					return extensions;
				}
			}
		}

		/// <summary>
		/// Wrapper around glGetString(GL_EXTENSIONS)
		/// </summary>
		/// <returns>
		/// <c>true</c> if the specified extension is available; otherwise, <c>false</c>.
		/// </returns>
		public static bool HasExtension(string name) {
			if (Extensions != null) {
				return Extensions.Contains(name);
			} else {
				return false;
			}
		}
	}
}
