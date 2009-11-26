//  SuperTux Editor
//  Copyright (C) 2006 Matthias Braun <matze@braunis.de>
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using LispReader;

[SupertuxObject("camera", "images/engine/editor/camera.png", Target = SupertuxObjectAttribute.Usage.None)]
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
	public bool PathRemovable {
		get { return true; }
	}
}

/* EOF */
