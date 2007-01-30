//  $Id$
using System;
using Sdl;
using OpenGl;

namespace Drawing
{

	public sealed class ImageTexture : Texture
	{
		public float ImageWidth;
		public float ImageHeight;
		public int refcount;

		public ImageTexture(IntPtr surface)
		{
			Create(surface);
		}

		private unsafe void Create(IntPtr sdlsurface)
		{
			Sdl.Surface* surface = (Sdl.Surface*) sdlsurface;
			uint width = NextPowerOfTwo((uint) surface->w);
			uint height = NextPowerOfTwo((uint) surface->h);

			IntPtr pixelbufferp;
			if(BitConverter.IsLittleEndian) {
				pixelbufferp = SDL.CreateRGBSurface(SDL.SWSURFACE,
				                                    (int) width, (int) height, 32,
				                                    0x000000ff, 0x0000ff00, 0x00ff0000, 0xff000000);
			} else {
				pixelbufferp = SDL.CreateRGBSurface(SDL.SWSURFACE,
				                                    (int) width, (int) height, 32,
				                                    0xff000000, 0x00ff0000, 0x0000ff00, 0x000000ff);
			}
			if(pixelbufferp == IntPtr.Zero)
				throw new Exception("Couldn't create surface texture (out of memory?)");

			try {
				SDL.SetAlpha(sdlsurface, 0, 0);
				SDL.BlitSurface(sdlsurface, IntPtr.Zero, pixelbufferp, IntPtr.Zero);

				CreateFromSurface(pixelbufferp, gl.RGBA);
			} finally {
				SDL.FreeSurface(pixelbufferp);
			}

			ImageWidth = (float) surface->w;
			ImageHeight = (float) surface->h;
		}

		private static uint NextPowerOfTwo(uint val)
		{
			uint result = 1;
			while(result < val)
				result *= 2;
			return result;
		}

		public float UVRight
		{
			get
			{
				return (ImageWidth - 0.5f) / (float) Width;
			}
		}

		public float UVBottom
		{
			get
			{
				return (ImageHeight - 0.5f) / (float) Height;
			}
		}

		public void Ref()
		{
			refcount++;
		}

		public void UnRef()
		{
			refcount--;
			if(refcount == 0)
				Dispose();
		}
	}

}
