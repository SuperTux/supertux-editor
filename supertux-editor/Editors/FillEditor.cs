//  $Id$
using DataStructures;
using OpenGl;
using System;
using Gdk;
using Undo;

public sealed class FillEditor : TileEditorBase, IEditor, IDisposable {

	public event RedrawEventHandler Redraw;

	public FillEditor(IEditorApplication application, Tilemap Tilemap, Tileset Tileset, Selection selection)
		: base(application, Tilemap, Tileset) {
		this.selection = selection;
		selection.Changed += OnSelectionChanged;
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

	public void Dispose()
	{
		selection.Changed -= OnSelectionChanged;
	}

	public void OnMouseButtonPress(Vector mousePos, int button, ModifierType Modifiers)
	{
		if (Tilemap == null) return;

		UpdateMouseTilePos(mousePos);

		if (button == 1) {

			// save backup of Tilemap
			tilemapBackup = Tilemap.SaveState();

			if ((selection.Width == 1) && (selection.Height == 1)) {
				FloodFill(MouseTilePos, selection[0, 0]);
			}
			LastDrawPos = MouseTilePos;
			drawing = true;
			Redraw();
		}
		if(button == 3) {
			if(MouseTilePos.X < 0 || MouseTilePos.Y < 0
				|| MouseTilePos.X >= Tilemap.Width
				|| MouseTilePos.Y >= Tilemap.Height)
				return;

			SelectStartPos = MouseTilePos;
			selecting = true;
			UpdateSelection();
			Redraw();
		}
	}

	public void OnMouseButtonRelease(Vector mousePos, int button, ModifierType Modifiers)
	{
		if (Tilemap == null) return;

		UpdateMouseTilePos(mousePos);

		if(button == 1) {
			drawing = false;

			// use backup of Tilemap to create undo command
			TilemapModifyCommand command = new TilemapModifyCommand("Flood Fill on Tilemap \""+Tilemap.Name+"\"", Tilemap, tilemapBackup, Tilemap.SaveState());
			UndoManager.AddCommand(command);

		}
		if(button == 3) {
			UpdateSelection();

			selection.FireChangedEvent();
			selecting = false;
		}

		Redraw();
	}

	public void OnMouseMotion(Vector mousePos, ModifierType Modifiers)
	{
		if (Tilemap == null) return;

		if (UpdateMouseTilePos(mousePos)) {
			if(selection.Width == 0 || selection.Height == 0)
				return;

			if (drawing &&
			    ((Modifiers & ModifierType.ShiftMask) != 0 ||
			     ((LastDrawPos.X - MouseTilePos.X) % selection.Width == 0 &&
			      (LastDrawPos.Y - MouseTilePos.Y) % selection.Height == 0
			     )
			    )
			   ) {
				LastDrawPos = MouseTilePos;
				if ((selection.Width == 1) && (selection.Height == 1))
					FloodFill(MouseTilePos, selection[0, 0]);
			}
			if(selecting)
				UpdateSelection();
			Redraw();
		}
	}

	private void OnSelectionChanged() {
		Redraw();
	}
}
