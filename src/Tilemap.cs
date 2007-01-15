using System;
using DataStructures;
using SceneGraph;
using Lisp;
using System.Collections.Generic;

public class Tilemap : GameObject {
    public Tileset Tileset;
    public Field<uint> Field;
    public bool Solid;

    public Tilemap(Tileset Tileset, Lisp.List Data) {
        this.Tileset = Tileset;

        Properties Props = new Properties(Data);
        uint Width = 0;
        uint Height = 0;
        Props.Get("width", ref Width);
        Props.Get("height", ref Height);
        if(Width == 0 || Height == 0)
            throw new Exception("Width or Height of Tilemap invalid");

        List<uint> Tiles = new List<uint>();
        Props.GetUIntList("tiles", Tiles);
        if(Tiles.Count != (int) (Width * Height))
            throw new Exception("TileCount != Width*Height");
        Props.Get("solid", ref Solid);
        Props.PrintUnusedWarnings();

        Field = new Field<uint>(Tiles, Width, Height);
    }

    public uint Width {
        get {
            return Field.Width;
        }
    }
    public uint Height {
        get {
            return Field.Height;
        }
    }
    public uint this[uint X, uint Y] {
        get {
            return Field[X, Y];
        }
        set {
            Field[X, Y] = value;
        }
    }
    public uint this[FieldPos Pos] {
        get {
            return Field[Pos];
        }
        set {
            Field[Pos] = value;
        }
    }

    public Tile GetTile(FieldPos Pos) {
        return Tileset.Get(this[Pos]);
    }

    public override void SetupGraphics(Layer Layer) {
        Node node = new TilemapNode(Field, Tileset);
        Layer.Add(0f, node);
    }
}

