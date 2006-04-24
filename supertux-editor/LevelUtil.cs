using System;

public class LevelUtil
{
	public static Level CreateLevel()
	{
		Level level = new Level();
		level.Name = "Unnamed";
		level.Author = Environment.UserName;

		level.Sectors.Add(CreateSector("main"));

		return level;
	}
	
	public static Sector CreateSector(string Name)
	{
		Sector sector = new Sector();
		sector.Name = "main";

		Tilemap tilemap = new Tilemap();
		tilemap.Resize(100, 35, 0);
		tilemap.ZPos = -100;
		sector.Add(tilemap);

		tilemap = new Tilemap();
		tilemap.Resize(100, 35, 0);
		tilemap.ZPos = 0;
		tilemap.Solid = true;
		sector.Add(tilemap);

		tilemap = new Tilemap();
		tilemap.Resize(100, 35, 0);
		tilemap.ZPos = 100;
		sector.Add(tilemap);

		SpawnPoint spawnpoint = new SpawnPoint();
		spawnpoint.X = 96;
		spawnpoint.Y = 96;
		spawnpoint.Name = "main";
		// hack for now...
		sector.Add(spawnpoint);

		Camera camera = new Camera();
		sector.Add(camera);

		Background background = new Background();
		background.Image = "images/background/arctis.jpg";
		sector.Add(background);

		return sector;
	}
}

