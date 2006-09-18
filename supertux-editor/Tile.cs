using DataStructures;
using System.Collections;
using System.Collections.Generic;
using Drawing;
using Lisp;
using System;

public sealed class Tile {
	public enum Attribute {
		/// <summary>solid tile that is indestructable by Tux</summary>
		SOLID     = 0x0001,
		/// <summary>uni-directional solid tile</summary>
		UNISOLID  = 0x0002,
		/// <summary>a brick that can be destroyed by jumping under it</summary>
		BRICK     = 0x0004,
		/// <summary>an ice brick that makes tux sliding more than usual</summary>
		ICE       = 0x0008,
		/// <summary>a water tile in which tux starts to swim</summary>
		WATER     = 0x0010,
		/// <summary>a tile that hurts the player if he touches it</summary>
		SPIKE     = 0x0020,
		/// <summary>Bonusbox, content is stored in <see cref="Data">data</see></summary>
		FULLBOX   = 0x0040,
		/// <summary>Tile is a coin</summary>
		COIN      = 0x0080,
		/// <summary>the level should be finished when touching a goaltile.</summary>
		/// <remarks>
		/// if <see cref="Data">data</see> is 0 then the endsequence should be
		/// triggered, if <see cref="Data">data</see> is 1 then we can finish
		/// the level instantly.
		/// </remarks>
		GOAL      = 0x0100,
		/// <summary>slope tile</summary>
		SLOPE     = 0x0200,

		// worldmap flags
		WORLDMAP_NORTH = 0x0001,
		WORLDMAP_SOUTH = 0x0002,
		WORLDMAP_EAST  = 0x0004,
		WORLDMAP_WEST  = 0x0008,

		WORLDMAP_STOP  = 0x0010
	};

	/// <summary>tile id</summary>
	public int Id;
	/// <summary>tile attributes (see Attributes enum)</summary>
	public Attribute Attributes;
	/// <summary>General purpose data attached to a tile (content of a box, type of coin)</summary>
	public int Data;
	/// <summary>Frames per second for the animation</summary>
	public float AnimFps;
	/// <summary>ResourceNames of the tile images</summary>
	public struct ImageResource {
		public string Filename;
		public int x, y, w, h;

		public static bool operator ==(ImageResource i1, ImageResource i2) {
			return i1.Filename == i2.Filename && i1.x == i2.x && i1.y == i2.y && i1.w == i2.w && i1.h == i2.h;
		}

		public static bool operator !=(ImageResource i1, ImageResource i2) {
			return i1.Filename != i2.Filename || i1.x != i2.x || i1.y != i2.y || i1.w != i2.w || i1.h != i2.h;
		}

		public override bool Equals(object obj) {
			if (!(obj is ImageResource))
				return false;
			ImageResource res = (ImageResource)obj;
			return this == res;
		}

		public override int GetHashCode() {
			return base.GetHashCode();
		}

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

	private static Surface LoadSurface(string BaseDir, ImageResource Resource) {
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
