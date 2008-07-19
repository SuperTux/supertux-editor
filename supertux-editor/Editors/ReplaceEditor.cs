//  $Id$
using DataStructures;
using OpenGl;
using System;
using Gdk;

public sealed class ReplaceEditor : TileEditorBase, IEditor {

	public ReplaceEditor(IEditorApplication application, Tilemap Tilemap, Tileset Tileset, Selection selection)
		: base(application, Tilemap, Tileset, selection) {
		ActionName = "Replace Tiles";
	}

	private void Replace(int oldId, int newId) {
		for (int x = 0; x < Tilemap.Width; x++) {
			for (int y = 0; y < Tilemap.Height; y++) {
				if (Tilemap[x,y] == oldId) Tilemap[x,y] = newId;
			}
		}
	}

	public override void EditorAction(ModifierType Modifiers)
	{
		if ((selection.Width == 1) && (selection.Height == 1)) {
			Replace(Tilemap[MouseTilePos], selection[0,0]);
		}
	}
}
