using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

public class Settings {
	public uint ScreenWidth		= 800;
	public uint ScreenHeight	= 600;

	public bool UseFullscreen	= true;
	public bool ShowFps			= false;
	public bool EnableSound		= true;
	public bool EnableMusic		= true;
	public bool EnableCheats	= true;

	public static Settings Instance;
	private static XmlSerializer settingsSerializer = new XmlSerializer(typeof(Settings));
	private static string SettingsFile;

	static Settings() {
		SettingsFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		SettingsFile += "/" + Constants.PACKAGE_NAME + "/settings.xml";

		StreamReader reader = null;
		try {
			reader = new StreamReader(SettingsFile);
			Instance = (Settings) settingsSerializer.Deserialize(reader);
		} catch(Exception e) {
			Console.WriteLine("Couldn't load configfile: " + e.Message);
			Instance = new Settings();
		} finally {
			if(reader != null)
				reader.Close();
		}
	}

	public Settings() {
	}

	public void Save() {
		StreamWriter writer = null;
		try {
			string dir = Path.GetDirectoryName(SettingsFile);
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

