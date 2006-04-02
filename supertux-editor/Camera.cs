using LispReader;

[SupertuxObject("camera", "images/engine/editor/camera.png")]
public class Camera : IGameObject {
	[LispChild("mode")]
	public string Mode = "normal";
	[LispChild("backscrolling", Optional = true, Default = true)]
	public bool BackScrolling = true;
}
