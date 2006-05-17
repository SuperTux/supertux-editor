using LispReader;
using Drawing;

[SupertuxObject("background", "images/engine/editor/background.png")]
public class Background : IGameObject {
	[LispChild("x", Optional = true, Default = 0)]
	public float X;
	[LispChild("y", Optional = true, Default = 0)]
	public float Y;
	[LispChild("image-top", Optional = true, Default = null)]
	[ChooseResourceSetting]	
	public string ImageTop;
	[LispChild("image")]
	[ChooseResourceSetting]	
	public string Image;
	[LispChild("image-bottom", Optional = true, Default = null)]
	[ChooseResourceSetting]	
	public string ImageBottom;
	[LispChild("speed")]
	public float Speed = 0.5f;
	[LispChild("layer", Optional = true, Default = -200)]
	public int Layer = -200;
}

[SupertuxObject("gradient", "images/engine/editor/gradient.png")]
public class Gradient : IGameObject {
	[LispChild("layer", Optional = true, Default = -200)]
	public int Layer = -200;
	[ChooseColorSetting]
	[LispChild("top_color")]
	public Color TopColor;
	[ChooseColorSetting]
	[LispChild("bottom_color")]
	public Color BottomColor;
}
