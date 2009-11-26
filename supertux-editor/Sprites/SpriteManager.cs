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

using System.Collections.Generic;
using System.IO;
using System;
using Lisp;
using Resources;
using DataStructures;
using Drawing;

namespace Sprites {

	public static class SpriteManager  {
		/// <summary>
		/// Stores already created SpriteDatas as a cache.
		/// </summary>
		private static Dictionary<string, SpriteData> SpriteDatas
			= new Dictionary<string, SpriteData>();

		/// <summary>
		/// Load a sprite from a sprite file.
		/// </summary>
		/// <remarks>
		/// If the sprite was already loaded before it will return
		/// a new sprite from the SpriteData in the cache.
		/// </remarks>
		/// <param name="SpriteFile">The file to load the sprite from.</param>
		/// <returns>A <see cref="Sprite"/>.</returns>
		public static Sprite Create(string SpriteFile) {
			if(!SpriteDatas.ContainsKey(SpriteFile)) {
				SpriteData Data = LoadSprite(SpriteFile);
				SpriteDatas[SpriteFile] = Data;
				return new Sprite(Data);
			}

			return new Sprite(SpriteDatas[SpriteFile]);
		}

		/// <summary>
		/// Creates a sprite from an image file.
		/// </summary>
		/// <remarks>
		/// If a sprite from the same image was already loaded
		/// before it will return a new sprite from the SpriteData
		/// in the cache.
		/// </remarks>
		/// <param name="ImageFile">The image file to create the sprite from</param>
		/// <param name="offset">Offset, same as <see cref="Sprite.Offset"/>.</param>
		/// <returns>A <see cref="Sprite"/>.</returns>
		public static Sprite CreateFromImage(string ImageFile, Vector offset) {
			if(!SpriteDatas.ContainsKey(ImageFile)) {
				Surface Surface = new Surface(ImageFile);
				SpriteData Data = new SpriteData(Surface, offset);
				SpriteDatas[ImageFile] = Data;
				return new Sprite(Data);
			}

			return new Sprite(SpriteDatas[ImageFile]);
		}

		public static Sprite CreateFromImage(string ImageFile) {
			return CreateFromImage(ImageFile, new Vector(0, 0));
		}

		/// <summary>
		/// Creates a <see cref="SpriteData"/> from a sprite filename.
		/// </summary>
		/// <param name="Filename">The file to load the sprite from.</param>
		/// <returns>A <see cref="SpriteData"/>.</returns>
		private static SpriteData LoadSprite(string Filename) {
			string BaseDir = ResourceManager.Instance.GetDirectoryName(Filename);
			List SpriteData = Util.Load(Filename, "supertux-sprite");

			return new SpriteData(SpriteData, BaseDir);
		}
	}

}

/* EOF */
