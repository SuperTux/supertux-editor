using Lisp;
using DataStructures;
using System;
using System.IO;
using System.Collections.Generic;

public class WorldmapSector : SectorBase {
    public Tilemap Solids;
    public WorldmapTux Player;
    private Tileset Tileset = new Tileset("tilesets/worldmap/worldmap.strf");
    private Dictionary<string, WorldmapSpawnPoint> SpawnPoints 
        = new Dictionary<string, WorldmapSpawnPoint>();
    private Field<List<WorldmapObject> > ObjectGrid;
    
    public WorldmapSector(string Filename) {
        List WorldMapL = Util.Load(Filename, "supertux-worldmap");
        
        LispIterator iter = new LispIterator(WorldMapL);
        while(iter.MoveNext()) {
            switch(iter.Key) {
                case "properties":
                    Properties Props = new Properties(iter.List);
                    Props.Get("name", ref Name);
                    Props.Get("music", ref Music);
                    Console.WriteLine("Name: " + Name);
                    Console.WriteLine("Music: " + Music);
                    Props.PrintUnusedWarnings();
                    break;
                case "spawnpoint":
                    WorldmapSpawnPoint SpawnPoint = new WorldmapSpawnPoint();
                    SpawnPoint.Parse(iter.List);
                    SpawnPoints.Add(SpawnPoint.Name, SpawnPoint);
                    break;
                default:
                    GameObject Object = ParseObject(iter.Key, iter.List);
                    if(Object != null)
                        AddObject(Object);
                    break;
            }
        }

        Player = new WorldmapTux(this);
        AddObject(Player);
        Spawn("default");
    }

    private GameObject ParseObject(string Name, List Data) {
        switch(Name) {
            case "tilemap":
                Tilemap tilemap = new Tilemap(Tileset, Data);
                if(tilemap.Solid) {
                    if(Solids != null)
                        Console.WriteLine("Warning: 2 solid tilemaps specified");
                    Solids = tilemap;
                    ObjectGrid = new Field<List<WorldmapObject> >(tilemap.Width, tilemap.Height, (List<WorldmapObject>) null);
                }
                return tilemap;
            case "level":
                return new WorldmapLevel(this, Data);
            default:
                Console.WriteLine("Unknown Object " + Name);
                break;
        }

        return null;
    }
    
    public void Spawn(string SpawnPoint) {
        WorldmapSpawnPoint Point = SpawnPoints[SpawnPoint];
        Player.Spawn(Point.Pos);
    }
    
    public override void Update(float ElapsedTime) {
        base.Update(ElapsedTime);
        
        // check for collisions
        if(ObjectGrid[Player.TilemapPos] != null) {
            foreach(WorldmapObject Object in ObjectGrid[Player.TilemapPos]) {
                HandleCollision(Player, Object);
            }
        }
    }
    
    public void HandleCollision(WorldmapTux Player, WorldmapObject Object) {
        Object.Touch(Player, true);
    }
    
    public IEnumerable<WorldmapObject> GetObjectsAt(FieldPos Pos) {
        List<WorldmapObject> List = ObjectGrid[Pos];
        if(List == null)
            return new List<WorldmapObject> (0);
        return List;
    }
    
    public override void AddObject(GameObject Object) {
        base.AddObject(Object);
        
        if(Object is WorldmapObject) {
            WorldmapObject WObject = (WorldmapObject) Object;
            if(ObjectGrid[WObject.Pos] == null)
                ObjectGrid[WObject.Pos] = new List<WorldmapObject> ();
            ObjectGrid[WObject.Pos].Add(WObject);
        }
    }
    
    protected override void RemoveObject(GameObject Object) {
        base.RemoveObject(Object);
        
        if(Object is WorldmapObject) {
            WorldmapObject WObject = (WorldmapObject) Object;
            ObjectGrid[WObject.Pos].Remove(WObject);
        }
    }
}
