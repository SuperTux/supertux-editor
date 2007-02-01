//  $Id$
//
//  Copyright (C) 2007 Arvid Norlander <anmaster AT berlios DOT de>
//
//  This program is free software; you can redistribute it and/or
//  modify it under the terms of the GNU General Public License
//  as published by the Free Software Foundation; either version 2
//  of the License, or (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA
//  02111-1307, USA.
using System;
using System.Collections.Generic;

/// <summary>
/// Functions to check for common problems in levels.
/// </summary>
public static class QACheck
{

	/// <summary>
	/// Check a tile block for non existant tile ids.
	/// </summary>
	/// <param name="tiles">TileBlock to check</param>
	/// <param name="Tileset">Tileset where the ids should be defined</param>
	/// <returns>List of invalid tile ids.</returns>
	public static List<int> CheckIds(TileBlock tiles, Tileset Tileset) {
		List<int> invalidTiles = new List<int>();
		for (uint y = 0; y < tiles.Height; ++y) {
			for (uint x = 0; x < tiles.Width; ++x) {
				int TileId = tiles[x, y];
				if (!Tileset.IsValid(TileId)) {
					if (invalidTiles.IndexOf(TileId) == -1)
						invalidTiles.Add(TileId);
				}
			}
		}
		return invalidTiles;
	}


	// 26 -> 83
	// 63 -> 70
	// 101 -> 93
	/// <summary>
	///		A map for replacing deprecated tiles with new ones automaticly.
	/// </summary>
	private static SortedList<int, int> LevelReplaceMap = new SortedList<int, int>();

	/// <summary>
	/// Initialize the ReplaceMap.
	/// </summary>
	static QACheck() {
		// Keep this sorted on key to make it faster.
		LevelReplaceMap.Add(26, 83);
		LevelReplaceMap.Add(63, 70);
		LevelReplaceMap.Add(101, 93);
	}

	/// <summary>
	/// Replace deprecated tiles in tileblocks.
	/// </summary>
	/// <param name="tiles">The tileblock</param>
	/// <param name="TilesetFile">The TileSet file.</param>
	private static void ReplaceDepercatedTiles(TileBlock tiles, string TilesetFile) {
		// We don't have any worldmap one currently
		if (TilesetFile != "images/tiles.strf")
			return;
		for (uint y = 0; y < tiles.Height; ++y) {
			for (uint x = 0; x < tiles.Width; ++x) {
				int TileId = tiles[x, y];
				if (LevelReplaceMap.ContainsKey(TileId)) {
					tiles[x, y] = LevelReplaceMap[TileId];
					LogManager.Log(LogLevel.Info, "Replaced deprecated tile {0} with {1}", TileId, LevelReplaceMap[TileId]);
				}
			}
		}
	}

	public static void ReplaceDepercatedTiles(Level level) {
		foreach (Sector sector in level.Sectors) {
			foreach (Tilemap tilemap in sector.GetObjects(typeof(Tilemap)))
				ReplaceDepercatedTiles(tilemap, level.TilesetFile);
		}
	}


}
