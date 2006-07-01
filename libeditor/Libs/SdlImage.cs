using System;
using System.Runtime.InteropServices;
using Sdl;

namespace Sdl.Image
{

	public class IMG
	{
		private const string SDLIMAGE_DLL = "sdl_image.dll";

		[DllImport(SDLIMAGE_DLL, EntryPoint = "IMG_Load")]
		public static extern IntPtr /*Surface*/ Load(string filename);
	}
}
