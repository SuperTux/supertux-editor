using System;
using System.IO;
using System.Collections.Generic;
using LispReader;

[LispRoot("supertux-level")]
public class Level
{
	[LispChild("version")]
	public int Version = 2;
	[LispChild("name", Translatable = true)]
	public string Name = "";
	[LispChild("author")]
	public string Author = "";
	
	private string tilesetFile = "images/tiles.strf";
	public Tileset Tileset = new Tileset("images/tiles.strf");	
	
	[LispChild("tileset", Optional = true, Default = "images/tiles.strf")]
	[ChooseResourceSetting]
	public string TilesetFile {
		get {
			return tilesetFile;
		}
		set {
			if(value == null && value == "")
				return;
			tilesetFile = value;
			Tileset = new Tileset(value);
		}
	}
	
	[LispChilds(Name = "sector", Type = typeof(Sector))]
	public List<Sector> Sectors = new List<Sector> ();
}
