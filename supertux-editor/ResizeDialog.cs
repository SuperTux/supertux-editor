//  $Id$
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
