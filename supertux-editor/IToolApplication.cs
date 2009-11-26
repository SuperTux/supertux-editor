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

/// <summary>
/// Interface to the editor application.
/// </summary>
public interface IToolApplication
{
	void ChangeCurrentLevel(Level NewLevel);
	void ChangeCurrentSector(Sector Sector);
	void ChangeCurrentTilemap(Tilemap Tilemap);
	void SetTool(ITool editor);
	void SetToolSelect();
	void SetToolTiles();
	void SetToolObjects();
	void SetToolBrush();
	void SetToolFill();
	void SetToolReplace();
	void SetToolPath();
	void EditCurrentCamera();
	void DeleteCurrentPath();
	void EditProperties(object Object, string title);
	void PrintStatus(string message);

	bool SnapToGrid{
		get;
	}

	SectorRenderer CurrentRenderer {
		get;
	}

	Sector CurrentSector {
		get;
		set;
	}

	Level CurrentLevel {
		get;
		set;
	}

	Tilemap CurrentTilemap {
		get;
		set;
	}

	/// <summary>
	/// Occurs when a new level is loaded.
	/// </summary>
	event LevelChangedEventHandler LevelChanged;
	/// <summary>
	/// Occurs when user changes sector in a level.
	/// </summary>
	event SectorChangedEventHandler SectorChanged;
	event TilemapChangedEventHandler TilemapChanged;
}

public delegate void LevelChangedEventHandler(Level NewLevel);
public delegate void SectorChangedEventHandler(Level Level, Sector NewSector);
public delegate void TilemapChangedEventHandler(Tilemap Tilemap);

/* EOF */
