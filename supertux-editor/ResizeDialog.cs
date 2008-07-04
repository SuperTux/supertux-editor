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
	private uint width = 0;
	private uint height = 0;

	public ResizeDialog(Sector sector)
	{
		this.sector = sector;
		Glade.XML gxml = new Glade.XML("editor.glade", "resizeDialog");
		gxml.Autoconnect(this);

		if(resizeDialog == null || WidthEntry == null || HeightEntry == null)
			throw new Exception("Couldn't load resize Dialog");

		foreach(Tilemap tilemap in sector.GetObjects(typeof(Tilemap))) {
			if(tilemap.Width > width)
				width = tilemap.Width;
			if(tilemap.Height > height)
				height = tilemap.Height;
		}
		WidthEntry.Text = width.ToString();
		HeightEntry.Text = height.ToString();
		resizeDialog.Icon = EditorStock.WindowIcon;
		resizeDialog.ShowAll();
	}

	protected void OnOk(object o, EventArgs args)
	{
		try {
			uint newWidth = UInt32.Parse(WidthEntry.Text);
			uint newHeight = UInt32.Parse(HeightEntry.Text);
			//application.TakeUndoSnapshot( "Sector resized to " + newWidth + "x" + newHeight);
			SectorSizeChangeCommand command = new SectorSizeChangeCommand(
				"Sector resized to " + newWidth + "x" + newHeight,
				sector,
				newWidth,
				newHeight,
				width,
				height);
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
