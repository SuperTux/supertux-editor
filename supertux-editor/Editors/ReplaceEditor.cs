//  $Id$
using Gdk;

public sealed class ReplaceEditor : TileEditorBase, IEditor {

	public ReplaceEditor(IEditorApplication application, Tileset Tileset, Selection selection)
		: base(application, Tileset, selection) {
		ActionName = "Replace Tiles";
	}

	private void Replace(int oldId, int newId) {
		for (int x = 0; x < application.CurrentTilemap.Width; x++) {
			for (int y = 0; y < application.CurrentTilemap.Height; y++) {
				if (application.CurrentTilemap[x,y] == oldId)
					application.CurrentTilemap[x,y] = newId;
			}
		}
	}

	public override void EditorAction(ModifierType Modifiers)
	{
		if ((selection.Width == 1) && (selection.Height == 1)) {
			Replace(application.CurrentTilemap[MouseTilePos], selection[0,0]);
		}
	}
}
