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

using Gdk;

public sealed class ReplaceTool : TileToolBase, IEditor
{
	public ReplaceTool(IEditorApplication application, Tileset Tileset, Selection selection)
		: base(application, Tileset, selection) 
	{
		ActionName = "Replace Tiles";
	}

	private void Replace(int oldId, int newId) 
	{
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

/* EOF */
