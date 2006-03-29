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
        foreach(List List in TilesP.GetList("tile")) {
            try {
                Tile Tile = new Tile();
                ParseTile(Tile, List);
                while(Tiles.Count <= Tile.Id)
                    Tiles.Add(null);
                Tiles[Tile.Id] = Tile;
            } catch(Exception e) {
                Console.WriteLine("Couldn't parse a Tile: " + e.Message);
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

