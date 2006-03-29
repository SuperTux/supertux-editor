using Lisp;
using Sprites;
using DataStructures;
using SceneGraph;
using System;

public class WorldmapLevel : WorldmapObject {
    private Sprite Sprite;
    public override FieldPos Pos {
        get {
            return new FieldPos((int) (Sprite.Pos.X-16) / 32,
                                (int) (Sprite.Pos.Y-16) / 32);
        }
    }
    public string LevelFile;
    public string LevelName;

    public WorldmapLevel(WorldmapSector Sector, List Data) 
        : base(Sector) {
        string SpriteName = "worldmap/common/leveldot.sprite";
        Properties Props = new Properties(Data);
        Props.Get("name", ref LevelFile);
        FieldPos LevelPos = new FieldPos();
        Props.Get("x", ref LevelPos.X);
        Props.Get("y", ref LevelPos.Y);
        Props.Get("sprite", ref SpriteName);
        Props.PrintUnusedWarnings();
        
        Sprite = SpriteManager.Create(SpriteName);
        Sprite.Pos = new Vector(LevelPos.X*32 + 16, LevelPos.Y*32 + 16);
    }
    
    public override void SetupGraphics(Layer Layer) {
        Layer.Add(1.0f, Sprite);
    }
    
    public override void Use(WorldmapTux Player) {
        Console.WriteLine("Play Level " + LevelFile);
        Solved = true;
    }
    
    public bool Solved {
        set {
            Sprite.Action = value ? "solved" : "default";
        }
        get {
            return Sprite.Action == "solved";
        }
    }
}
