using System.Collections.Generic;

public interface ITileset {
	uint TileWidth {
		get;
	}
	uint TileHeight {
		get;
	}

	ITile Get(uint Id);
	bool IsValid(uint Id);
	IEnumerator<uint> GetValidTileIds();
}
