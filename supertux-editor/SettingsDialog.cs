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

	/// <summary>
	/// Used to show message about the editor needs to be restarted.
	/// </summary>
	private bool Changed = false;

	public SettingsDialog()
	{
		Glade.XML gxml = new Glade.XML("editor.glade", "settingsDialog");
		gxml.Autoconnect(this);

		if(settingsDialog == null || dataDirChooser == null || exeChooser == null)
			throw new Exception("Couldn't load settings Dialog");

		dataDirChooser.SelectFilename(Settings.Instance.SupertuxData);
		exeChooser.SelectFilename(Settings.Instance.SupertuxExe);

		dataDirChooser.SelectionChanged += OnDataDirChanged;
		exeChooser.SelectionChanged += OnExeChanged;

		settingsDialog.ShowAll();
	}

	protected void OnDataDirChanged(object o, EventArgs args)
	{
		if (Settings.Instance.SupertuxData.TrimEnd(System.IO.Path.DirectorySeparatorChar) != dataDirChooser.Filename)
			Changed = true;
		Settings.Instance.SupertuxData = dataDirChooser.Filename;
		Settings.Instance.Save();
	}

	protected void OnExeChanged(object o, EventArgs args)
	{
		if (exeChooser.Filename == null)
			return;
		if (Settings.Instance.SupertuxExe != exeChooser.Filename)
			Changed = true;
		Settings.Instance.SupertuxExe = exeChooser.Filename;
		Settings.Instance.Save();
	}

	protected void OnClose(object o, EventArgs args)
	{
		if (Changed) {
			MessageDialog md = new MessageDialog(settingsDialog,
			                                     DialogFlags.DestroyWithParent,
			                                     MessageType.Warning,
			                                     ButtonsType.Ok,
			                                     "You have to restart the editor before the changes take effect.");
			md.Run();
			md.Destroy();
		}

		settingsDialog.Hide();
	}
}
