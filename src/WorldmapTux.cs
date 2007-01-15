using System;
using System.Collections.Generic;
using SceneGraph;
using DataStructures;
using Sprites;

/// <summary>
/// Description of WorldmapTux.
/// </summary>
public class WorldmapTux : GameObject {
    public enum Direction {
        NORTH = 0, EAST, SOUTH, WEST, STOP
    }

    private FieldPos _Pos;
    public FieldPos TilemapPos {
        set {
            _Pos = value;
            MovementDelta = 0f;
            MovementDirection = Direction.STOP;
            FromDirection = Direction.STOP;
            Sprite.Pos.X = value.X * 32;
            Sprite.Pos.Y = value.Y * 32;
        }
        get {
            return _Pos;
        }
    }

    public Vector Pos {
        get {
            return Sprite.Pos;
        }
    }

	public Direction MovementDirection = Direction.STOP;
	private Direction FromDirection = Direction.STOP;
	private float MovementDelta;
	private WorldmapSector Sector;
	private Sprite Sprite;
	private Controller Controller;
	//private List<WorldmapObject> TouchedObjects = new List<WorldmapObject>();
	private const float SPEED = 64.0f;

	public WorldmapTux(WorldmapSector Sector)
	{
	    this.Sector = Sector;
	    Controller = Application.Controller;
	    Sprite = SpriteManager.Create("worldmap/common/tux.sprite");
	}

	public override void SetupGraphics(Layer Layer) {
	    Layer.Add(10f, Sprite);
	}

	public override void Update(float ElapsedTime) {
	    CheckController();

	    if(MovementDirection != Direction.STOP) {
	        Vector Movement = GetMovement();
	        float LastDelta = MovementDelta;
	        MovementDelta += ElapsedTime * SPEED;
	        if(LastDelta < 0 && MovementDelta > 0) {
	            CheckNextTile();
	        }
	        while(MovementDelta >= 32f) {
	            MovementDelta -= 32f;
	            AdvanceTilemapPos();
	            CheckNextTile();
	        }
	        Sprite.Pos = new Vector(TilemapPos.X * 32, TilemapPos.Y * 32) + Movement * MovementDelta;
	    }
	}

	private void CheckController() {
	    if(Controller.IsDown(Control.UP) && !Controller.IsDown(Control.DOWN)) {
	        if(MovementDirection == Direction.SOUTH
	           || MovementDirection == Direction.STOP) {
	            MovementDirection = Direction.NORTH;
	            MovementDelta = -MovementDelta;
	        }
	    }
	    if(Controller.IsDown(Control.DOWN) && !Controller.IsDown(Control.UP)) {
	        if(MovementDirection == Direction.NORTH
	           || MovementDirection == Direction.STOP) {
	            MovementDirection = Direction.SOUTH;
	            MovementDelta = -MovementDelta;
	        }
	    }
	    if(Controller.IsDown(Control.LEFT) && !Controller.IsDown(Control.RIGHT)) {
	        if(MovementDirection == Direction.EAST
	           || MovementDirection == Direction.STOP) {
	            MovementDirection = Direction.WEST;
	            MovementDelta = -MovementDelta;
	        }
	    }
	    if(Controller.IsDown(Control.RIGHT) && !Controller.IsDown(Control.LEFT)) {
	        if(MovementDirection == Direction.WEST
	           || MovementDirection == Direction.STOP) {
	            MovementDirection = Direction.EAST;
	            MovementDelta = -MovementDelta;
	        }
	    }
	    if(Controller.WasPressed(Control.JUMP)) {
	        foreach(WorldmapObject Object in Sector.GetObjectsAt(TilemapPos)) {
	            Object.Use(this);
	        }
	    }
	}

	private Vector GetMovement() {
	    switch(MovementDirection) {
	        case Direction.NORTH:
	            return new Vector(0f, -1.0f);
	        case Direction.SOUTH:
	            return new Vector(0f, 1.0f);
	        case Direction.WEST:
	            return new Vector(-1.0f, 0f);
	        case Direction.EAST:
	            return new Vector(1.0f, 0f);
	    }

	    return new Vector(0f, 0f);
	}

	private void AdvanceTilemapPos() {
	    switch(MovementDirection) {
	        case Direction.NORTH:
	            _Pos.Y--;
	            break;
	        case Direction.SOUTH:
	            _Pos.Y++;
	            break;
	        case Direction.WEST:
	            _Pos.X--;
	            break;
	        case Direction.EAST:
	            _Pos.X++;
	            break;
	    }
	}

	private void CheckNextTile() {
	    FromDirection = (Direction) ((((int) MovementDirection) + 2) % 4);
	    Tile NextTile = Sector.Solids.GetTile(_Pos);
	    int dir = (int) FromDirection;
	    while(!IsDirectionSupported(NextTile, MovementDirection)) {
	        dir = (dir + 1) % 4;
	        MovementDirection = (Direction) dir;
	        if(MovementDirection == FromDirection) {
	            MovementDirection = Direction.STOP;
	            break;
	        }
	    }

	    foreach(WorldmapObject Object in Sector.GetObjectsAt(_Pos)) {
	        if(Object is WorldmapLevel)
	            MovementDirection = Direction.STOP;
	    }

	    if(NextTile.HasAttribute(Tile.Attribute.WORLDMAP_STOP))
	        MovementDirection = Direction.STOP;

	    if(MovementDirection == Direction.STOP)
	        MovementDelta = 0f;
	}

	private bool IsDirectionSupported(Tile Tile, Direction Dir) {
	    switch(Dir) {
	        case Direction.NORTH:
	            return (Tile.Attributes & Tile.Attribute.WORLDMAP_NORTH) != 0;
	        case Direction.SOUTH:
	            return (Tile.Attributes & Tile.Attribute.WORLDMAP_SOUTH) != 0;
	        case Direction.WEST:
	            return (Tile.Attributes & Tile.Attribute.WORLDMAP_WEST) != 0;
	        case Direction.EAST:
	            return (Tile.Attributes & Tile.Attribute.WORLDMAP_EAST) != 0;
	    }

	    return false;
	}

	public void Spawn(FieldPos TilemapPos) {
	    this.TilemapPos = TilemapPos;
	}
}
