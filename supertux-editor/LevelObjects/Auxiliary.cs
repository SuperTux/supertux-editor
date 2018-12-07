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
using System;

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

	private Path path;
	[LispChild("path", Optional = true, Default = null)]
	public Path Path {
		get { return path; }
		set { path = value; }
	}

	private string pathRef = String.Empty;

	[LispChild("path-ref", Optional = true, Default="")]
	public string PathRef {
		get { return pathRef; }
		set { pathRef = value; }
	}

	public bool PathRemovable {
		get { return true; }
	}
}

[SupertuxObject("music", "images/engine/editor/music.png",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class MusicObject : IGameObject
{
	[LispChild("file", Optional = true, Default = "")]
	public string File = String.Empty;

	public MusicObject() {
		File = String.Empty;
	}

	public MusicObject(string file) {
		File = file;
	}
}

[SupertuxObject("ambient-light", "images/engine/editor/ambient_light.png",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class AmbientLightObject : IGameObject
{
	[ChooseColorSetting(UseAlpha = false)]
	[LispChild("color", Optional = false)]
	public Drawing.Color Color = new Drawing.Color(1f, 1f, 1f);

	public AmbientLightObject() {
		Color = new Drawing.Color(1f, 1f, 1f);
	}

	public AmbientLightObject(Drawing.Color color) {
		Color = color;
	}
}

[SupertuxObject("textscroller", "images/engine/editor/textscroller.png",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class TextScroller : IGameObject
{
	[LispChild("speed", Optional = true, Default = 20.0f)]
	public float Speed = 20.0f;

	[LispChild("file", Optional = true, Default = "")]
	public string File = String.Empty;
}

/* EOF */
