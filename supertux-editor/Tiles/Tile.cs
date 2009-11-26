//  SuperTux Editor
//  Copyright (C) 2006 Matthias Braun <matze@braunis.de>
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using DataStructures;
using System.Collections;
using System.Collections.Generic;
using Drawing;
using Lisp;
using System;

public sealed class Tile {
	public enum Attribute {
		/// <summary>solid tile that is indestructible by Tux</summary>
		SOLID     = 0x0001,
		/// <summary>uni-directional solid tile</summary>
		UNISOLID  = 0x0002,
		/// <summary>a brick that can be destroyed by jumping under it</summary>
		BRICK     = 0x0004,
		/// <summary>the level should be finished when touching a goaltile.</summary>
		/// <remarks>
		/// if <see cref="Data">data</see> is 0 then the endsequence should be
		/// triggered, if <see cref="Data">data</see> is 1 then we can finish
		/// the level instantly.
		/// </remarks>
		GOAL      = 0x0008,
		/// <summary>slope tile</summary>
		SLOPE     = 0x0010,
		/// <summary>Bonusbox, content is stored in <see cref="Data">data</see></summary>
		FULLBOX   = 0x0020,
		/// <summary>Tile is a coin</summary>
		COIN      = 0x0040,

		/// <summary>an ice brick that makes tux sliding more than usual</summary>
		ICE       = 0x0100,
		/// <summary>a water tile in which tux starts to swim</summary>
		WATER     = 0x0200,
		/// <summary>a tile that hurts the player if he touches it</summary>
		HURTS     = 0x0400,
		/// <summary>for lava: WATER, HURTS, FIRE</summary>
		FIRE      = 0x0800,


	// TODO: Find out why are worldmap tile attributes stored in data(s)
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

	/// <summary>Returns a string describing the level-tile's attributes in human readable form.</summary>
	public string ParseLevelAttributes(){
		string result = "";
		if( HasAttribute(Attribute.SOLID) ){
			result += "Solid ";
		}
		if( HasAttribute(Attribute.UNISOLID) ){
			result += "Unisolid ";
		}
		if( HasAttribute(Attribute.BRICK) ){
			result += "Brick ";
		}
		if( HasAttribute(Attribute.GOAL) ){ //has data
			result += "Goal("+Data+") ";
		}
		if( HasAttribute(Attribute.SLOPE) ){
			result += "Slope ";
		}
		if( HasAttribute(Attribute.FULLBOX) ){ //has data
			result += "Fullbox("+Data+") ";
		}
		if( HasAttribute(Attribute.COIN) ){
			result += "Coin ";
		}
		if( HasAttribute(Attribute.ICE) ){
			result += "Ice ";
		}
		if( HasAttribute(Attribute.WATER) ){
			result += "Water ";
		}
		if( HasAttribute(Attribute.HURTS) ){
			result += "Hurts ";
		}
		if( HasAttribute(Attribute.FIRE) ){
			result += "Fire ";
		}

		if( result == "" ){
			result = "(none) ";
		}
		return result;
	}

	/// <summary>Returns a string describing the worldmap-tile's attributes in human readable form.</summary>
	// TODO: Find out why are worldmap attributes stored in data(s)
	public string ParseWorldmapAttributes(){
		string result = "";
		if( HasWMAttribute(Attribute.WORLDMAP_NORTH) ){
			result += "North ";
		}
		if( HasWMAttribute(Attribute.WORLDMAP_SOUTH) ){
			result += "South ";
		}
		if( HasWMAttribute(Attribute.WORLDMAP_WEST) ){
			result += "West ";
		}
		if( HasWMAttribute(Attribute.WORLDMAP_EAST) ){
			result += "East ";
		}
		if( HasWMAttribute(Attribute.WORLDMAP_STOP) ){
			result += "Stop ";
		}

		if( result == "" ){
			result = "(none) ";
		}
		return result;
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

	// TODO: Find out why are worldmap tile attributes stored in data(s)
	public bool HasWMAttribute(Attribute Attrib) {
		return ((Attribute)Data & Attrib) != 0;
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

/* EOF */
