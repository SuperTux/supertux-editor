using System;
using System.Collections.Generic;
using LispReader;

[LispRoot("tilegroup")]
public class Tilegroup
{
	[LispChild("name")]
	public string Name;
	[LispChild("tiles")]
	public List<int> Tiles = new List<int>();

	public Tilegroup()
	{
	}
}
