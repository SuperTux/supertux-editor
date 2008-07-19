//  $Id$
using Gdk;

public sealed class TilemapEditor : TileEditorBase, IEditor {

	public TilemapEditor(IEditorApplication application, Tileset Tileset, Selection selection)
		: base(application, Tileset, selection) {
		ActionName = "Change Tiles";
	}

	public override void EditorAction(ModifierType Modifiers)
	{
		selection.ApplyToTilemap(MouseTilePos, application.CurrentTilemap, ((Modifiers & ModifierType.ControlMask) == 0));
	}
}
