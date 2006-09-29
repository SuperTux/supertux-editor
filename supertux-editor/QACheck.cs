//  $Id$
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


}
