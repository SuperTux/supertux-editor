/// <summary>
/// Interface to the editor application.
/// </summary>
public interface IEditorApplication
{
	void ChangeCurrentLevel(Level NewLevel);
	void ChangeCurrentSector(Sector Sector);
	void ChangeCurrentTilemap(Tilemap Tilemap);
	void SetEditor(IEditor editor);
	void EditProperties(object Object, string title);
	void PrintStatus( string message );
	void TakeUndoSnapshot(string actionTitle);
	
	bool SnapToGrid{
		get;
	}
	
	SectorRenderer CurrentRenderer {
		get;
	}
	Sector CurrentSector {
		get;
	}

	Level CurrentLevel {
		get;
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

