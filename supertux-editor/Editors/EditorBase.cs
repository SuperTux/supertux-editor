//  $Id$
//
//  Copyright (C) 2007 Arvid Norlander <anmaster AT berlios DOT de>
//
//  This program is free software; you can redistribute it and/or
//  modify it under the terms of the GNU General Public License
//  as published by the Free Software Foundation; either version 2
//  of the License, or (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA
//  02111-1307, USA.
using DataStructures;
using OpenGl;
using System;
using Gdk;
using Undo;

// TODO: More things should be moved into this class.
/// <summary>
/// Base class for all editors.
/// </summary>
public abstract class EditorBase {
	protected IEditorApplication application;
}

// TODO: More things should be moved into this class.
/// <summary>
/// Base class for editors editing objects.
/// </summary>
public abstract class ObjectEditorBase : EditorBase {
	protected Sector sector;

	/// <summary>
	/// Returns unit to snap to, based on passed Modifier keys and application settings
	/// </summary>
	protected int SnapValue(ModifierType Modifiers) {
		if ((Modifiers & ModifierType.ShiftMask) != 0) return 32;
		if ((Modifiers & ModifierType.ControlMask) != 0) return 16;
		if (application.SnapToGrid) return 32;
		return 0;
	}

}

/// <summary>
/// Base class for editors editing tilemaps.
/// </summary>
public abstract class TileEditorBase : EditorBase, IDisposable {

	public event RedrawEventHandler Redraw;

	protected Selection selection;
	protected bool drawing;
	protected bool selecting;
	protected FieldPos MouseTilePos;
	protected FieldPos SelectStartPos;
	protected FieldPos LastDrawPos;
	protected FieldPos SelectionP1;
	protected FieldPos SelectionP2;

	protected Tileset Tileset;

	internal TileBlock.StateData tilemapBackup; // saved OnMouseButtonPress

	public abstract void EditorAction(ModifierType Modifiers);
	public string ActionName;

	protected TileEditorBase(IEditorApplication application, Tileset Tileset, Selection selection) {
		this.application = application;
		this.Tileset = Tileset;
		this.selection = selection;
		selection.Changed += OnSelectionChanged;
	}

	public void Dispose()
	{
		selection.Changed -= OnSelectionChanged;
	}

	protected bool UpdateMouseTilePos(Vector MousePos) {
		//count position relative to current tilemap
		int XPos = (int) (MousePos.X - application.CurrentTilemap.X);
		int YPos = (int) (MousePos.Y - application.CurrentTilemap.Y);
		FieldPos NewMouseTilePos = new FieldPos(XPos / Tileset.TILE_WIDTH,
							YPos / Tileset.TILE_HEIGHT);

		if (XPos < 0) NewMouseTilePos.X--;	//Fix for negative X values
		if (YPos < 0) NewMouseTilePos.Y--;	//Fix for negative Y values

		if (NewMouseTilePos != MouseTilePos) {
			MouseTilePos = NewMouseTilePos;
			return true;
		}

		return false;
	}

	public virtual void Draw(Gdk.Rectangle cliprect) {
		if (!selecting) {
			gl.Color4f(1, 1, 1, 0.7f);
			Vector pos = new Vector(
				MouseTilePos.X * Tileset.TILE_WIDTH  + application.CurrentTilemap.X,
				MouseTilePos.Y * Tileset.TILE_HEIGHT + application.CurrentTilemap.Y);
			selection.Draw(pos, Tileset);
			gl.Color4f(1, 1, 1, 1);
		}
		if (selecting) {
			float left = SelectionP1.X * Tileset.TILE_WIDTH + application.CurrentTilemap.X;
			float top = SelectionP1.Y * Tileset.TILE_HEIGHT + application.CurrentTilemap.Y;
			float right = (SelectionP2.X + 1) * Tileset.TILE_WIDTH + application.CurrentTilemap.X;
			float bottom = (SelectionP2.Y + 1) * Tileset.TILE_HEIGHT + application.CurrentTilemap.Y;

			gl.Color4f(0, 0, 1, 0.7f);
			gl.Disable(gl.TEXTURE_2D);

			gl.Begin(gl.QUADS);
			gl.Vertex2f(left, top);
			gl.Vertex2f(right, top);
			gl.Vertex2f(right, bottom);
			gl.Vertex2f(left, bottom);
			gl.End();

			gl.Enable(gl.TEXTURE_2D);
			gl.Color4f(1, 1, 1, 1);
		}
	}

	public void OnMouseButtonPress(Vector mousePos, int button, ModifierType Modifiers)
	{
		if (application.CurrentTilemap == null) return;

		UpdateMouseTilePos(mousePos);

		if(button == 1) {

			// save backup of Tilemap
			tilemapBackup = application.CurrentTilemap.SaveState();

			EditorAction(Modifiers);	//Call editor-specific part of code

			LastDrawPos = MouseTilePos;
			drawing = true;
			Redraw();
		}
		if(button == 3) {
			if(MouseTilePos.X < 0 || MouseTilePos.Y < 0
			   || MouseTilePos.X >= application.CurrentTilemap.Width
			   || MouseTilePos.Y >= application.CurrentTilemap.Height)
				return;

			SelectStartPos = MouseTilePos;
			selecting = true;
			UpdateSelection();
			Redraw();
		}
	}

	public void OnMouseButtonRelease(Vector mousePos, int button, ModifierType Modifiers)
	{
		if (application.CurrentTilemap == null) return;

		UpdateMouseTilePos(mousePos);

		if(button == 1) {
			drawing = false;

			// use backup of Tilemap to create undo command
			TilemapModifyCommand command = new TilemapModifyCommand(
				ActionName + " on Tilemap \"" + application.CurrentTilemap.Name + "\"",
				application.CurrentTilemap,
				tilemapBackup,
				application.CurrentTilemap.SaveState());
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
		if (application.CurrentTilemap == null) return;

		if (UpdateMouseTilePos(mousePos)) {
			if(selection.Width == 0 || selection.Height == 0)
				return;

			if(drawing &&
			   ( (Modifiers & ModifierType.ShiftMask) != 0 ||
			     ((LastDrawPos.X - MouseTilePos.X) % selection.Width == 0 &&
			      (LastDrawPos.Y - MouseTilePos.Y) % selection.Height == 0
			    )
			   )
			  ) {
				LastDrawPos = MouseTilePos;

				EditorAction(Modifiers);	//Call editor-specific part of code

			}
			if(selecting)
				UpdateSelection();
			Redraw();
		}
	}

	protected virtual void UpdateSelection() {
		if (MouseTilePos.X < SelectStartPos.X) {
			if (MouseTilePos.X < 0)
				SelectionP1.X = 0;
			else
				SelectionP1.X = MouseTilePos.X;
			SelectionP2.X = SelectStartPos.X;
		} else {
			SelectionP1.X = SelectStartPos.X;
			if (MouseTilePos.X >= application.CurrentTilemap.Width)
				SelectionP2.X = (int) application.CurrentTilemap.Width - 1;
			else
				SelectionP2.X = MouseTilePos.X;
		}

		if (MouseTilePos.Y < SelectStartPos.Y) {
			if (MouseTilePos.Y < 0)
				SelectionP1.Y = 0;
			else
				SelectionP1.Y = MouseTilePos.Y;
			SelectionP2.Y = SelectStartPos.Y;
		} else {
			SelectionP1.Y = SelectStartPos.Y;
			if (MouseTilePos.Y >= application.CurrentTilemap.Height)
				SelectionP2.Y = (int) application.CurrentTilemap.Height - 1;
			else
				SelectionP2.Y = MouseTilePos.Y;
		}

		uint NewWidth = (uint) (SelectionP2.X - SelectionP1.X) + 1;
		uint NewHeight = (uint) (SelectionP2.Y - SelectionP1.Y) + 1;
		selection.Resize(NewWidth, NewHeight, 0);

			for(uint y = 0; y < NewHeight; y++) {
				for(uint x = 0; x < NewWidth; ++x) {
					selection[x, y]
						= application.CurrentTilemap[(uint) SelectionP1.X + x,
									     (uint) SelectionP1.Y + y];
				}
			}
	}

	private void OnSelectionChanged() {
		Redraw();
	}

	protected void FireRedraw() {
		Redraw();
	}
}
