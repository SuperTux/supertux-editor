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

public static class LevelUtil
{
	/// <summary>
	/// Initializes a new level with some default values.
	/// </summary>
	/// <returns>The created <see cref="Level"/> object.</returns>
	public static Level CreateLevel()
	{
		Level level = new Level();
		level.Name = "Unnamed";
		level.Author = Settings.Instance.Name;
 		level.Contact = Settings.Instance.ContactInfo;
 		level.License = Settings.Instance.License;

		level.Sectors.Add(CreateSector("main"));

		return level;
	}

	/// <summary>
	/// Initializes a new sector with some default values.
	/// </summary>
	/// <param name="Name">Name of new sector.</param>
	/// <returns>The created <see cref="Sector"/> object.</returns>
	public static Sector CreateSector(string Name)
	{
		Sector sector = new Sector();
		sector.Name = Name;

		Tilemap tilemap = new Tilemap();
		tilemap.Resize(100, 35, 0);
		tilemap.Layer = -100;
		tilemap.Name = "Background";
		sector.Add(tilemap, true);

		tilemap = new Tilemap();
		tilemap.Resize(100, 35, 0);
		tilemap.Layer = 0;
		tilemap.Solid = true;
		tilemap.Name = "Interactive";
		sector.Add(tilemap, true);

		tilemap = new Tilemap();
		tilemap.Resize(100, 35, 0);
		tilemap.Layer = 100;
		tilemap.Name = "Foreground";
		sector.Add(tilemap, true);

		SpawnPoint spawnpoint = new SpawnPoint();
		spawnpoint.X = 96;
		spawnpoint.Y = 96;
		spawnpoint.EntityName = "main";
		sector.Add(spawnpoint, true);

		Camera camera = new Camera();
		sector.Add(camera, true);

		sector.FinishRead(); //let sector detect it's dimensions

		return sector;
	}
}

/* EOF */
