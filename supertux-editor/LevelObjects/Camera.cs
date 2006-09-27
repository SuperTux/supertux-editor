//  $Id$
using LispReader;

[SupertuxObject("camera", "images/engine/editor/camera.png")]
public sealed class Camera : IGameObject, IPathObject {

	/// <summary>
	/// Modes for <see cref="Camera"/>.
	/// </summary>
	public enum Modes {
		/// <summary>
		/// Normal scrolling of camera.
		/// </summary>
		normal,
		/// <summary>
		/// Follow the path <see cref="Path"/>.
		/// </summary>
		autoscroll,
		/// <summary>
		/// Manually control the camera by scripting.
		/// </summary>
		manual
	}

	[LispChild("mode")]
	public Modes Mode = Modes.normal;

	[PropertyProperties(Tooltip = "Defines if camera can scroll backwards")]
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
