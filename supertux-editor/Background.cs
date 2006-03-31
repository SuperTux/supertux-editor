using LispReader;
using Drawing;

[SupertuxObject("background", "images/engine/editor/background.png")]
public class Background : IGameObject {
	[LispChild("image")]
	[ChooseResourceSetting]	
	public string Image;
	[LispChild("speed")]
	public float Speed = 0.5f;
	[LispChild("layer")]
	public int Layer = -200;
}

[SupertuxObject("gradient", "images/engine/editor/gradient.png")]
public class Gradient : IGameObject {
	[LispChild("layer")]
	public int Layer = -200;
	[ChooseColorSetting]
	[LispChild("top_color")]
	public Color TopColor;
	[ChooseColorSetting]
	[LispChild("bottom_color")]
	public Color BottomColor;
}
