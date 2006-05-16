//  $Id$
// 
//  Copyright (C) 2006 Christoph Sommer <christoph.sommer@2006.expires.deltadevelopment.de>
//
//  This program is free software; you can redistribute it and/or
//  modify it under the terms of the GNU General Public License
//  as published by the Free Software Foundation; either version 2
//  of the License, or (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA
//  02111-1307, USA.

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using DataStructures;

/// <summary>
/// Smoothes Tilemaps by changing tiles to one of several stored valid patterns
/// </summary>
public class Brush 
{
	protected List<Field<int>> patterns;
	protected uint width;
	protected uint height;
	protected Tileset tileset;

	public Brush(uint width, uint height, List<Field<int>> patterns, Tileset tileset)
	{
		this.width = width;
		this.height = height;
		this.patterns = patterns;
		this.tileset = tileset;
	}

	public Brush(uint width, uint height, Tileset tileset)
	{
		this.width = width;
		this.height = height;
		this.patterns = new List<Field<int>>();
		this.tileset = tileset;
	}

	public int PatternCount {
		get {
			return patterns.Count;
		}
	}

	public uint Width {
		get {
			return width;
		}
	}

	public uint Height {
		get {
			return height;
		}
	}

	/// <summary>
	/// Add the tiles of the given tileBlock as a valid pattern
	/// </summary>
	/// <param name="tileBlock">tileBlock that specifies the pattern to add</param>
	public void LearnPattern(Field<int> tileBlock, int startX, int startY) {
		Field<int> tb = (Field<int>)tileBlock.CloneSubset(startX, startY, width, height);
		if (!patterns.Contains(tb)) patterns.Add(tb);
	}

	/// <summary>
	/// Add all tiles of the given tileBlock to the list of valid patterns
	/// </summary>
	/// <param name="tileBlock">tileBlock that specifies the patterns to add</param>
	public void LearnPatterns(Field<int> tileBlock) {
		for (int tx = 0; tx <= (int)tileBlock.Width - (int)width; tx++) {
			for (int ty = 0; ty <= (int)tileBlock.Height - (int)height; ty++) {
				LearnPattern(tileBlock, tx, ty);
			}
		}
	}

	/// <summary>
	/// Remove the tiles of the given tileBlock from the list of valid patterns
	/// </summary>
	/// <param name="tileBlock">tileBlock that specifies the pattern to remove</param>
	public void ForgetPattern(Field<int> tileBlock, int startX, int startY) {
		Field<int> tb = (Field<int>)tileBlock.CloneSubset(startX, startY, width, height);
		if (patterns.Contains(tb)) patterns.Remove(tb);
	}

	protected float calculateSimilarity(Field<int> t1, Field<int> t2) {
		float sim = 0;
		if (t1.Width != t2.Width) throw new ArgumentException("Field<int>s had different widths");
		if (t1.Height != t2.Height) throw new ArgumentException("Field<int>s had different heights");
		for (int px = 0; px < t1.Width; px++) {
			for (int py = 0; py < t1.Height; py++) {
				int id1 = t1[px, py];
				int id2 = t2[px, py];
				Tile tile1 = tileset.Get(id1);
				Tile tile2 = tileset.Get(id2);
				if (tile1 == null) return -1;
				if (tile2 == null) return -1;
				bool solid1 = ((tile1.Attributes & Tile.Attribute.SOLID) != 0);
				bool solid2 = ((tile2.Attributes & Tile.Attribute.SOLID) != 0);
				if (id1 == id2) sim += 1;
				if (solid1 == solid2) sim += 100;
			}
		}
		return sim;
	}

	/// <summary>
	/// Smoothes Tilemap by changing tiles around the given position to one of several stored valid patterns
	/// </summary>
	public void ApplyToTilemap(FieldPos pos, Tilemap tilemap) {
		int px = pos.X - (int)(width/2);
		int py = pos.Y - (int)(height/2);
		if (px < 0) return;
		if (py < 0) return;
		if (px+width > tilemap.Width) return;
		if (py+width > tilemap.Height) return;

		Field<int> tb = (Field<int>)tilemap.CloneSubset(px, py, width, height);

		float bestSimilarity = 0;
		Field<int> bestPattern = null;
		foreach (Field<int> pattern in patterns) {
			float sim = calculateSimilarity(pattern, tb);
			if (sim > bestSimilarity) {
				bestSimilarity = sim;
				bestPattern = pattern;
			}
		}
		if (bestPattern != null) {
			uint StartX = (uint) Math.Max(0, -px);
			uint StartY = (uint) Math.Max(0, -py);
			uint W = Math.Min((uint) (tilemap.Width - px), Width);
			uint H = Math.Min((uint) (tilemap.Height - py), Height);
			for(uint y = StartY; y < H; ++y) {
				for(uint x = StartX; x < W; ++x) {
					tilemap[(uint) (px + x), (uint) (py + y)] = bestPattern[x, y];
				}
			}
		}
	}

	// FIXME: untested
	// TODO: change file syntax from CSV to Lisp
	public void saveToFile(string fname) {
		FileStream fs = new FileStream(fname, FileMode.Create);
		TextWriter tw = new StreamWriter(fs);

		foreach (Field<int> m1 in patterns) {
			tw.WriteLine("" + m1[0, 0] + "," + m1[1, 0] + "," + m1[2, 0] + "," + m1[0, 1] + "," + m1[1, 1] + "," + m1[2, 1] + "," + m1[0, 2] + "," + m1[1, 2] + "," + m1[2, 2] + "");
		}

		tw.Close();
		fs.Close();
	}

	/// <summary>
	/// Creates a new Brush by loading a list of valid patterns from the given file
	/// </summary>
	// TODO: change file syntax from CSV to Lisp
	public static Brush loadFromFile(string fname, Tileset tileset) {
		FileStream fs = new FileStream(fname, FileMode.Open);
		TextReader trd = new StreamReader(fs);

		Brush brush = new Brush(3, 3, tileset);

		try {
			string s;
			while ((s = trd.ReadLine()) != null) {
				string[] v = s.Split(',');
				if (v.Length < 9) continue;
				Field<int> tb = (Field<int>)new Field<int>(3,3,0);
				tb[0, 0] = int.Parse(v[0]);
				tb[1, 0] = int.Parse(v[1]);
				tb[2, 0] = int.Parse(v[2]);
				tb[0, 1] = int.Parse(v[3]);
				tb[1, 1] = int.Parse(v[4]);
				tb[2, 1] = int.Parse(v[5]);
				tb[0, 2] = int.Parse(v[6]);
				tb[1, 2] = int.Parse(v[7]);
				tb[2, 2] = int.Parse(v[8]);
				if (!brush.patterns.Contains(tb)) brush.patterns.Add(tb);
			}
		} finally {
			trd.Close();
			fs.Close();
		}

		return brush;
	}	

}
