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
using System.Diagnostics;
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

	public string SupertuxExe
	{
		get
		{
			return supertuxexe;
		}
		set
		{
			supertuxexe = value;
			supertuxdatadir_cached = false;
		}
		
	}
	private string supertuxexe = "supertux2";

	public string SupertuxData
	{
		get
		{
			if (!supertuxdatadir_cached)
			{
				supertuxdatadir = GuessDataDir();
				supertuxdatadir_cached = true;
			}
			return supertuxdatadir;
		}
	}
	private string supertuxdatadir;
	private bool supertuxdatadir_cached = false;

	public bool ToolboxOnRight = false;
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

		LogManager.Log(LogLevel.Info, "Supertux is run as: " + Instance.SupertuxExe);
		if (Instance.SupertuxData != null)
			LogManager.Log(LogLevel.Info, "Data files are in: " + Instance.SupertuxData);
		else
			LogManager.Log(LogLevel.Info, "Unable to find data files when querying supertux.");

		// If data path does not exist, prompt user to change it before we try continue initializing
		if (Instance.SupertuxData == null
		    || !new DirectoryInfo(System.IO.Path.GetDirectoryName(Instance.SupertuxData)).Exists) {
			LogManager.Log(LogLevel.Error, "Data path does not exist.");

			String bad_data_path_msg;
			if (Instance.SupertuxData == null)
				bad_data_path_msg = "The data path could not be calculated." + Environment.NewLine
					+ Environment.NewLine
					+ "You must install a newer version of Supertux to use this version of the editor or specify the correct path to the supertux2 binary.";
			else
				bad_data_path_msg = "The current data path, `"
					+ Instance.SupertuxData + "', does not exist." + Environment.NewLine
					+ Environment.NewLine
					+ "This means that your supertux installation (`" + Instance.SupertuxExe + "') is corrupted or incomplete.";

			MessageDialog md = new MessageDialog(null, DialogFlags.DestroyWithParent, MessageType.Warning, ButtonsType.None, bad_data_path_msg);
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
		/* Move the following statement into GuessDataDir() if it's needed */
		/* SupertuxData = SupertuxDir + "\\data\\"; */
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

	/**
	 * \brief
	 *   Utility for the SupertuxData property.
	 * \return
	 *   The guessed datadir.
	 */
	private String GuessDataDir()
	{
		if (String.IsNullOrEmpty(SupertuxExe))
			return null;

		/*
		 * If the SupertuxExe has directory separators in it, we can
		 * check if it exists before inefficiently trying to execute it.
		 */
		if ((SupertuxExe.IndexOf(System.IO.Path.DirectorySeparatorChar) > -1
		     || SupertuxExe.IndexOf(System.IO.Path.AltDirectorySeparatorChar) > -1)
		    && !File.Exists(SupertuxExe))
			return null;

		/* Query supertux for its datadirs. */
		Process supertux_process = new Process();
		supertux_process.StartInfo.FileName = SupertuxExe;
		String working_dir;
		try
		{
			working_dir = System.IO.Path.GetDirectoryName(Settings.Instance.SupertuxExe);
			if (working_dir != null)
				supertux_process.StartInfo.WorkingDirectory = working_dir;
		} catch (Exception) { working_dir = ""; }
		supertux_process.StartInfo.Arguments = "--print-datadir";
		supertux_process.StartInfo.UseShellExecute = false;
		supertux_process.StartInfo.RedirectStandardOutput = true;
		/* SDL-1.2 likes to mess with supertux's stdout unless we ask SDL not to: */
		supertux_process.StartInfo.EnvironmentVariables.Add("SDL_STDIO_REDIRECT", "0");
		try
		{
			supertux_process.Start();
			String supertux_datadir = null;
			String supertux_datadir_best = null;
			while (!String.IsNullOrEmpty(supertux_datadir = supertux_process.StandardOutput.ReadLine())) {
				if(System.IO.Path.IsPathRooted(supertux_datadir)) {
					supertux_datadir_best = supertux_datadir;
				}
			}

			if (supertux_datadir_best != null && Directory.Exists(supertux_datadir_best))
				return supertux_datadir_best;
		}
		catch (Exception)
		{
		}

		return null;
	}
}

/* EOF */
