public class Background : IGameObject {
	[LispChild("image")]
	[ChooseResourceSetting]	
	public string Image;
	[LispChild("speed")]
	public float Speed = 0.5f;
}
