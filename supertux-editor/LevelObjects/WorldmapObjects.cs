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

using System;
using Sprites;
using DataStructures;
using LispReader;

public abstract class WorldmapObject : SimpleObject
{
	public WorldmapObject()
	{
	}

	public override RectangleF Area {
		get {
			return new RectangleF(X*32 + 16 - Sprite.Offset.X,
			                      Y*32 + 16 - Sprite.Offset.Y,
			                      Sprite.Width,
			                      Sprite.Height);
		}
	}

	public override void ChangeArea(RectangleF NewArea) {
		X = ((int) (NewArea.Left - 16 + Sprite.Offset.X)) / 32;
		Y = ((int) (NewArea.Top - 16 + Sprite.Offset.Y)) / 32;
	}

	public override void Draw(Gdk.Rectangle cliprect) {
		if (!cliprect.IntersectsWith((Gdk.Rectangle) Area))
			return;
		if(Sprite == null)
			return;

		Sprite.Draw(new Vector(X*32 + 16, Y*32 + 16));
	}
}

[SupertuxObject("worldmap-spawnpoint", "images/worldmap/common/tux.png", Target = SupertuxObjectAttribute.Usage.WorldmapOnly)]
public sealed class WorldmapSpawnpoint : WorldmapObject
{
	[LispChild("name")]
	public string Name;
	[LispChild("auto-dir", Optional = true, Default = "none")]
	public string AutoDir = "none";

	public WorldmapSpawnpoint()
	{
		Sprite = SpriteManager.CreateFromImage("images/worldmap/common/tux.png", new Vector(16, 16));
	}
}

[SupertuxObject("level", "images/worldmap/common/leveldot.sprite", Target = SupertuxObjectAttribute.Usage.WorldmapOnly)]
public sealed class WorldmapLevel : WorldmapObject
{
	[LispChild("name")]
	public string Name;
	[LispChild("extro-filename", Optional = true, Default = "")]
	public string ExtroFilename = String.Empty;
	[LispChild("quit-worldmap", Optional = true, Default = false)]
	public bool QuitWorldmap = false;
	[LispChild("auto-play", Optional = true, Default = false)]
	public bool AutoPlay = false;
	[ChooseResourceSetting]
	[LispChild("sprite", Optional = true, Default = "")]
	public string SpriteFile {
		get {
			return spriteFile;
		}
		set {
			if (!String.IsNullOrEmpty(value)) {
				Sprite newSprite = SpriteManager.Create(value);
				newSprite.Action = "solved";
				Sprite = newSprite;	//save new sprite after (no exception only)
			}
			spriteFile = value;
		}
	}
	private string spriteFile = String.Empty;

	public WorldmapLevel()
	{
		Sprite = SpriteManager.Create("images/worldmap/common/leveldot.sprite");
		Sprite.Action = "solved";
	}
}

[SupertuxObject("special-tile", "images/worldmap/common/teleporterdot.sprite", Target = SupertuxObjectAttribute.Usage.WorldmapOnly)]
public sealed class SpecialTile : WorldmapObject
{
	[LispChild("teleport-to-x", Optional = true, Default = -1f)]
	public float TeleportToX = -1f;
	[LispChild("teleport-to-y", Optional = true, Default = -1f)]
	public float TeleportToY = -1f;
	[LispChild("map-message", Optional = true, Default = "", Translatable = true)]
	public string Message = String.Empty;
	[LispChild("invisible-tile", Optional = true, Default = false)]
	public bool invisible;
	[LispChild("passive-message", Optional = true, Default = false)]
	public bool PassiveMessage;
	[LispChild("script", Optional = true, Default = "")]
	[EditScriptSetting]
	public string Script = String.Empty;
	[LispChild("apply-to-direction", Optional = true, Default = "")]
	public string ApplyToDirection = String.Empty;
	[ChooseResourceSetting]
	[LispChild("sprite", Optional = true, Default = "")]
	public string SpriteFile {
		get {
			return spriteFile;
		}
		set {
			if (!String.IsNullOrEmpty(value)) {
				Sprite = SpriteManager.Create(value);
			} else {
				Sprite = SpriteManager.Create("images/worldmap/common/teleporterdot.sprite");
			}
			spriteFile = value;
		}
	}
	private string spriteFile = String.Empty;

	public SpecialTile()
	{
		Sprite = SpriteManager.Create("images/worldmap/common/teleporterdot.sprite");
	}
}

[SupertuxObject("sprite-change", "images/engine/editor/spritechange.png", Target = SupertuxObjectAttribute.Usage.WorldmapOnly)]
public sealed class SpriteChange : WorldmapObject
{
	[ChooseResourceSetting]
	[LispChild("sprite")]
	public string SpriteFile {
		get {
			return spriteFile;
		}
		set {
			if (!String.IsNullOrEmpty(value)) {
				Sprite = SpriteManager.Create(value);
			} else {
				Sprite = SpriteManager.Create("images/worldmap/common/tux.sprite");
			}
			spriteFile = value;
		}
	}
	private string spriteFile = String.Empty;
	[LispChild("change-on-touch")]
	public bool ChangeOnTouch;
	[LispChild("stay-action")]
	public string StayAction;
	[LispChild("initial-stay-action")]
	public bool InitialStayAction = false;
	[LispChild("stay-group", Optional = true, Default = "")]
	public string StayGroup;

	public SpriteChange() {
		Sprite = SpriteManager.CreateFromImage("images/engine/editor/spritechange.png", new Vector(16, 16));
	}
}

[SupertuxObject("teleporter", "images/worldmap/common/teleporterdot.sprite", Target = SupertuxObjectAttribute.Usage.WorldmapOnly)]
public sealed class Teleporter : WorldmapObject
{
	[ChooseResourceSetting]
	[LispChild("sprite", Optional = true, Default = "")]
	public string SpriteFile {
		get {
			return spriteFile;
		}
		set {
			if (!String.IsNullOrEmpty(value)) {
				Sprite = SpriteManager.Create(value);
			} else {
				Sprite = SpriteManager.Create("images/worldmap/common/teleporterdot.sprite");
			}
			spriteFile = value;
		}
	}
	private string spriteFile = String.Empty;

	[LispChild("worldmap", Optional = true, Default = "")]
	public string Worldmap = String.Empty;

	[LispChild("spawnpoint", Optional = true, Default = "")]
	public string Spawnpoint = String.Empty;

	[LispChild("automatic", Optional = true, Default = false)]
	public bool Automatic;

	[LispChild("message", Optional = true, Default = "", Translatable = true)]
	public string Message = String.Empty;

	public Teleporter()
	{
		Sprite = SpriteManager.Create("images/worldmap/common/teleporterdot.sprite");
	}
}

/* EOF */
