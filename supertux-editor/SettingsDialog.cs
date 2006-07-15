using System;
using Gtk;
using Gdk;
using Glade;

public class SettingsDialog
{
	[Glade.Widget]
	private Dialog settingsDialog;
	[Glade.Widget]
	private FileChooserButton dataDirChooser;
	[Glade.Widget]
	private FileChooserButton exeChooser;
		
	public SettingsDialog()
	{
		Glade.XML gxml = new Glade.XML("editor.glade", "settingsDialog");
		gxml.Autoconnect(this);
		
		if(settingsDialog == null || dataDirChooser == null || exeChooser == null)
			throw new Exception("Couldn't load resize Dialog");
		
		dataDirChooser.SelectFilename(Settings.Instance.SupertuxData);
		exeChooser.SelectFilename(Settings.Instance.SupertuxExe);
		
		dataDirChooser.SelectionChanged += OnDataDirChanged;
		exeChooser.SelectionChanged += OnExeChanged;
				
		settingsDialog.ShowAll();
	}
	
	protected void OnDataDirChanged(object o, EventArgs args)
	{
		Settings.Instance.SupertuxData = dataDirChooser.Filename;
		Settings.Instance.Save();
	}
	
	protected void OnExeChanged(object o, EventArgs args)
	{
		Settings.Instance.SupertuxExe = exeChooser.Filename;
		Settings.Instance.Save();
	}
	
	protected void OnClose(object o, EventArgs args)
	{
		settingsDialog.Hide();
	}
}
