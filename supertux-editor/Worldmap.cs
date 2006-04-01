using System;

[LispRoot("supertux-worldmap")]
public class Worldmap
{
	[LispChild("version")]
	public int Version = 2;
	[LispChild("name", Translatable = true)]
	public string Name = "";
	[LispChild("author")]
	public string Author = "";
	
	public Worldmap()
	{
	}
}


[LispRoot("supertux-level")]
public class Level {

	public Tileset Tileset = new Tileset("images/tiles.strf");
	
	[LispChilds(Name = "sector", Type = typeof(Sector))]
	public List<Sector> Sectors = new List<Sector> ();
}
