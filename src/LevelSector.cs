using Lisp;
using System;
using System.Collections.Generic;

public class Sector : SectorBase {
    public float Gravity;
    private Dictionary<string, SpawnPoint> SpawnPoints
        = new Dictionary<string, SpawnPoint>();
    private Level Level;
    public Tilemap Solids;
    
    public void Parse(Level Level, List list) {
        this.Level = Level;
        
        Properties props = new Properties(list);
        props.Get("name", ref Name);
        props.Get("music", ref Music);
        props.Get("gravity", ref Gravity);
        
        LispIterator iter = new LispIterator(list);
        while(iter.MoveNext()) {
            switch(iter.Key) {
                case "spawnpoint":
                    SpawnPoint SpawnPoint = new SpawnPoint();
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
    }

    public GameObject ParseObject(string Name, List Data) {
        switch(Name) {
            case "tilemap":
                Tilemap tilemap = new Tilemap(Level.Tileset, Data);
                if(tilemap.Solid) {
                    if(Solids != null)
                        Console.WriteLine("Warning: 2 solid tilemaps specified");
                    Solids = tilemap;
                }
                return tilemap;
            default:
                Console.WriteLine("Unknown Object " + Name);
                break;
        }

        return null;
    }                
}
