//  $Id$
using System;

public static class LevelUtil
{
	/// <summary>
	/// Initialises a new level with some default values.
	/// </summary>
	/// <returns>The created <see cref="Level"/> object.</returns>
	public static Level CreateLevel()
	{
		Level level = new Level();
		level.Name = "Unnamed";
		level.Author = Environment.UserName;

		level.Sectors.Add(CreateSector("main"));

		return level;
	}

	/// <summary>
	/// Initialises a new sector with some default values.
	/// </summary>
	/// <param name="Name">Name of new sector.</param>
	/// <returns>The created <see cref="Sector"/> object.</returns>
	public static Sector CreateSector(string Name)
	{
		Sector sector = new Sector();
		sector.Name = Name;

		Tilemap tilemap = new Tilemap();
		tilemap.Resize(100, 35, 0);
		tilemap.ZPos = -100;
		tilemap.Name = "Background";
		sector.Add(tilemap);

		tilemap = new Tilemap();
		tilemap.Resize(100, 35, 0);
		tilemap.ZPos = 0;
		tilemap.Solid = true;
		tilemap.Name = "Interactive";
		sector.Add(tilemap);

		tilemap = new Tilemap();
		tilemap.Resize(100, 35, 0);
		tilemap.ZPos = 100;
		tilemap.Name = "Foreground";
		sector.Add(tilemap);

		SpawnPoint spawnpoint = new SpawnPoint();
		spawnpoint.X = 96;
		spawnpoint.Y = 96;
		spawnpoint.Name = "main";
		sector.Add(spawnpoint);

		Camera camera = new Camera();
		sector.Add(camera);

		Background background = new Background();
		background.Image = "images/background/BlueRock_Forest/blue-middle.jpg";
		background.ImageTop = "images/background/BlueRock_Forest/blue-top.jpg";
		background.ImageBottom = "images/background/BlueRock_Forest/blue-bottom.jpg";
		sector.Add(background);

		return sector;
	}
}
