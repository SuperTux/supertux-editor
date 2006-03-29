using System;
using System.Collections.Generic;
using System.IO;
using Lisp;
using Resources;

public class Tileset {
    private List<Tile> Tiles = new List<Tile>();
    private string BaseDir;
	public static bool LoadEditorImages;

	public const int TILE_WIDTH = 32;
	public const int TILE_HEIGHT = 32;

    public Tileset() {
    }

    public Tileset(string Resourcepath) {
        BaseDir = ResourceManager.Instance.GetDirectoryName(Resourcepath);
		List TilesL = Util.Load(Resourcepath, "supertux-tiles");

        Properties TilesP = new Properties(TilesL);
        foreach(List list in TilesP.GetList("tile")) {
            try {
                Tile Tile = new Tile();
                ParseTile(Tile, list);
                while(Tiles.Count <= Tile.Id)
                    Tiles.Add(null);
                Tiles[Tile.Id] = Tile;
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
    }

	public bool IsValid(uint Id) {
		return Tiles[(int) Id] != null;
	}

    public Tile Get(uint Id) {
        Tile Tile = Tiles[(int) Id];
		if(Tile == null)
			return null;

        Tile.LoadSurfaces(BaseDir, LoadEditorImages);
        
        return Tile;
    }

	public uint LastTileId {
		get {
			return (uint) Tiles.Count;
		}
	}

	private void ParseTiles(List list) {
		Properties props = new Properties(list);

		int width = 0;
		int height = 0;
		string image = "";
		props.Get("width", ref width);
		props.Get("height", ref height);
		props.Get("image", ref image);
		if(width == 0 || height == 0)
			throw new Exception("Width and Height of tiles block must be > 0");

		List<int> ids = new List<int> ();
		List<uint> attributes = new List<uint> ();
		props.GetIntList("ids", ids);
		props.GetUIntList("attributes", attributes);
		if(ids.Count != width * height)
			throw new Exception("Must have width*height ids in tiles block");
		if(attributes.Count != width * height)
			throw new Exception("Must have width*height attributes in tiles block");
		
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

                while(Tiles.Count <= tile.Id)
                    Tiles.Add(null);
                Tiles[tile.Id] = tile;

				id++;
			}
		}
	}
	

    private void ParseTile(Tile Tile, List Data) {
        Properties Props = new Properties(Data);
        
        if(!Props.Get("id", ref Tile.Id))
            throw new Exception("Tile has no ID");

		List Images = null;
		Props.Get("images", ref Images);
		if(Images != null)
			Tile.Images = ParseTileImages(Images);
		
		List EditorImages = null;
		Props.Get("editor-images", ref EditorImages);
		if(EditorImages != null) {
			Tile.EditorImages = ParseTileImages(EditorImages);
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

	private List<Tile.ImageResource> ParseTileImages(List list) {
		List<Tile.ImageResource> Result = new List<Tile.ImageResource>();

		for(int i = 1; i < list.Length; ++i) {
			if(list[i] is string) {
				Tile.ImageResource resource = new Tile.ImageResource();
				resource.Filename = (string) list[i];
				Result.Add(resource);
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
				Result.Add(resource);
			}
		}

		return Result;
	}
}

