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

using System;
using Gtk;

public class TilegroupSelector : ComboBox
{
	private Level level;
	private TileListWidget tileList;

	public TilegroupSelector(Application application, TileListWidget tileList)
	{
		this.tileList = tileList;
		application.LevelChanged += OnLevelChanged;

		CellRendererText renderer = new CellRendererText();
		PackStart(renderer, false);
		SetCellDataFunc(renderer, TextDataFunc);

		Changed += OnTileGroupChosen;
	}

	private void OnLevelChanged(Level level)
	{
		if(this.level != null)
			this.level.TilesetChanged -= OnTilesetChanged;
		if(level != null)
			level.TilesetChanged += OnTilesetChanged;
		this.level = level;

		OnTilesetChanged(level);
	}

	private void OnTilesetChanged(Level level)
	{
		Tileset tileset = level.Tileset;

		TreeStore store = new TreeStore(typeof(Tilegroup));
		foreach(Tilegroup group in tileset.Tilegroups.Values) {
			store.AppendValues(group);
		}
		Model = store;
	}

	private void TextDataFunc(CellLayout cell_layout, CellRenderer renderer, TreeModel model, TreeIter iter)
	{
		CellRendererText textRenderer = (CellRendererText) renderer;
		Tilegroup group = (Tilegroup) Model.GetValue(iter, 0);

		textRenderer.Text = group.Name;
	}

	private void OnTileGroupChosen(object o, EventArgs args)
	{
		TreeIter iter;

		if (!GetActiveIter (out iter))
			return;

		Tilegroup group = (Tilegroup) Model.GetValue(iter, 0);
		tileList.ChangeTilegroup(group);
	}
}

/* EOF */
