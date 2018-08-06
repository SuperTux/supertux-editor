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
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using LispReader;

public delegate void TilesetChangedHandler(Level level);

[LispRoot("supertux-level")]
public sealed class Level
{
	[PropertyProperties(Hidden = true)]
	[LispChild("version")]
	public int Version = 2;
	[PropertyProperties(Tooltip = "Name of this level.")]
	[LispChild("name", Translatable = true)]
	public string Name = String.Empty;
	[PropertyProperties(Tooltip = "Author of this level, e.g. \"John Doe\"")]
	[LispChild("author")]
	public string Author = String.Empty;
	[PropertyProperties(Tooltip = "How to contact the author, e.g. \"mailto:john.doe@example.com\"")]
	[LispChild("contact", Optional = true, Default = "")]
	public string Contact = "";
	[PropertyProperties(Tooltip = "Whether to allow the community to share the level, improve it, include it in bonus packs, ...")]
	[LispChild("license", Optional = true, Default = ""), ChooseLicenseSetting()]
	public string License = "";
  [PropertyProperties(Tooltip = "Set an optional time, in seconds, that the player can try to beat the level in")]
	[LispChild("target-time", Optional = true, Default = 0.0f)]
	public float TargetTime;

	private string tilesetFile = "images/tiles.strf";
	public bool isWorldmap {
		get {
		  //Tileset files containing "world" in their name are considered worldmaps
			return (Regex.IsMatch(tilesetFile, @"world", RegexOptions.IgnoreCase));
		}
	}
	public Tileset Tileset = new Tileset("images/tiles.strf");
	public event TilesetChangedHandler TilesetChanged;

	[PropertyProperties(Tooltip = "Tileset used for level.\nDo not change this unless you know what you are doing.")]
	[LispChild("tileset", Optional = true, Default = "images/tiles.strf")]
	[ChooseResourceSetting]
	public string TilesetFile {
		get {
			return tilesetFile;
		}
		set {
			if(String.IsNullOrEmpty(value))
				return;
			Tileset = new Tileset(value);
			if(TilesetChanged != null)
				TilesetChanged(this);
			tilesetFile = value;
		}
	}

	[LispChilds(Name = "sector", Type = typeof(Sector))]
	public List<Sector> Sectors = new List<Sector> ();
}

/* EOF */
