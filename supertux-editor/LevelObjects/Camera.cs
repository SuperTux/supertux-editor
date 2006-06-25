using LispReader;

[SupertuxObject("camera", "images/engine/editor/camera.png")]
public class Camera : IGameObject, IPathObject {
	// we should support enums here...
	/// Can be: normal, autoscroll and manual
	[LispChild("mode")]
	public string Mode = "normal";
	[LispChild("backscrolling", Optional = true, Default = true)]
	public bool BackScrolling = true;

	private Path path;
	[LispChild("path", Optional = true, Default = null)]
	public Path Path {
		get {
			return path;
		}
		set {
			path = value;
		}
	}
}
