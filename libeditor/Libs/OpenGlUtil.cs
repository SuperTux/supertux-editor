using System;
using System.Runtime.InteropServices;

namespace OpenGlUtil
{
	public class glu
	{
		private const string GLU_DLL = "GLU.dll";

		[DllImport(GLU_DLL, EntryPoint = "gluErrorString")]
		private static extern IntPtr _ErrorString(uint error);

		public static string ErrorString(uint error)
		{
			IntPtr errorstr = _ErrorString(error);
			return Marshal.PtrToStringAuto(errorstr);
		}
	}
}
