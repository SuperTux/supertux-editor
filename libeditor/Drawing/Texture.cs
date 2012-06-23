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

	public class Texture : IDisposable
	{
		private uint handle;
		public uint Handle
		{
			get
			{
				if (handle == 0) {
					Create();
				} 
				return handle;
			}
		}

		protected uint width;
		public uint Width
		{
			get
			{
				return width;
			}
		}

		protected uint height;
		public uint Height
		{
			get
			{
				return height;
			}
		}
		
		//cached value for use in Create()
		private uint glformat;

		protected Texture()
		{
			handle = 0;
		}
		
		public Texture(uint width, uint height, uint glformat)
		{
			this.width = width;
			this.height = height;
			this.glformat = glformat;
		}
		
		protected virtual void Create()
		{
			LogManager.Log(LogLevel.Debug, "Texture.Create()");
			// Not needed on newer OpenGL
			if (!glHelper.HasExtension("GL_ARB_texture_non_power_of_two"))
			{
				if(!IsPowerOf2(width) || !IsPowerOf2(height))
					throw new Exception("Texture size must be power of 2");
			}

			GlUtil.Assert("before creating texture");
			CreateTexture();

			try {
				gl.BindTexture(gl.TEXTURE_2D, Handle);
				gl.TexImage2D(gl.TEXTURE_2D, 0, (int) glformat,
				              (int) width, (int) height, 0,
				              gl.RGBA, gl.UNSIGNED_BYTE, IntPtr.Zero);
				GlUtil.Assert("creating texture (too big?)");
				SetTextureParams();
			} catch(Exception) {
				uint[] handles = { Handle };
				gl.DeleteTextures(1, handles);
				throw;
			}
		}

		public unsafe void Dispose()
		{
			if(handle == 0)
				return;

			uint[] handles = { Handle };

			gl.DeleteTextures(1, handles);
			this.handle = 0;
		}
		
		/// <summary>
		/// Helper method: set common texture parameters.
		/// </summary>
		protected static void SetTextureParams()
		{
			gl.TexParameteri(gl.TEXTURE_2D, gl.TEXTURE_MIN_FILTER, gl.LINEAR);
			gl.TexParameteri(gl.TEXTURE_2D, gl.TEXTURE_MAG_FILTER, gl.LINEAR);
			gl.TexParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_S, gl.CLAMP);
			gl.TexParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_T, gl.CLAMP);
			gl.TexParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_R, gl.CLAMP);

			GlUtil.Assert("setting texture parameters");
		}
		
		/// <summary>
		/// Helper method: creates a texture using glGenTexture and saves the handle.
		/// </summary>
		protected unsafe void CreateTexture()
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

		protected static bool IsPowerOf2(uint val)
		{
			return (val & (val - 1)) == 0;
		}
	}

}

/* EOF */
