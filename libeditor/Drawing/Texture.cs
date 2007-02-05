//  $Id$
using System;
using Sdl;
using OpenGl;

namespace Drawing
{

	public class Texture : IDisposable
	{
		private uint handle;
		public uint Handle
		{
			get
			{
				return handle;
			}
		}

		private uint width;
		public uint Width
		{
			get
			{
				return width;
			}
		}

		private uint height;
		public uint Height
		{
			get
			{
				return height;
			}
		}

		protected Texture()
		{
		}

		public Texture(uint width, uint height, uint glformat)
		{
			CreateEmpty(width, height, glformat);
		}

		public Texture(IntPtr surfacep, uint glformat)
		{
			CreateFromSurface(surfacep, glformat);
		}

		protected unsafe void CreateFromSurface(IntPtr surfacep, uint glformat)
		{
			Sdl.Surface* surface = (Sdl.Surface*) surfacep;
			this.width = (uint) surface->w;
			this.height = (uint) surface->h;
			if(!IsPowerOf2(width) || !IsPowerOf2(height))
				throw new Exception("Texture size must be power of 2");

			GlUtil.Assert("before creating texture");
			CreateTexture();

			try {
				gl.BindTexture(gl.TEXTURE_2D, handle);
				uint surface_format = SetupPixelFormat(surfacep);

				gl.TexImage2D(gl.TEXTURE_2D, 0, (int) glformat,
				              (int) width, (int) height, 0, surface_format,
				              gl.UNSIGNED_BYTE, surface->pixels);
				GlUtil.Assert("creating texture (too big?)");

				SetTextureParams();
			} catch(Exception) {
				uint[] handles = { handle };
				gl.DeleteTextures(1, handles);
				throw;
			}
		}

		protected void CreateEmpty(uint width, uint height, uint glformat)
		{
			if(!IsPowerOf2(width) || !IsPowerOf2(height))
				throw new Exception("Texture size must be power of 2");
			this.width = width;
			this.height = height;

			GlUtil.Assert("before creating texture");
			CreateTexture();

			try {
				gl.BindTexture(gl.TEXTURE_2D, handle);
				gl.TexImage2D(gl.TEXTURE_2D, 0, (int) glformat,
				              (int) width, (int) height, 0,
				              gl.RGBA, gl.UNSIGNED_BYTE, IntPtr.Zero);
				GlUtil.Assert("creating texture (too big?)");
				SetTextureParams();
			} catch(Exception) {
				uint[] handles = { handle };
				gl.DeleteTextures(1, handles);
				throw;
			}
		}

		public unsafe void Dispose()
		{
			if(handle == 0)
				return;

			uint[] handles = { handle };

			gl.DeleteTextures(1, handles);
			this.handle = 0;
		}

		public unsafe void copy_to(IntPtr surfacep, uint surface_x, uint surface_y,
		                           uint texture_x, uint texture_y,
		                           uint width, uint height)
		{
			Sdl.Surface* surface = (Sdl.Surface*) surfacep;
			PixelFormat* format = (PixelFormat*) surface->format;
			GlUtil.Assert("Before update textxure");

			uint surface_format = SetupPixelFormat(surfacep);


			/* We're extracting sub rectangles from the SDL_Surface pixeldata, by
			 * setting the pitch to the real width, but telling OpenGL just our
			 * desired image dimensions.
			 */
			IntPtr pixeldata = (IntPtr)
				(((byte*) surface->pixels) + surface_y * surface->pitch
				 + format->BytesPerPixel * surface_x);
			gl.TexSubImage2D(gl.TEXTURE_2D, 0, (int) texture_x, (int) texture_y,
			                 (int) width, (int) height, surface_format,
			                 gl.UNSIGNED_BYTE, pixeldata);

			GlUtil.Assert("Updating Texture Part");
		}

		private static void SetTextureParams()
		{
			gl.TexParameteri(gl.TEXTURE_2D, gl.TEXTURE_MIN_FILTER, gl.LINEAR);
			gl.TexParameteri(gl.TEXTURE_2D, gl.TEXTURE_MAG_FILTER, gl.LINEAR);
			gl.TexParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_S, gl.CLAMP);
			gl.TexParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_T, gl.CLAMP);
			gl.TexParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_R, gl.CLAMP);

			GlUtil.Assert("setting texture parameters");
		}

		private unsafe void CreateTexture()
		{
			if(!GlUtil.ContextValid) {
				LogManager.Log(LogLevel.Warning, "No opengl context active when creating textures");
			}
			uint[] handles = new uint[1];
			gl.GenTextures(1, handles);
			// 0 is used as special value to mark invalid textures here
			if(handles[0] == 0) {
				gl.GenTextures(1, handles);
			}
			this.handle = handles[0];
		}

		private static bool IsPowerOf2(uint val)
		{
			return (val & (val - 1)) == 0;
		}

		private unsafe uint SetupPixelFormat(IntPtr surfacep)
		{
			Sdl.Surface* surface = (Sdl.Surface*) surfacep;
			PixelFormat* format = (PixelFormat*) surface->format;
			uint glformat;
			if(format->BytesPerPixel == 3)
				glformat = gl.RGB;
			else if(format->BytesPerPixel == 4)
				glformat = gl.RGBA;
			else
				throw new Exception("Surface format not supported (only 24 and 32BPP modes supported at the moment");

			gl.PixelStorei(gl.UNPACK_ALIGNMENT, 1);
			gl.PixelStorei(gl.UNPACK_ROW_LENGTH,
			               surface->pitch / format->BytesPerPixel);

			return glformat;
		}
	}

}
