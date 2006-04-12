using DataStructures;
using System.Collections;
using System.Collections.Generic;
using Drawing;
using Lisp;
using System;

public class Tile {
    public enum Attribute {
        /** solid tile that is indestructable by Tux */
        SOLID     = 0x0001,
        /** uni-directional solid tile */
        UNISOLID  = 0x0002,
        /** a brick that can be destroyed by jumping under it */
        BRICK     = 0x0004,
        /** an ice brick that makes tux sliding more than usual */
        ICE       = 0x0008,
        /** a water tile in which tux starts to swim */
        WATER     = 0x0010,
        /** a tile that hurts the player if he touches it */
        SPIKE     = 0x0020,
        /** Bonusbox, content is stored in \a data */
        FULLBOX   = 0x0040,
        /** Tile is a coin */
        COIN      = 0x0080,
        /** the level should be finished when touching a goaltile.
         * if data is 0 then the endsequence should be triggered, if data is 1
         * then we can finish the level instantly.
         */
        GOAL      = 0x0100,
        /** slope tile */
        SLOPE     = 0x0200,

        // worldmap flags
        WORLDMAP_NORTH = 0x0001,
        WORLDMAP_SOUTH = 0x0002,
        WORLDMAP_EAST  = 0x0004,
        WORLDMAP_WEST  = 0x0008,

        WORLDMAP_STOP  = 0x0010
    };

    /** tile id */
    public int Id;
    /** tile attributes (see Attributes enum) */
    public Attribute Attributes;
    /** General purpose data attached to a tile (content of a box, type of
     * coin)
     */
    public int Data;
    /** Frames per second for the animation */
    public float AnimFps;
    /** ResourceNames of the tile images */
	public struct ImageResource {
		public string Filename;
		public int x, y, w, h;
	}
	
    public List<ImageResource> Images;
    private List<Surface> Surfaces;
    public List<ImageResource> EditorImages;
	private List<Surface> EditorSurfaces;

    public Tile() {
    }
    
    public bool HasAttribute(Attribute Attrib) {
        return (Attributes & Attrib) != 0;
    }

    public void LoadSurfaces(string BaseDir, bool Editor) {
        if(Surfaces != null)
            return;
		
		if(Images != null) {
			Surfaces = new List<Surface>();
			foreach(ImageResource Resource in Images) {
				Surfaces.Add(LoadSurface(BaseDir, Resource));
			}
		}

		if(Editor && EditorImages != null) {
			EditorSurfaces = new List<Surface>();
			foreach(ImageResource Resource in EditorImages) {
				EditorSurfaces.Add(LoadSurface(BaseDir, Resource));
			}
		}
    }

	private Surface LoadSurface(string BaseDir, ImageResource Resource) {
		if(Resource.w > 0) {
			return new Surface(BaseDir + "/" + Resource.Filename,
							   Resource.x, Resource.y,
							   Resource.w, Resource.h);
		} else {
			return new Surface(BaseDir + "/" + Resource.Filename);
		}
	}

    public void Draw(Vector pos) {
		if(Surfaces != null)
			Surfaces[0].Draw(pos);
    }

	public void DrawEditor(Vector pos) {
		if(EditorSurfaces != null) {
			EditorSurfaces[0].Draw(pos);
			return;
		}
		Draw(pos);
	}
}

