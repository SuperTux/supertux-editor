//  $Id$
using System;
using DataStructures;
using LispReader;
using Drawing;
using SceneGraph;

[SupertuxObject("background", "images/engine/editor/background.png")]
public sealed class Background : IGameObject, Node, IDrawableLayer {
	public string Name {
		get {
			return "BG Image";
		}
	}

	[LispChild("x", Optional = true, Default = 0f)]
	public float X;
	[LispChild("y", Optional = true, Default = 0f)]
	public float Y;

	[LispChild("image-top", Optional = true, Default = "")]
	[ChooseResourceSetting]
	public string ImageTop {
		get {
			return imageTop;
		}
		set {
			if (String.IsNullOrEmpty(value)) {
				surfaceTop = null;
			} else {
				surfaceTop = new Surface(value);
			}
			imageTop = value;
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
			if (String.IsNullOrEmpty(value)) {
				surface = null;
			} else {
				surface = new Surface(value);
			}
			image = value;
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
			if (String.IsNullOrEmpty(value)) {
				surfaceBottom = null;
			} else {
				surfaceBottom = new Surface(value);
			}
			imageBottom = value;
		}
	}
	private string imageBottom;
	private Surface surfaceBottom;

	[LispChild("speed")]
	public float Speed = 0.5f;

	private int layer = -200;
	[LispChild("layer", Optional = true, Default = -200)]
	public int Layer {
		get {
			return layer;
		}
		set {
			layer = value;
		}
	}

	public RectangleF Area {
		get {
			if(surface != null) {
				return new RectangleF(X, Y, surface.Width, surface.Height);
			} else {
				return new RectangleF(X, Y, 32, 32);
			}
		}
	}

	public bool Resizable {
		get {
			return false;
		}
	}

	public void ChangeArea(RectangleF NewArea) {
		X = NewArea.Left;
		Y = NewArea.Top;
	}

	public void Draw(Gdk.Rectangle cliprect)
	{
		if (surface == null) return;

		Surface sm = surface;
		Surface st = (surfaceTop != null)?(surfaceTop):(surface);
		Surface sb = (surfaceBottom != null)?(surfaceBottom):(surface);

		// TODO only draw tiles inside level and do draw all inside level

		for (int tileX = -10; tileX <= 10; tileX++) {
			for (int tileY = -10; tileY <= 0; tileY++) {
				if (cliprect.IntersectsWith(new Gdk.Rectangle((int) (X + st.Width * tileX),
				                                              (int) (Y - st.Height + st.Height * tileY),
				                                              (int) st.Width, (int) st.Height)))
					st.Draw(new Vector(X + st.Width * tileX, Y - st.Height + st.Height * tileY));
			}
			if (cliprect.IntersectsWith(new Gdk.Rectangle((int) (X + sm.Width * tileX),
			                                              (int) (Y),
			                                              (int) sm.Width, (int) sm.Height)))
				sm.Draw(new Vector(X + sm.Width * tileX, Y));
			for (int tileY = 0; tileY <= 10; tileY++) {
				if (cliprect.IntersectsWith(new Gdk.Rectangle((int) (X + sb.Width * tileX),
				                                              (int) (Y + surface.Height + sb.Height * tileY),
				                                              (int) sb.Width, (int) sb.Height)))
					sb.Draw(new Vector(X + sb.Width * tileX, Y + surface.Height + sb.Height * tileY));
			}
		}
	}

	public Node GetSceneGraphNode() {
		return this;
	}

}

[SupertuxObject("gradient", "images/engine/editor/gradient.png")]
public sealed class Gradient : IGameObject {
	[LispChild("layer", Optional = true, Default = -200)]
	public int Layer = -200;
	[ChooseColorSetting]
	[LispChild("top_color")]
	public Color TopColor;
	[ChooseColorSetting]
	[LispChild("bottom_color")]
	public Color BottomColor;
}
