using System;
using System.Collections.Generic;
using System.IO;
using Lisp;
using LispReader;
using Resources;

public sealed class Tileset {
	private List<Tile> tiles = new List<Tile>();
	private Dictionary<string, Tilegroup> tilegroups = new Dictionary<string, Tilegroup>();
	private string baseDir;
	public static bool LoadEditorImages;

	public IDictionary<string, Tilegroup> Tilegroups {
		get {
			return tilegroups;
		}
	}

	public const int TILE_WIDTH = 32;
	public const int TILE_HEIGHT = 32;

	public Tileset() {
	}

	/// <summary>Loads the tiles from <paramref name="Resourcepath"/>.</summary>
	/// <param name="Resourcepath">A path relative to the data folder of SuperTux.</param>
	public Tileset(string Resourcepath) {
		baseDir = ResourceManager.Instance.GetDirectoryName(Resourcepath);
		List TilesL = Util.Load(Resourcepath, "supertux-tiles");

		Properties TilesP = new Properties(TilesL);
		foreach(List list in TilesP.GetList("tile")) {
			try {
				Tile tile = new Tile();
				ParseTile(tile, list);
				while(tiles.Count <= tile.Id)
					tiles.Add(null);
				tiles[tile.Id] = tile;
			} catch(Exception e) {
				Console.WriteLine("Couldn't parse a Tile: " + e.Message);
				Console.WriteLine(e.StackTrace);
			}
		}

		foreach(List list in TilesP.GetList("tiles")) {
			try {
				ParseTiles(list);
			} catch(Exception e) {
				Console.WriteLine("Couldn't parse a tiles: " + e.Message);
				Console.WriteLine(e.StackTrace);
			}
		}

		// construct a tilegroup with all tiles
		Tilegroup allGroup = new Tilegroup();
			allGroup.Name = "All";
			foreach(Tile tile in tiles) {
				if(tile != null)
					allGroup.Tiles.Add(tile.Id);
			}
			tilegroups.Add(allGroup.Name, allGroup);

		LispSerializer serializer = new LispSerializer(typeof(Tilegroup));
		foreach(List list in TilesP.GetList("tilegroup")) {
			try {
				Tilegroup group = (Tilegroup) serializer.Read(list);
				for(int i = 0; i < group.Tiles.Count; ) {
					if(!IsValid(group.Tiles[i])) {
						Console.WriteLine("Tilegroup " + group.Name + " contains invalid TileID " + group.Tiles[i]);
						group.Tiles.RemoveAt(i);
						continue;
					}
					++i;
				}
				tilegroups.Add(group.Name, group);
			} catch(Exception e) {
				Console.WriteLine("Couldn't parse tilegroup: " + e.Message);
				Console.WriteLine(e.StackTrace);
			}
		}
	}

	/// <summary>Checks if <paramref name="id"/> is a valid tile id</summary>
	/// <returns>Returns true if tile id is valid, otherwise false.</returns>
	public bool IsValid(int id) {
		return tiles[id] != null;
	}

	public Tile Get(int id) {
		Tile tile = tiles[id];
		if(tile == null)
			return null;

		tile.LoadSurfaces(baseDir, LoadEditorImages);

		return tile;
	}

	/// <summary>Id of the last existing tile.</summary>
	public int LastTileId {
		get {
			return tiles.Count;
		}
	}

	/// <summary>Parse a list of tiles (from a tiles block in the tileset file)</summary>
	/// <exception cref="System.Exception">
	/// Thrown when width and/or height of ids/attributes of a tiles block is wrong.
	/// </exception>
	private void ParseTiles(List list) {
		Properties props = new Properties(list);

		int width = 0;
		int height = 0;
		string image = "";
		props.Get("width", ref width);
		props.Get("height", ref height);
		props.Get("image", ref image);
		if(width == 0 || height == 0)
			throw new ApplicationException("Width and Height of tiles block must be > 0");

		List<int> ids = new List<int> ();
		List<uint> attributes = new List<uint> ();
		props.GetIntList("ids", ids);
		props.GetUIntList("attributes", attributes);
		if(ids.Count != width * height)
			throw new ApplicationException("Must have width*height ids in tiles block");
		if(attributes.Count != width * height)
			throw new ApplicationException("Must have width*height attributes in tiles block");

		int id = 0;
		for(int y = 0; y < height; ++y) {
			for(int x = 0; x < width; ++x) {
				Tile tile = new Tile();

				Tile.ImageResource res = new Tile.ImageResource();
				res.Filename = image;
				res.x = x * TILE_WIDTH;
				res.y = y * TILE_HEIGHT;
				res.w = TILE_WIDTH;
				res.h = TILE_HEIGHT;
				tile.Images = new List<Tile.ImageResource>();
				tile.Images.Add(res);
				tile.Id = ids[id];
				tile.Attributes = (Tile.Attribute) attributes[id];

				while(tiles.Count <= tile.Id)
					tiles.Add(null);
				tiles[tile.Id] = tile;

				id++;
			}
		}
	}

	/// <summary>Parse a tile (from a tile block in the tileset file)</summary>
	/// <exception cref="System.Exception">Thrown when a tile has no ID</exception>
	private static void ParseTile(Tile Tile, List Data) {
		Properties Props = new Properties(Data);

		if(!Props.Get("id", ref Tile.Id))
			throw new ApplicationException("Tile has no ID");

		List images = null;
		Props.Get("images", ref images);
		if(images != null)
			Tile.Images = ParseTileImages(images);

		List editorImages = null;
		Props.Get("editor-images", ref editorImages);
		if(editorImages != null) {
			Tile.EditorImages = ParseTileImages(editorImages);
		}

		bool val = false;
		if(Props.Get("solid", ref val) && val)
			Tile.Attributes |= Tile.Attribute.SOLID;
		if(Props.Get("unisolid", ref val) && val)
			Tile.Attributes |= Tile.Attribute.UNISOLID | Tile.Attribute.SOLID;
		if(Props.Get("brick", ref val) && val)
			Tile.Attributes |= Tile.Attribute.BRICK;
		if(Props.Get("ice", ref val) && val)
			Tile.Attributes |= Tile.Attribute.ICE;
		if(Props.Get("water", ref val) && val)
			Tile.Attributes |= Tile.Attribute.WATER;
		if(Props.Get("spike", ref val) && val)
			Tile.Attributes |= Tile.Attribute.SPIKE;
		if(Props.Get("fullbox", ref val) && val)
			Tile.Attributes |= Tile.Attribute.FULLBOX;
		if(Props.Get("coin", ref val) && val)
			Tile.Attributes |= Tile.Attribute.COIN;
		if(Props.Get("goal", ref val) && val)
			Tile.Attributes |= Tile.Attribute.GOAL;

		if(Props.Get("north", ref val) && val)
			Tile.Attributes |= Tile.Attribute.WORLDMAP_NORTH;
		if(Props.Get("south", ref val) && val)
			Tile.Attributes |= Tile.Attribute.WORLDMAP_SOUTH;
		if(Props.Get("west", ref val) && val)
			Tile.Attributes |= Tile.Attribute.WORLDMAP_WEST;
		if(Props.Get("east", ref val) && val)
			Tile.Attributes |= Tile.Attribute.WORLDMAP_EAST;
		if(Props.Get("stop", ref val) && val)
			Tile.Attributes |= Tile.Attribute.WORLDMAP_STOP;

		Props.Get("data", ref Tile.Data);
		Props.Get("anim-fps", ref Tile.AnimFps);
		if(Props.Get("slope-type", ref Tile.Data))
			Tile.Attributes |= Tile.Attribute.SLOPE | Tile.Attribute.SOLID;
	}

	private static List<Tile.ImageResource> ParseTileImages(List list) {
		List<Tile.ImageResource> result = new List<Tile.ImageResource>();

		for(int i = 1; i < list.Length; ++i) {
			if(list[i] is string) {
				Tile.ImageResource resource = new Tile.ImageResource();
				resource.Filename = (string) list[i];
				result.Add(resource);
			} else {
				if(!(list[i] is List)) {
					Console.WriteLine("Unexpected data in images part: " + list[i]);
					continue;
				}
				List region = (List) list[i];
				if(!(region[0] is Symbol)) {
					Console.WriteLine("Expected symbol in sublist of images");
					continue;
				}
				Symbol symbol = (Symbol) region[0];
				if(symbol.Name != "region") {
					Console.WriteLine("Non-supported image type '" + symbol.Name + "'");
					continue;
				}
				if(region.Length != 6) {
					Console.WriteLine("region list has to contain 6 elemetns");
					continue;
				}

				Tile.ImageResource resource = new Tile.ImageResource();
				resource.Filename = (string) region[1];
				resource.x = (int) region[2];
				resource.y = (int) region[3];
				resource.w = (int) region[4];
				resource.h = (int) region[5];
				result.Add(resource);
			}
		}

		return result;
	}
}
