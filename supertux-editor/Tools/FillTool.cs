//  SuperTux Editor
//  Copyright (C) 2006 Matthias Braun <matze@braunis.de>
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  Copyright (C) 2006 Matthias Braun <matze@braunis.de>

using DataStructures;
using Gdk;

public sealed class FillTool : TileToolBase, IEditor {

	public FillTool(IEditorApplication application, Tileset Tileset, Selection selection)
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

/* EOF */
