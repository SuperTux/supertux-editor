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
using Gdk;
using Glade;
using Undo;

public class ResizeDialog
{
	[Glade.Widget]
	private Dialog resizeDialog = null;
	[Glade.Widget]
	private Entry WidthEntry = null;
	[Glade.Widget]
	private Entry HeightEntry = null;

	private Sector sector;
	private Tilemap tilemap;
	private string undoTitleBase;

	public ResizeDialog(Sector sector, Tilemap tilemap)
	{
		this.sector = sector;
		this.tilemap = tilemap;
		Glade.XML gxml = new Glade.XML("editor.glade", "resizeDialog");
		gxml.Autoconnect(this);

		if(resizeDialog == null || WidthEntry == null || HeightEntry == null)
			throw new Exception("Couldn't load resize Dialog");

		if (tilemap == null) {
			WidthEntry.Text = sector.Width.ToString();
			HeightEntry.Text = sector.Height.ToString();
			undoTitleBase = "Sector \"" + sector.Name + "\"";
		} else {
			WidthEntry.Text = tilemap.Width.ToString();
			HeightEntry.Text = tilemap.Height.ToString();
			undoTitleBase = "Tilemap \"" + tilemap.Name + "\"";
		}
		resizeDialog.Title += " " + undoTitleBase;
		resizeDialog.Icon = EditorStock.WindowIcon;
		resizeDialog.ShowAll();
	}

	public ResizeDialog(Sector sector)
		:this (sector, null)
	{ }

	protected void OnOk(object o, EventArgs args)
	{
		try {
			uint newWidth = UInt32.Parse(WidthEntry.Text);
			uint newHeight = UInt32.Parse(HeightEntry.Text);
			//application.TakeUndoSnapshot( "Sector resized to " + newWidth + "x" + newHeight);
			SectorSizeChangeCommand command = new SectorSizeChangeCommand(
				undoTitleBase + " resized to " + newWidth + "x" + newHeight,
				sector,
				tilemap,
				newWidth,
				newHeight);
			command.Do();
			UndoManager.AddCommand(command);
		} catch(Exception e) {
			ErrorDialog.Exception(e);
		}

		resizeDialog.Hide();
	}

	protected void OnCancel(object o, EventArgs args)
	{
		resizeDialog.Hide();
	}
}

/* EOF */
