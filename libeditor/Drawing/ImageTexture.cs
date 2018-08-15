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
using System.IO;

namespace Drawing
{

	public sealed class ImageTexture : Texture
	{
		public float ImageWidth;
		public float ImageHeight;
		public int refcount;

		/// <summary>
		/// Reference to the Gdk.Pixbuf containing the texture image. Needed for lazy loading textures
		/// </summary>
		private Gdk.Pixbuf pixbuf;

		public ImageTexture(Stream input)
		{
			Gdk.Pixbuf pixbuf = new Gdk.Pixbuf(input);

			uint width = (uint)pixbuf.Width;
			uint height = (uint)pixbuf.Height;


			// Round to next power-of-two isn't needed with newer OpenGL versions
			if (!GlHelper.HasExtension("GL_ARB_texture_non_power_of_two")) {
				width  = NextPowerOfTwo((uint)width);
				height = NextPowerOfTwo((uint)height);
			}

			Gdk.Pixbuf target = new Gdk.Pixbuf(Gdk.Colorspace.Rgb, true, 8, (int)width, (int)height);
			pixbuf.CopyArea(0, 0, pixbuf.Width, pixbuf.Height, target, 0, 0);

			ImageWidth = (float) pixbuf.Width;
			ImageHeight = (float) pixbuf.Height;

			this.width = (uint)target.Width;
			this.height = (uint)target.Height;

			this.pixbuf = target;
		}

		protected override void Create()
		{
			// Not needed on newer OpenGL
			if (!GlHelper.HasExtension("GL_ARB_texture_non_power_of_two"))
			{
				if(!IsPowerOf2(width) || !IsPowerOf2(height))
					throw new Exception("Texture size must be power of 2");
			}

			GlUtil.Assert("before creating texture");
			CreateTexture();

			try {
				gl.BindTexture(gl.TEXTURE_2D, Handle);

				gl.TexImage2D(gl.TEXTURE_2D, 0, (int)gl.RGBA,
				              (int) width, (int) height, 0, (pixbuf.HasAlpha ? gl.RGBA : gl.RGB),
				              gl.UNSIGNED_BYTE, pixbuf.Pixels);
				GlUtil.Assert("creating texture (too big?)");

				SetTextureParams();

				this.pixbuf = null; //gc can take it now
			} catch(Exception) {
				uint[] handles = { Handle };
				gl.DeleteTextures(1, handles);
				throw;
			}

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
