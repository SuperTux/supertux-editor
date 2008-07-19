//  $Id$
using DataStructures;
using Gdk;

public sealed class FillEditor : TileEditorBase, IEditor {

	public FillEditor(IEditorApplication application, Tileset Tileset, Selection selection)
		: base(application, Tileset, selection) {
		ActionName = "Flood Fill";
	}

	private void FloodFill(FieldPos pos, int new_tile) {
		if (application.CurrentTilemap[pos] != new_tile)
			FloodFillAt(pos, application.CurrentTilemap[pos], new_tile);
	}

	private void FloodFillAt(FieldPos pos, int oldId, int newId) {
		if (!application.CurrentTilemap.InBounds(pos)) return;
		if (application.CurrentTilemap[pos] != oldId) return;
		application.CurrentTilemap[pos] = newId;
		FloodFillAt(pos.Up, oldId, newId);
		FloodFillAt(pos.Down, oldId, newId);
		FloodFillAt(pos.Left, oldId, newId);
		FloodFillAt(pos.Right, oldId, newId);
	}

	public override void EditorAction(ModifierType Modifiers)
	{
		if ((selection.Width == 1) && (selection.Height == 1)) {
			FloodFill(MouseTilePos, selection[0, 0]);
		}
	}
}
