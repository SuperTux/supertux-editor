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
