//  SuperTux Editor
//  Copyright (C) 2006 Matthias Braun <matze@braunis.de>
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

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

			uint width  = (uint)surface->w;
			uint height = (uint)surface->h;

			// Round to next power-of-two isn't needed with newer OpenGL versions
			if (false)
			{
				width  = NextPowerOfTwo((uint)width);
				height = NextPowerOfTwo((uint)height);
			}

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
				return ImageWidth / (float) Width;
			}
		}

		public float UVBottom
		{
			get
			{
				return ImageHeight / (float) Height;
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

/* EOF */
