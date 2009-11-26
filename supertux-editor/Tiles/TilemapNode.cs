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

using SceneGraph;
using DataStructures;
using System;

/// <summary>
///	A SceneGraph Node which draws a tilemap
/// </summary>
public sealed class TilemapNode : Node {
	private Tilemap tilemap;
	private Tileset Tileset;

	public TilemapNode(Tilemap tilemap, Tileset Tileset) {
		this.tilemap = tilemap;
		this.Tileset = Tileset;
	}

	public void Draw(Gdk.Rectangle cliprect) {
		tilemap.UpdatePos();
		cliprect.X -= (int)tilemap.X;
		cliprect.Y -= (int)tilemap.Y;

		uint start_x = (uint) Math.Max(0, cliprect.X / 32 - 1);
		uint start_y = (uint) Math.Max(0, cliprect.Y / 32 - 1);
		//HACK: Couldn't find any "inrange" or such
		uint end_x = (uint) Math.Max(0, Math.Min(tilemap.Width, (cliprect.X + cliprect.Width) / 32 + 1));
		uint end_y = (uint) Math.Max(0, Math.Min(tilemap.Height, (cliprect.Y + cliprect.Height) / 32 + 1));
		Tile Tile;
		for (uint y = start_y; y < end_y; ++y) {
			for (uint x = start_x; x < end_x; ++x) {
				Tile = Tileset.Get(tilemap[x, y]);
				if (Tile != null)
					Tile.DrawEditor(new Vector(x * 32 + tilemap.X,
									y * 32 + tilemap.Y));
				else
					LogManager.Log(LogLevel.Warning,
					               "Tile {0} is null?! The tile with id {0} at {1},{2} is probably invalid.",
					               tilemap[x, y], x, y);
			}
		}
	}
}

/* EOF */
