//  $Id$
using System;
using Gtk;
using Gdk;
using Glade;

public class SettingsDialog
{
	[Glade.Widget]
	private Dialog settingsDialog;
	[Glade.Widget]
	private Entry entryDataDir;
	[Glade.Widget]
	private Entry entryExe;

	/// <summary>
	/// Used to show message about the editor needs to be restarted.
	/// </summary>
	private bool Changed = false;

	public SettingsDialog(bool modal)
	{
		Glade.XML gxml = new Glade.XML("editor.glade", "settingsDialog");
		gxml.Autoconnect(this);

		if(settingsDialog == null || entryDataDir == null || entryExe == null)
			throw new Exception("Couldn't load settings Dialog");

		entryDataDir.Text = Settings.Instance.SupertuxData;
		entryExe.Text = Settings.Instance.SupertuxExe;

		Changed = false;

		if (!modal) {
			settingsDialog.ShowAll();
		} else {
			settingsDialog.Run();
			settingsDialog.Destroy();
		}
	}

	protected void OnEntryDataDirChanged(object o, EventArgs args)
	{
		if (Settings.Instance.SupertuxData.TrimEnd(System.IO.Path.DirectorySeparatorChar) != entryDataDir.Text)
			Changed = true;
		Settings.Instance.SupertuxData = entryDataDir.Text;
		Settings.Instance.Save();
	}

	protected void OnEntryExeChanged(object o, EventArgs args)
	{
		if (entryExe.Text == null)
			return;
		if (Settings.Instance.SupertuxExe != entryExe.Text)
			Changed = true;
		Settings.Instance.SupertuxExe = entryExe.Text;
		Settings.Instance.Save();
	}

	protected void OnBtnDataDirBrowseClicked(object o, EventArgs args)
	{
		FileChooserDialog fileChooser = new FileChooserDialog("Locate SuperTux Data Directory", settingsDialog, FileChooserAction.SelectFolder, new object[] {});
		fileChooser.AddButton(Gtk.Stock.Cancel, Gtk.ResponseType.Cancel);
		fileChooser.AddButton(Gtk.Stock.Ok, Gtk.ResponseType.Ok);
		fileChooser.DefaultResponse = Gtk.ResponseType.Ok;
		if (fileChooser.Run() == (int)ResponseType.Ok) {
			entryDataDir.Text = fileChooser.Filename;
		}
		fileChooser.Destroy();
	}

	protected void OnBtnExeBrowseClicked(object o, EventArgs args)
	{
		FileChooserDialog fileChooser = new FileChooserDialog("Locate SuperTux Executable", settingsDialog, FileChooserAction.Open, new object[] {});
		fileChooser.AddButton(Gtk.Stock.Cancel, Gtk.ResponseType.Cancel);
		fileChooser.AddButton(Gtk.Stock.Ok, Gtk.ResponseType.Ok);
		fileChooser.DefaultResponse = Gtk.ResponseType.Ok;
		if (fileChooser.Run() == (int)ResponseType.Ok) {
			entryExe.Text = fileChooser.Filename;
		}
		fileChooser.Destroy();
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
