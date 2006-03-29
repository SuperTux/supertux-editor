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
		tilemap.LayerName = "background";
		sector.GameObjects.Add(tilemap);

		tilemap = new Tilemap();
		tilemap.Resize(100, 35, 0);
		tilemap.LayerName = "interactive";
		tilemap.Solid = true;
		sector.GameObjects.Add(tilemap);

		tilemap = new Tilemap();
		tilemap.Resize(100, 35, 0);
		tilemap.LayerName = "foreground";
		sector.GameObjects.Add(tilemap);

		SpawnPoint spawnpoint = new SpawnPoint();
		spawnpoint.X = 96;
		spawnpoint.Y = 96;
		spawnpoint.Name = "main";
		// hack for now...
		spawnpoint.FinishRead();
		sector.GameObjects.Add(spawnpoint);

		Camera camera = new Camera();
		sector.GameObjects.Add(camera);

		Background background = new Background();
		background.Image = "images/background/arctis.jpg";
		sector.GameObjects.Add(background);

		return sector;
	}
}

