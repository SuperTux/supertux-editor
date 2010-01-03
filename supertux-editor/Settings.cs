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
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using Gtk;
using Gdk;
using Glade;

public sealed class Settings {
	public string LastDirectoryName;
	public string LastBrushDir = "/usr/share/games/supertux-editor/brushes";
	public string SupertuxExe = "/usr/games/supertux2";
	public string SupertuxData = "/usr/share/games/supertux2";
	public bool   ToolboxOnRight = false;
	public List<string> RecentDocuments = new List<string>();	//Added default value to prevent null-pointer-exceptions

	public static Settings Instance;
	private static XmlSerializer settingsSerializer = new XmlSerializer(typeof(Settings));
	private static string SettingsFile;

	static Settings() {
		String SettingsPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Constants.PACKAGE_NAME);
		SettingsFile = System.IO.Path.Combine(SettingsPath, "settings.xml");

		StreamReader reader = null;
		try {
			LogManager.Log(LogLevel.Info, "Using configfile: " + SettingsFile);
			reader = new StreamReader(SettingsFile);
			Instance = (Settings) settingsSerializer.Deserialize(reader);
		} catch(Exception e) {
			LogManager.Log(LogLevel.Error, "Couldn't load configfile: " + e.Message);
			LogManager.Log(LogLevel.Info, "Creating new config from scratch");
			Instance = new Settings();
		} finally {
			if(reader != null)
				reader.Close();
		}

		if(!Instance.SupertuxData.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString())) {
			Instance.SupertuxData += System.IO.Path.DirectorySeparatorChar;
		}

		LogManager.Log(LogLevel.Info, "Supertux is run as: " + Instance.SupertuxExe);
		LogManager.Log(LogLevel.Info, "Data files are in: " + Instance.SupertuxData);

		// If data path does not exist, prompt user to change it before we try continue initializing
		if (!new DirectoryInfo(System.IO.Path.GetDirectoryName(Instance.SupertuxData)).Exists) {
			LogManager.Log(LogLevel.Error, "Data path does not exist.");
			MessageDialog md = new MessageDialog(null, DialogFlags.DestroyWithParent, MessageType.Warning, ButtonsType.None, "The current data path, \"" + Instance.SupertuxData + "\", does not exist." + Environment.NewLine + Environment.NewLine + "Edit the settings to set a valid data path.");
			md.AddButton(Gtk.Stock.No, ResponseType.No);
			md.AddButton(Gtk.Stock.Edit, ResponseType.Yes);
			if (md.Run() == (int)ResponseType.Yes) {
				new SettingsDialog(true);
			}
			md.Destroy();
		}

		Resources.ResourceManager.Instance = new Resources.DefaultResourceManager(Instance.SupertuxData + "/");
	}

	public Settings() {
		// Get default values from the register on Windows.
		// Values read here will be overwritten by the settings file if it exist.
#if WINDOWS
		string SupertuxDir = (string)Microsoft.Win32.Registry.GetValue(
			"HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{4BEF4147-E17A-4848-BDC4-60A0AAC70F2A}_is1",
			"Inno Setup: App Path", null);
		if (SupertuxDir == null)
			SupertuxDir = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\SuperTux-0.3";
		SupertuxExe = SupertuxDir + "\\SuperTux2.exe";
		SupertuxData = SupertuxDir + "\\data\\";
#endif
	}

	public void Save() {
		StreamWriter writer = null;
		try {
			string dir = System.IO.Path.GetDirectoryName(SettingsFile);
			DirectoryInfo d = new DirectoryInfo(dir);
			if(!d.Exists) {
				LogManager.Log(LogLevel.Info, "Settings path \"" + dir + "\" does not exist. Trying to create.");
				d.Create();
			}

			writer = new StreamWriter(SettingsFile);
			settingsSerializer.Serialize(writer, Instance);
		} catch(Exception e) {
			LogManager.Log(LogLevel.Error, "Couldn't write configfile: " + e.Message);
		} finally {
			if(writer != null)
				writer.Close();
		}
	}

	public void addToRecentDocuments(string fileName) {
		RecentDocuments.RemoveAll(delegate(string s) { return (s == fileName); });
		RecentDocuments.Add(fileName);
		while (RecentDocuments.Count > 8) RecentDocuments.RemoveAt(0);
	}

}

/* EOF */
