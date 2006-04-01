using System;
using System.Collections.Generic;
using Sdl;
using Sdl.Image;
using Resources;

namespace Drawing
{

public class TextureManager {
	private static Dictionary<string, ImageTexture> ImageTextures
		= new Dictionary<string, ImageTexture>();

	public static ImageTexture Get(string Resourcepath) {
		ImageTexture Result;
		if(ImageTextures.ContainsKey(Resourcepath)) {
			Result = ImageTextures[Resourcepath];
		} else {
			Result = CreateImageTexture(Resourcepath);
			ImageTextures.Add(Resourcepath, Result);
		}

		return Result;
	}

	private static ImageTexture CreateImageTexture(string Resourcepath) {
		IntPtr image = IMG.Load(
				ResourceManager.Instance.GetFileName(Resourcepath));
		if(image == IntPtr.Zero) {
			throw new Exception("Couldn't load image '" + Resourcepath
					+ "' : " + SDL.GetError());
		}
		try {
			return new ImageTexture(image);
		} finally {
			SDL.FreeSurface(image);
		}
	}
}

}
