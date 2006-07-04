using LispReader;

[SupertuxObject("camera", "images/engine/editor/camera.png")]
public sealed class Camera : IGameObject, IPathObject {
	public enum Modes {
		normal,
		autoscroll,
		manual
	}

	// we should support enums here...
	/// <remarks>Can be: normal, autoscroll and manual</remarks>
	[LispChild("mode")]
	public Modes Mode = Modes.normal;
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
