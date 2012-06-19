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
using DataStructures;

namespace Drawing
{

	/// <summary>
	/// Surface class. This class basically holds a reference to an opengl texture
	/// along with texture coordinates that specify a rectangle on that texture.
	/// Several surface may share a single texture (but can still have different
	/// texture coordinates)
	/// </summary>
	public sealed class Surface : IDisposable, ICloneable
	{
		private float width;
		/// <summary>get surface width in pixels</summary>
		public float Width
		{
			get
			{
				return width;
			}
		}

		private float height;
		/// <summary>get surface height in pixels</summary>
		public float Height
		{
			get
			{
				return height;
			}
		}

		private ImageTexture texture;
		/// <summary>Get OpenGL Texture</summary>
		public Texture Texture
		{
			get
			{
				return texture;
			}
		}

		public float Left;
		public float Right;
		public float Top;
		public float Bottom;

		/// <summary>Construct a new Surface from the given image resource</summary>
		public Surface(string Resourcepath)
		{
			texture = TextureManager.Get(Resourcepath);
			texture.Ref();
			width = texture.ImageWidth;
			height = texture.ImageHeight;

			Left = 0;
			Top = 0;
			Right = texture.UVRight;
			Bottom = texture.UVBottom;
		}

		public Surface(string Resourcepath, float x, float y, float w, float h)
		{
			texture = TextureManager.Get(Resourcepath);
			texture.Ref();

			width = w;
			height = h;

			Left = x / texture.Width;
			Top = y / texture.Height;
			Right = (x + w) / texture.Width;
			Bottom = (y + h) / texture.Height;
		}

		public Surface(Surface other)
		{
			texture = other.texture;
			texture.Ref();

			width = other.width;
			height = other.height;

			Left = other.Left;
			Top = other.Top;
			Right = other.Right;
			Bottom = other.Bottom;
		}

		public object Clone()
		{
			return new Surface(this);
		}

		public void Dispose()
		{
			texture.UnRef();
			texture = null;
		}

		public void Draw(Vector pos)
		{
			gl.BindTexture(gl.TEXTURE_2D, texture.Handle);

			gl.Begin(gl.QUADS);
			gl.TexCoord2f(Left, Top);
			gl.Vertex2f(pos.X, pos.Y);
			gl.TexCoord2f(Right, Top);
			gl.Vertex2f(pos.X + Width, pos.Y);
			gl.TexCoord2f(Right, Bottom);
			gl.Vertex2f(pos.X + Width, pos.Y + Height);
			gl.TexCoord2f(Left, Bottom);
			gl.Vertex2f(pos.X, pos.Y + Height);
			gl.End();
		}
	}

}

/* EOF */
