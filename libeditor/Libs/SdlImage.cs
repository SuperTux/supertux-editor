using System;
using System.Runtime.InteropServices;
using Sdl;
using System.Security;

namespace Sdl.Image
{

	public static class IMG
	{
		private const string SDLIMAGE_DLL = "sdl_image.dll";

		[DllImport(SDLIMAGE_DLL, EntryPoint = "IMG_Load"), SuppressUnmanagedCodeSecurityAttribute]
		public static extern IntPtr /*Surface*/ Load(string filename);
	}
}
