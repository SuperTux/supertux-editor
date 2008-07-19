//  $Id$
using DataStructures;
using OpenGl;
using System;
using Gdk;
using Undo;

public sealed class TilemapEditor : TileEditorBase, IEditor {

	public TilemapEditor(IEditorApplication application, Tilemap Tilemap, Tileset Tileset, Selection selection)
		: base(application, Tilemap, Tileset, selection) {
	}

	public void OnMouseButtonPress(Vector mousePos, int button, ModifierType Modifiers)
	{
		if (Tilemap == null) return;

		UpdateMouseTilePos(mousePos);

		if(button == 1) {

			// save backup of Tilemap
			tilemapBackup = Tilemap.SaveState();

			selection.ApplyToTilemap(MouseTilePos, Tilemap, ((Modifiers & ModifierType.ControlMask) == 0));
			LastDrawPos = MouseTilePos;
			drawing = true;
			FireRedraw();
		}
		if(button == 3) {
			if(MouseTilePos.X < 0 || MouseTilePos.Y < 0
			   || MouseTilePos.X >= Tilemap.Width
			   || MouseTilePos.Y >= Tilemap.Height)
				return;

			SelectStartPos = MouseTilePos;
			selecting = true;
			UpdateSelection();
			FireRedraw();
		}
	}

	public void OnMouseButtonRelease(Vector mousePos, int button, ModifierType Modifiers)
	{
		if (Tilemap == null) return;

		UpdateMouseTilePos(mousePos);

		if(button == 1) {
			drawing = false;

			// use backup of Tilemap to create undo command
			TilemapModifyCommand command = new TilemapModifyCommand("Change Tiles on Tilemap \""+Tilemap.Name+"\"", Tilemap, tilemapBackup, Tilemap.SaveState());
			UndoManager.AddCommand(command);

		}
		if(button == 3) {
			UpdateSelection();

			selection.FireChangedEvent();
			selecting = false;
		}

		FireRedraw();
	}

	public void OnMouseMotion(Vector mousePos, ModifierType Modifiers)
	{
		if (Tilemap == null) return;

		if (UpdateMouseTilePos(mousePos)) {
			if(selection.Width == 0 || selection.Height == 0)
				return;

			if(drawing &&
			   ( (Modifiers & ModifierType.ShiftMask) != 0 ||
			     ((LastDrawPos.X - MouseTilePos.X) % selection.Width == 0 &&
			      (LastDrawPos.Y - MouseTilePos.Y) % selection.Height == 0)
			   )
			  ) {
				LastDrawPos = MouseTilePos;
				selection.ApplyToTilemap(MouseTilePos, Tilemap, ((Modifiers & ModifierType.ControlMask) == 0));
			}
			if(selecting)
				UpdateSelection();
			FireRedraw();
		}
	}
}
