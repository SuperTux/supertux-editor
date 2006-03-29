public class Camera : IGameObject {
	[LispChild("mode")]
	public string Mode = "normal";
	[LispChild("backscrolling")]
	public bool BackScrolling = true;
}
