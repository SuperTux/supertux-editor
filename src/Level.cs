using System;
using System.Collections.Generic;
using System.IO;
using Lisp;

public class Level {
    public string Name;
    public string Author;
    public List<Sector> Sectors = new List<Sector>();
    private Tileset _tileSet = new Tileset("tilesets/tiles.strf");
    public Tileset Tileset {
        get {
            return _tileSet;
        }
    }
    
    public void Load(string Filename) {
        List levelLisp = Util.Load(Filename, "supertux-level");

        Properties props = new Properties(levelLisp);
        int version = 1;
        props.Get("version", ref version);
        if(version == 1)
            throw new Exception("Old Level format not supported");
        if(version > 2)
            Console.WriteLine("Warning: Level Format newer than application");

        props.Get("name", ref Name);
        props.Get("author", ref Author);

        LispIterator iter = new LispIterator(levelLisp);
        while(iter.MoveNext()) {
            switch(iter.Key) {
                case "sector":
                    Sector sector = new Sector();
                    sector.Parse(this, iter.List);
                    break;
                default:
                    Console.WriteLine("Ignoring unknown tag '" + iter.Key + "' in level");
                    break;
            }   
        }        
    }

    public void Save(TextReader reader) {
        // TODO ...
    }
}
