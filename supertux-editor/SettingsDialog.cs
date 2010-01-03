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

public class SettingsDialog
{
	[Glade.Widget]
	private Dialog settingsDialog = null;
	[Glade.Widget]
	private Entry entryDataDir = null;
	[Glade.Widget]
	private Entry entryExe = null;
	[Glade.Widget]
	private RadioButton rbToolsLeft = null;
	[Glade.Widget]
	private RadioButton rbToolsRight = null;

	/// <summary>
	/// Used to show message about the editor needs to be restarted.
	/// </summary>
	private bool Changed;

	public SettingsDialog(bool modal)
	{
		Glade.XML gxml = new Glade.XML("editor.glade", "settingsDialog");
		gxml.Autoconnect(this);

		if(settingsDialog == null || entryDataDir == null || entryExe == null)
			throw new Exception("Couldn't load settings Dialog");

		entryDataDir.Text = Settings.Instance.SupertuxData;
		entryExe.Text = Settings.Instance.SupertuxExe;
		rbToolsLeft.Active = !Settings.Instance.ToolboxOnRight;
		rbToolsRight.Active = Settings.Instance.ToolboxOnRight;

		Changed = false;
		settingsDialog.Icon = EditorStock.WindowIcon;
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

	protected void OnRbToolboxPosition(object o, EventArgs args)
	{
		if (Settings.Instance.ToolboxOnRight != rbToolsRight.Active)
			Changed = true;
		Settings.Instance.ToolboxOnRight = rbToolsRight.Active;
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

/* EOF */
