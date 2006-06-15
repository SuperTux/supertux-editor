public interface IEditorApplication
{
	void ChangeCurrentLevel(Level NewLevel);
	void ChangeCurrentSector(Sector Sector);
	void ChangeCurrentTilemap(Tilemap Tilemap);
	void SetEditor(IEditor editor);
	void EditProperties(object Object, string title);
	void PrintStatus( string message );
	void TakeUndoSnapshot(string actionTitle);
	
	SectorRenderer CurrentRenderer {
		get;
	}
	Sector CurrentSector {
		get;
	}

	event LevelChangedEventHandler LevelChanged;
	event SectorChangedEventHandler SectorChanged;
	event TilemapChangedEventHandler TilemapChanged;
}

public delegate void LevelChangedEventHandler(Level NewLevel);
public delegate void SectorChangedEventHandler(Level Level, Sector NewSector);
public delegate void TilemapChangedEventHandler(Tilemap Tilemap);

