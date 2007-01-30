//  $Id$
using System;
using OpenGl;
using Sdl;
using Sdl.Image;
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

			Left = 0.5f / width;
			Top = 0.5f / height;
			Right = texture.UVRight;
			Bottom = texture.UVBottom;
		}

		public Surface(string Resourcepath, float x, float y, float w, float h)
		{
			texture = TextureManager.Get(Resourcepath);
			texture.Ref();

			width = w;
			height = h;

			Left = (x+0.5f) / texture.Width;
			Top = (y+0.5f) / texture.Height;
			Right = (x + w - 0.5f) / texture.Width;
			Bottom = (y + h - 0.5f) / texture.Height;
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
