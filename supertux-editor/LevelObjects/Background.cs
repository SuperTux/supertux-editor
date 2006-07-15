using DataStructures;
using LispReader;
using Drawing;
using SceneGraph;

[SupertuxObject("background", "images/engine/editor/background.png")]
public sealed class Background : IGameObject, Node {
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
			imageTop = value;
			if (imageTop == "") {
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
			if (image == "") {
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
			if (imageBottom == "") {
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

	public void Draw()
	{
		if (surface == null) return;

		Surface sm = surface;
		Surface st = (surfaceTop != null)?(surfaceTop):(surface);
		Surface sb = (surfaceBottom != null)?(surfaceBottom):(surface);

		// TODO only draw visible tiles

		for (int tileX = -10; tileX <= 10; tileX++) {
			for (int tileY = -10; tileY <= 0; tileY++) {
				st.Draw(new Vector(X + st.Width * tileX, Y - st.Height + st.Height * tileY));
			}
			sm.Draw(new Vector(X + sm.Width * tileX, Y));
			for (int tileY = 0; tileY <= 10; tileY++) {
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
