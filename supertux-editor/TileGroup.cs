using System;
using System.Collections.Generic;
using LispReader;

[LispRoot("tilegroup")]
public class TileGroup
{	
	[LispChild("name")]
	public string Name;
	[LispChild("tiles")]
	public List<uint> tiles = new List<uint>();
	
	public TileGroup()
	{
	}
}
