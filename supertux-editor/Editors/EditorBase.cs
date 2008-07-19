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

// TODO: More things should be moved into this class.
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

	protected Tilemap Tilemap;
	protected Tileset Tileset;

	internal TileBlock.StateData tilemapBackup; // saved OnMouseButtonPress

	protected bool UpdateMouseTilePos(Vector MousePos) {
		FieldPos NewMouseTilePos = new FieldPos((int) (MousePos.X) / 32, (int) (MousePos.Y) / 32);
		if (NewMouseTilePos != MouseTilePos) {
			MouseTilePos = NewMouseTilePos;
			return true;
		}

		return false;
	}

	public virtual void Draw(Gdk.Rectangle cliprect) {
		if (!selecting) {
			gl.Color4f(1, 1, 1, 0.7f);
			Vector pos = new Vector(MouseTilePos.X * 32f, MouseTilePos.Y * 32f);
			selection.Draw(pos, Tileset);
			gl.Color4f(1, 1, 1, 1);
		}
		if (selecting) {
			float left = SelectionP1.X * 32f;
			float top = SelectionP1.Y * 32f;
			float right = SelectionP2.X * 32f + 32f;
			float bottom = SelectionP2.Y * 32f + 32f;

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

	protected TileEditorBase(IEditorApplication application, Tilemap Tilemap, Tileset Tileset, Selection selection) {
		this.application = application;
		this.Tilemap = Tilemap;
		this.Tileset = Tileset;
		this.selection = selection;
		application.TilemapChanged += OnTilemapChanged;
		selection.Changed += OnSelectionChanged;
	}

	public void Dispose()
	{
		selection.Changed -= OnSelectionChanged;
	}

	public virtual void OnTilemapChanged(Tilemap newTilemap) {
		Tilemap = newTilemap;
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
			if (MouseTilePos.X >= Tilemap.Width)
				SelectionP2.X = (int) Tilemap.Width - 1;
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
			if (MouseTilePos.Y >= Tilemap.Height)
				SelectionP2.Y = (int) Tilemap.Height - 1;
			else
				SelectionP2.Y = MouseTilePos.Y;
		}

		uint NewWidth = (uint) (SelectionP2.X - SelectionP1.X) + 1;
		uint NewHeight = (uint) (SelectionP2.Y - SelectionP1.Y) + 1;
		selection.Resize(NewWidth, NewHeight, 0);

			for(uint y = 0; y < NewHeight; y++) {
				for(uint x = 0; x < NewWidth; ++x) {
					selection[x, y]
						= Tilemap[(uint) SelectionP1.X + x,
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
