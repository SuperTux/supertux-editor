using System;
using Gtk;
using Glade;

public class ResizeDialog
{
	[Glade.Widget]
	private Dialog resizeDialog;
	[Glade.Widget]
	private Entry WidthEntry;
	[Glade.Widget]
	private Entry HeightEntry;
	
	private Sector sector;
	
	public ResizeDialog(Sector sector)
	{
		this.sector = sector;
		
		Glade.XML gxml = new Glade.XML("editor.glade", "resizeDialog");
		gxml.Autoconnect(this);
		
		if(resizeDialog == null || WidthEntry == null || HeightEntry == null)
			throw new Exception("Couldn't load resize Dialog");
		
		resizeDialog.ShowAll();
	}
	
	protected void OnOk(object o, EventArgs args)
	{
		try {
			uint newWidth = UInt32.Parse(WidthEntry.Text);
			uint newHeight = UInt32.Parse(HeightEntry.Text);
			foreach(Tilemap tilemap in sector.GetObjects(typeof(Tilemap))) {
				tilemap.Resize(newWidth, newHeight, 0);				
			}
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
