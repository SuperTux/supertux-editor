//  SuperTux Editor
//  Copyright (C) 2007 Arvid Norlander <anmaster AT berlios DOT de>
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

using System;
using System.Collections.Generic;
using Gtk;

/// <summary>
/// Functions to check for common problems in levels.
/// </summary>
public static class QACheck
{

	#region CheckIds
	/// <summary>
	/// Check a tile block for nonexistent tile ids.
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


	public static void CheckIds(IEditorApplication application, Sector sector, bool AlertGood) {
		System.Text.StringBuilder sb = new System.Text.StringBuilder("These tilemaps have bad ids in sector " + sector.Name + ":");
		List<int> invalidtiles;
		// Any bad found yet?
		bool bad = false;
		foreach (Tilemap tilemap in sector.GetObjects(typeof(Tilemap))) {
			invalidtiles = CheckIds(tilemap, application.CurrentLevel.Tileset);
			if (invalidtiles.Count != 0) {
				bad = true;
				if (String.IsNullOrEmpty(tilemap.Name))
					sb.Append(Environment.NewLine + "Tilemap (" + tilemap.Layer + ")");
				else
					sb.Append(Environment.NewLine + tilemap.Name + " (" + tilemap.Layer + ")");
			}
		}

		MessageType msgtype;
		string message;
		if (! bad) {
			if (! AlertGood)
				return;
			msgtype = MessageType.Info;
			message = "No invalid tile ids in any tilemap in sector " + sector.Name + ".";
		} else {
			msgtype = MessageType.Warning;
			message = sb.ToString();
		}
		MessageDialog md = new MessageDialog(null, DialogFlags.DestroyWithParent,
		                                     msgtype, ButtonsType.Close, message);
		md.Run();
		md.Destroy();
	}
	#endregion CheckIds

	// 26 -> 83
	// 63 -> 70
	// 101 -> 93
	/// <summary>
	///		A map for replacing deprecated tiles with new ones automatically.
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

	#region ReplaceDeprecatedTiles
	/// <summary>
	/// Replace deprecated tiles in tileblocks.
	/// </summary>
	/// <param name="tiles">The tileblock</param>
	/// <param name="TilesetFile">The TileSet file.</param>
	private static void ReplaceDeprecatedTiles(TileBlock tiles, string TilesetFile) {
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

	public static void ReplaceDeprecatedTiles(Level level) {
		foreach (Sector sector in level.Sectors) {
			foreach (Tilemap tilemap in sector.GetObjects(typeof(Tilemap)))
				ReplaceDeprecatedTiles(tilemap, level.TilesetFile);
		}
	}
	#endregion ReplaceDeprecatedTiles

	#region CheckDirection
	private static void CheckBadDirection(SimpleDirObject dirobject) {
		if (dirobject.Direction == SimpleDirObject.Directions.auto) {
			string message = String.Format("The {0} at x={1} y={2} has direction set to auto. Setting the direction of {0} objects to auto is a bad idea.",
			                               dirobject.GetType().Name, dirobject.X, dirobject.Y);
			MessageDialog md = new MessageDialog(null, DialogFlags.DestroyWithParent, MessageType.Warning, ButtonsType.Close, message);
			md.Run();
			md.Destroy();
		}
	}

	public static void CheckObjectDirections(Level level) {
		foreach (Sector sector in level.Sectors) {
			// This is hackish, I know
			foreach (SimpleDirObject dirobject in sector.GetObjects(typeof(Ispy)))
				CheckBadDirection(dirobject);
			foreach (SimpleDirObject dirobject in sector.GetObjects(typeof(DartTrap)))
				CheckBadDirection(dirobject);
			foreach (SimpleDirObject dirobject in sector.GetObjects(typeof(Dispenser)))
				CheckBadDirection(dirobject);
		}
	}
	#endregion CheckDirection

	public static void CheckLicense(Level level) {
		if (String.IsNullOrEmpty(level.License)) {
			MessageDialog md = new MessageDialog(null, DialogFlags.DestroyWithParent,
			                                     MessageType.Warning, ButtonsType.Close, "No license is set for this level! Please make sure to fix this (setting is under Level menu -> Properties).");
			md.Run();
			md.Destroy();
		}
	}

}

/* EOF */
