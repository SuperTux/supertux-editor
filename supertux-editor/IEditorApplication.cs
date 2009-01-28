//  $Id$
/// <summary>
/// Interface to the editor application.
/// </summary>
public interface IEditorApplication
{
	void ChangeCurrentLevel(Level NewLevel);
	void ChangeCurrentSector(Sector Sector);
	void ChangeCurrentTilemap(Tilemap Tilemap);
	void SetEditor(IEditor editor);
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
