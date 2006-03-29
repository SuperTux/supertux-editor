using System;
using System.IO;
using System.Collections.Generic;

[LispRoot(Name = "supertux-level")]
public class Level {
	[LispChild("version")]
	public int Version = 2;
	[LispChild("name", Translatable = true)]
	public string Name = "";
	[LispChild("author")]
	public string Author = "";

	public Tileset Tileset = new Tileset("images/tiles.strf");
	
	[LispChilds(Name = "sector", Type = typeof(Sector))]
	public List<Sector> Sectors = new List<Sector> ();
}

