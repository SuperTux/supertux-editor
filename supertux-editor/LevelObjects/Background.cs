//  $Id$
using System;
using DataStructures;
using LispReader;
using Drawing;

[SupertuxObject("background", "images/engine/editor/background.png")]
public sealed class Background : VirtualObject {
	[LispChild("image-top", Optional = true, Default = "")]
	[ChooseResourceSetting]
	public string ImageTop {
		get {
			return imageTop;
		}
		set {
			imageTop = value;
			if (String.IsNullOrEmpty(imageTop)) {
				surfaceTop = null;
				return;
			}
			surfaceTop = new Surface(imageTop);
		}
	}
	private string imageTop;
	private Surface surfaceTop;

	[LispChild("image")]
	[ChooseResourceSetting]
	public string Image {
		get {
			return image;
		}
		set {
			image = value;
			if (String.IsNullOrEmpty(image)) {
				surface = null;
				return;
			}
			surface = new Surface(image);
		}
	}
	private string image;
	private Surface surface;

	[LispChild("image-bottom", Optional = true, Default = "")]
	[ChooseResourceSetting]
	public string ImageBottom {
		get {
			return imageBottom;
		}
		set {
			imageBottom = value;
			if (String.IsNullOrEmpty(imageBottom)) {
				surfaceBottom = null;
				return;
			}
			surfaceBottom = new Surface(imageBottom);
		}
	}
	private string imageBottom;
	private Surface surfaceBottom;

	[LispChild("speed")]
	public float Speed = 0.5f;
	[LispChild("layer", Optional = true, Default = -200)]
	public int Layer = -200;

	public override void Draw(DrawingContext context)
	{
		base.Draw(context);

		if (surface == null)
			return;

		float left = 0;
		float top = 0;
		float right = 10000; // TODO find the actual value for this
		float levelBottom = 3000;
		float bottom = levelBottom; // TODO

		/* TODO 
		if(cliprect.Left > left)
			left = cliprect.Left;
		if(cliprect.Right < right)
			right = cliprect.Right;
		if(cliprect.Top > top)
			top = cliprect.Top;
		if(cliprect.Bottom < bottom)
			bottom = cliprect.Bottom;
		*/

		float normalsurface_start = (surfaceTop != null) ? (surfaceTop.Height) : 0;
		float bottomsurface_start = (surfaceBottom != null) ? levelBottom - (surfaceBottom.Height) : bottom;

		Surface surf = surfaceTop;
		if(top >= bottomsurface_start) {
			surf = surfaceBottom;
		} else if(top >= normalsurface_start) {
			surf = surface;
		}
		top -= (top % surf.Height);
		left -= (left % surf.Width);
		for(float y = top; y < bottom; y += surf.Height) {
			surf = surfaceTop;
			if(y >= bottomsurface_start) {
				surf = surfaceBottom;
			} else if(y >= normalsurface_start) {
				surf = surface;
			}

			for(float x = left; x < right; x += surf.Width) {
				context.DrawSurface(surf, new Vector(x, y), Layer);
			}
		}
	}
}

[SupertuxObject("gradient", "images/engine/editor/gradient.png")]
public sealed class Gradient : VirtualObject {
	[LispChild("layer", Optional = true, Default = -200)]
	public int Layer = -200;
	[ChooseColorSetting]
	[LispChild("top_color")]
	public Color TopColor;
	[ChooseColorSetting]
	[LispChild("bottom_color")]
	public Color BottomColor;

	/* TODO: draw gradient */
}
