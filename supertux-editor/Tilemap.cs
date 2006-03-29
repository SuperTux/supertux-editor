using System;
using DataStructures;
using SceneGraph;
using Lisp;
using System.Collections.Generic;

public class Tilemap : Field<uint>, IGameObject, ICustomLispSerializer {
	[LispChild("layer")]
	public string LayerName;
	[LispChild("solid")]
	public bool Solid = false;
	[LispChild("speed")]
	public float Speed = 1.0f;
		
	public Tilemap() {
	}

	public float Layer {
		get {
			if(LayerName == "background")
				return -10f;
			else if(LayerName == "foreground")
				return 10f;
			else if(LayerName == "interactive")
				return 0f;
			else
				return 0f;
		}
	}
 
	public void CustomLispRead(Properties Props) {
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

		Assign(Tiles, Width, Height);
	}

	public void CustomLispWrite(Writer Writer) {
		Writer.Write("width", Width);
		Writer.Write("height", Height);
		Writer.Write("tiles", GetContentsArray());
	}

	public void FinishRead() {
	}
}

