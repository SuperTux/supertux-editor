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
	private Entry entryExe = null;
	[Glade.Widget]
	private RadioButton rbToolsLeft = null;
	[Glade.Widget]
	private RadioButton rbToolsRight = null;
	
	[Glade.Widget]
    private Entry entryName;
    [Glade.Widget]
    private Entry entryContact;
    [Glade.Widget]
    private Entry entryLicense;
 
	
	/// <summary>
	/// Used to show message about the editor needs to be restarted.
	/// </summary>
	private bool SidebarChanged;

	public SettingsDialog(bool modal)
	{
		Glade.XML gxml = new Glade.XML("editor.glade", "settingsDialog");
		gxml.Autoconnect(this);

		if(settingsDialog == null || entryExe == null)
			throw new Exception("Couldn't load settings Dialog");

		entryExe.Text = Settings.Instance.SupertuxExe;
		rbToolsLeft.Active = !Settings.Instance.ToolboxOnRight;
		rbToolsRight.Active = Settings.Instance.ToolboxOnRight;
		
		entryName.Text = Settings.Instance.Name;
        entryContact.Text = Settings.Instance.ContactInfo;
        entryLicense.Text = Settings.Instance.License;
		
		SidebarChanged = false;
		settingsDialog.Icon = EditorStock.WindowIcon;
		if (!modal) {
			settingsDialog.ShowAll();
		} else {
			settingsDialog.Run();
			settingsDialog.Destroy();
		}
	}

	protected void OnEntryExeChanged(object o, EventArgs args)
	{
		if (entryExe.Text == null)
			return;
		Settings.Instance.SupertuxExe = entryExe.Text;
		Settings.Instance.Save();
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
			SidebarChanged = true;
		Settings.Instance.ToolboxOnRight = rbToolsRight.Active;
		Settings.Instance.Save();
	}
	protected void OnNameChanged(object o, EventArgs args)
    {
        if (entryName.Text != null)
            Settings.Instance.Name = entryName.Text;
        Settings.Instance.Save();
    }
    
    protected void OnContactChanged(object o, EventArgs args)
    {
        if (entryContact.Text != null)
            Settings.Instance.ContactInfo = entryContact.Text;
        Settings.Instance.Save();
    }
    
    protected void OnLicenseChanged(object o, EventArgs args)
    {
        if (entryLicense.Text != null)
            Settings.Instance.License = entryLicense.Text;
        Settings.Instance.Save();
    }
	protected void OnClose(object o, EventArgs args)
	{
		if (SidebarChanged) {
			MessageDialog md = new MessageDialog(settingsDialog,
			                                     DialogFlags.DestroyWithParent,
			                                     MessageType.Warning,
			                                     ButtonsType.Ok,
			                                     "You have to restart the editor before the changes take effect.");
			md.Run();
			md.Destroy();
		}

		/* Verify the SupertuxExe setting. */
		if (Settings.Instance.SupertuxData == null)
		{
			MessageDialog md = new MessageDialog(null, DialogFlags.DestroyWithParent, MessageType.Warning, ButtonsType.Ok,
							     "Your SuperTux data directory could not be determined." + Environment.NewLine
							     + Environment.NewLine
							     + "The following problems are possible:" + Environment.NewLine
							     + "1. You have an old SuperTux installation. Upgrade to a newer version of SuperTux." + Environment.NewLine
							     + "2. You have specified an invalid path to the supertux2 executable. Reopen the preferences dialog and set a correct executable." + Environment.NewLine
							     + "3. Your SuperTux installation is confused about where its data directory is. Fix your corrupted SuperTux installation.");
			md.Run();
			md.Destroy();
		}
		/* Replace the ResourceManager with one which has the new datadir */
		Resources.ResourceManager.Instance = new Resources.DefaultResourceManager(Settings.Instance.SupertuxData + "/");

		settingsDialog.Hide();
	}
}

/* EOF */
