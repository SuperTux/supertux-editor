//  $Id$
using System;
using System.IO;
using System.Collections.Generic;
using LispReader;
using Lisp;

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

	private string tilesetFile = "images/tiles.strf";
	public Tileset Tileset = new Tileset("images/tiles.strf");
	public event TilesetChangedHandler TilesetChanged;

	public Level() {
		Tileset.CurrentTileset = Tileset;
	}

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
			Tileset.CurrentTileset = Tileset;
		}
	}

	[LispChilds(Name = "sector", Type = typeof(Sector))]
	public List<Sector> Sectors = new List<Sector> ();
}

