//  $Id$
using DataStructures;
using OpenGl;
using System;
using Gdk;

public sealed class TilemapEditor : TileEditorBase, IEditor {

	public TilemapEditor(IEditorApplication application, Tilemap Tilemap, Tileset Tileset, Selection selection)
		: base(application, Tilemap, Tileset, selection) {
		ActionName = "Change Tiles";
	}

	public override void EditorAction(ModifierType Modifiers)
	{
		selection.ApplyToTilemap(MouseTilePos, Tilemap, ((Modifiers & ModifierType.ControlMask) == 0));
	}
}
