using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

public class Settings {
	public string LastDirectoryName;
	public string SupertuxExe = "/usr/local/bin/supertux";
    public string SupertuxData = "/usr/local/share/supertux";

	public static Settings Instance;
	private static XmlSerializer settingsSerializer = new XmlSerializer(typeof(Settings));
	private static string SettingsFile;

	static Settings() {
		SettingsFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		SettingsFile += "/" + Constants.PACKAGE_NAME + "/settings.xml";

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

