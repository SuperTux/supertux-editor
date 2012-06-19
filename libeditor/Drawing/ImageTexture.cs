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
using OpenGl;

namespace Drawing
{

	public sealed class ImageTexture : Texture
	{
		public float ImageWidth;
		public float ImageHeight;
		public int refcount;

		public ImageTexture(Gdk.Pixbuf surface)
		{
			Create(surface);
		}

		private unsafe void Create(Gdk.Pixbuf bmp)
		{
			uint width = (uint)bmp.Width;
			uint height = (uint)bmp.Height;
			
			// Round to next power-of-two isn't needed with newer OpenGL versions
			if (!glHelper.HasExtension("GL_ARB_texture_non_power_of_two")) {
				width  = NextPowerOfTwo((uint)width);
				height = NextPowerOfTwo((uint)height);
			}
			
			Gdk.Pixbuf target = new Gdk.Pixbuf(Gdk.Colorspace.Rgb, true, 8, (int)width, (int)height);
			bmp.CopyArea(0, 0, bmp.Width, bmp.Height, target, 0, 0);
			
			ImageWidth = (float) bmp.Width;
			ImageHeight = (float) bmp.Height;
			
			CreateFromPixbuf(target, gl.RGBA);
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
