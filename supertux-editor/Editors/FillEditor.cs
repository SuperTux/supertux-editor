//  $Id$
using DataStructures;
using OpenGl;
using System;
using Gdk;

public sealed class FillEditor : TileEditorBase, IEditor {

	public FillEditor(IEditorApplication application, Tilemap Tilemap, Tileset Tileset, Selection selection)
		: base(application, Tilemap, Tileset, selection) {
		ActionName = "Flood Fill";
	}

	private void FloodFill(FieldPos pos, int new_tile) {
		if (Tilemap[pos] != new_tile)
			FloodFillAt(pos, Tilemap[pos], new_tile);
	}

	private void FloodFillAt(FieldPos pos, int oldId, int newId) {
		if (!Tilemap.InBounds(pos)) return;
		if (Tilemap[pos] != oldId) return;
		Tilemap[pos] = newId;
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
