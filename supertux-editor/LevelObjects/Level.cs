//  $Id$
using System;
using System.IO;
using System.Collections.Generic;
using LispReader;

public delegate void TilesetChangedHandler(Level level);

[LispRoot("supertux-level")]
public sealed class Level
{
	[LispChild("version")]
	public int Version = 2;
	[PropertyProperties(Tooltip = "Name of this level.")]
	[LispChild("name", Translatable = true)]
	public string Name = "";
	[PropertyProperties(Tooltip = "Author of this level.")]
	[LispChild("author")]
	public string Author = "";

	private string tilesetFile = "images/tiles.strf";
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
			tilesetFile = value;
			Tileset = new Tileset(value);
			if(TilesetChanged != null)
				TilesetChanged(this);
		}
	}

	[LispChilds(Name = "sector", Type = typeof(Sector))]
	public List<Sector> Sectors = new List<Sector> ();
}
