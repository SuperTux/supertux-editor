//  $Id$
using System;
using Gtk;
using Gdk;
using Glade;

public class ResizeDialog
{
	[Glade.Widget]
	private Dialog resizeDialog = null;
	[Glade.Widget]
	private Entry WidthEntry = null;
	[Glade.Widget]
	private Entry HeightEntry = null;

	private Sector sector;

	private IEditorApplication application;

	public ResizeDialog(Sector sector, IEditorApplication app)
	{
		this.sector = sector;
		application = app;
		Glade.XML gxml = new Glade.XML("editor.glade", "resizeDialog");
		gxml.Autoconnect(this);

		if(resizeDialog == null || WidthEntry == null || HeightEntry == null)
			throw new Exception("Couldn't load resize Dialog");

		uint width = 0;
		uint height = 0;
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
			application.TakeUndoSnapshot( "Sector resized to " + newWidth + "x" + newHeight);
			foreach(Tilemap tilemap in sector.GetObjects(typeof(Tilemap))) {
				tilemap.Resize(newWidth, newHeight, 0);
			}
			sector.EmitSizeChanged();
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
