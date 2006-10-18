//  $Id$
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

public sealed class Settings {
	public string LastDirectoryName;
	public string LastBrushDir;
	public string SupertuxExe = "/usr/local/bin/supertux";
	public string SupertuxData = "/usr/local/share/supertux";

	public static Settings Instance;
	private static XmlSerializer settingsSerializer = new XmlSerializer(typeof(Settings));
	private static string SettingsFile;

	static Settings() {
		String SettingsPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Constants.PACKAGE_NAME);
		SettingsFile = System.IO.Path.Combine(SettingsPath, "settings.xml");

		StreamReader reader = null;
		try {
			Console.WriteLine("Using configfile: " + SettingsFile);
			reader = new StreamReader(SettingsFile);
			Instance = (Settings) settingsSerializer.Deserialize(reader);
		} catch(Exception e) {
			Console.WriteLine("Couldn't load configfile: " + e.Message);
			Instance = new Settings();
		} finally {
			if(reader != null)
				reader.Close();
		}

		if(!Instance.SupertuxData.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
			Instance.SupertuxData += System.IO.Path.DirectorySeparatorChar;
			Console.WriteLine("Supertux is run as: " + Instance.SupertuxExe);
			Console.WriteLine("Data files are in: " + Instance.SupertuxData);
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
		SupertuxExe = SupertuxDir + "\\SuperTux.exe";
		SupertuxData = SupertuxDir + "\\data\\";
#endif
	}

	public void Save() {
		StreamWriter writer = null;
		try {
			string dir = System.IO.Path.GetDirectoryName(SettingsFile);
			DirectoryInfo d = new DirectoryInfo(dir);
			if(!d.Exists)
				d.Create();

			writer = new StreamWriter(SettingsFile);
			settingsSerializer.Serialize(writer, Instance);
		} catch(Exception e) {
			Console.WriteLine("Couldn't write configfile: " + e.Message);
		} finally {
			if(writer != null)
				writer.Close();
		}
	}
}
