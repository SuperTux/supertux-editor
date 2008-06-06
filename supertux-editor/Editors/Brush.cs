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
using LispReader;

/// <summary>
/// Smoothes Tilemaps by changing tiles to one of several stored valid patterns
/// </summary>
public class Brush
{
	/// <summary>
	/// width (in Tiles) of this Brush
	/// </summary>
	protected uint width;

	/// <summary>
	/// height (in Tiles) of this Brush
	/// </summary>
	protected uint height;

	/// <summary>
	/// List of TileBlocks that constitute a valid pattern
	/// </summary>
	/// <seealso cref="TileBlock"/>
	protected List<TileBlock> patterns = new List<TileBlock>();

	/// <summary>
	/// Tileset this Brush uses
	/// </summary>
	protected Tileset tileset;

	/// <summary>
	/// Create a new brush from a given list of patterns
	/// </summary>
	public Brush(uint width, uint height, List<TileBlock> patterns, Tileset tileset)
	{
		this.width = width;
		this.height = height;
		this.patterns = patterns;
		this.tileset = tileset;
	}

	/// <summary>
	/// Create a new, empty brush
	/// </summary>
	public Brush(uint width, uint height, Tileset tileset)
	{
		this.width = width;
		this.height = height;
		this.patterns = new List<TileBlock>();
		this.tileset = tileset;
	}

	/// <summary>
	/// Number of patterns this brush has stored
	/// </summary>
	public int PatternCount {
		get {
			return patterns.Count;
		}
	}

	/// <summary>
	/// Width (in Tiles) of patterns in this Brush
	/// </summary>
	public uint Width {
		get {
			return width;
		}
	}

	/// <summary>
	/// Height (in Tiles) of patterns in this Brush
	/// </summary>
	public uint Height {
		get {
			return height;
		}
	}

	/// <summary>
	/// Return the known pattern that has identical contents as the given TileBlock, or null if none is found
	/// </summary>
	/// <param name="tileBlock">tileBlock to search for</param>
	public TileBlock FindPattern(TileBlock tileBlock) {
		foreach (TileBlock pattern in patterns) {
			if (pattern.EqualContents(tileBlock)) return pattern;
		}
		return null;
	}

	/// <summary>
	/// Add the tiles of the given tileBlock as a valid pattern
	/// </summary>
	/// <param name="tileBlock">tileBlock that specifies the pattern to add</param>
	/// <param name="startX">horizontal offset (in tileBlock) of the pattern to add</param>
	/// <param name="startY">vertical offset (in tileBlock) of the pattern to add</param>
	public void LearnPattern(TileBlock tileBlock, int startX, int startY) {
		TileBlock tb = new TileBlock(tileBlock, startX, startY, width, height);
		if (FindPattern(tb) == null) patterns.Add(tb);
	}

	/// <summary>
	/// Add all tiles of the given tileBlock to the list of valid patterns
	/// </summary>
	/// <param name="tileBlock">tileBlock that specifies the patterns to add</param>
	public void LearnPatterns(TileBlock tileBlock) {
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
	/// <param name="startX">horizontal offset (in tileBlock) of the pattern to remove</param>
	/// <param name="startY">vertical offset (in tileBlock) of the pattern to remove</param>
	public void ForgetPattern(TileBlock tileBlock, int startX, int startY) {
		TileBlock tb = new TileBlock(tileBlock, startX, startY, width, height);
		TileBlock pattern = FindPattern(tb);
		if (pattern != null) patterns.Remove(pattern);
	}

	/// <summary>
	/// Return a measure of how similar fields t1 and t2 are.
	/// </summary>
	protected float calculateSimilarity(TileBlock t1, TileBlock t2) {
		float sim = 0;

		// make sure both fields have equal dimensions
		if (t1.Width != t2.Width) throw new ArgumentException("TileBlocks had different widths");
		if (t1.Height != t2.Height) throw new ArgumentException("TileBlocks had different heights");

		// compare each tile in the fields
		for (int px = 0; px < t1.Width; px++) {
			for (int py = 0; py < t1.Height; py++) {
				int id1 = t1[px, py];
				int id2 = t2[px, py];
				Tile tile1 = tileset.Get(id1);
				Tile tile2 = tileset.Get(id2);

				// if any of the tile ids is invalid, make this a really bad match
				if (tile1 == null) return -1;
				if (tile2 == null) return -1;

				bool solid1 = ((tile1.Attributes & Tile.Attribute.SOLID) != 0);
				bool solid2 = ((tile2.Attributes & Tile.Attribute.SOLID) != 0);

				// reward identical tiles...
				if (id1 == id2) sim += 1;

				// ...but not changing solid to non-solid (and vice versa) is even better
				if (solid1 == solid2) sim += 100;
			}
		}
		return sim;
	}

	/// <summary>
	/// Smoothes Tilemap by changing tiles around the given position to one of the stored patterns
	/// </summary>
	public void ApplyToTilemap(FieldPos pos, Tilemap tilemap) {

		// find upper-left corner of where to apply brush
		int px = pos.X - (int)(width/2);
		int py = pos.Y - (int)(height/2);

		TileBlock bestPattern = null;
		
		// if we find any usable pattern, we apply it
		if (FindBestPattern(px, py, tilemap, ref bestPattern)) {
			bestPattern.ApplyToTilemap(new FieldPos(px,py),tilemap);
		}
	}

	/// <summary>
	/// Find the best pattern to use when changing tiles around the given position to one of the stored patterns.
	/// </summary>
	/// <param name="pos">The (center) position at the <paramref name="tilemap"/> to look at.</param>
	/// <param name="tilemap">The tilemap to look at.</param>
	/// <param name="bestPattern">A tileblock that will replace the area.</param>
	/// <returns>
	/// True if <paramref name="bestPattern"/> is diffrent from the calculated
	/// pattern in <paramref name="tilemap"/>, otherwise false.
	/// </returns>
	public bool FindBestPattern(FieldPos pos, Tilemap tilemap, ref TileBlock bestPattern) {
		// find upper-left corner of where to apply brush
		int px = pos.X - (int) (width / 2);
		int py = pos.Y - (int) (height / 2);

		return FindBestPattern(px, py, tilemap, ref bestPattern);
	}

	/// <summary>
	/// Find the best pattern to use when changing tiles to one of the stored patterns (with topright position arguments).
	/// </summary>
	/// <param name="px">The starting X position at the <paramref name="tilemap"/> to look at.</param>
	/// <param name="py">The starting Y position at the <paramref name="tilemap"/> to look at.</param>
	/// <param name="tilemap">The tilemap to look at.</param>
	/// <param name="bestPattern">A tileblock that will replace the area.</param>
	/// <returns>
	/// True if <paramref name="bestPattern"/> is diffrent from the calculated
	/// pattern in <paramref name="tilemap"/>, otherwise false.
	/// </returns>
	public bool FindBestPattern(int px, int py, Tilemap tilemap, ref TileBlock bestPattern) {
		// Make sure we are in bounds of the tilemap, and don't throw an Exception, this
		// is user input that haven't been checked.
		if (px < 0) return false;
		if (py < 0) return false;
		if (px + width > tilemap.Width) return false;
		if (py + width > tilemap.Height) return false;

		// store subset of tilemap where brush will be applied as a reference pattern
		TileBlock tb = new TileBlock(tilemap, px, py, width, height);

		// find the stored pattern that matches this reference pattern best
		float bestSimilarity = 0;
		bestPattern = null;
		foreach (TileBlock pattern in patterns) {
			float sim = calculateSimilarity(pattern, tb);
			if (sim > bestSimilarity) {
				bestSimilarity = sim;
				bestPattern = pattern;
			}
		}
		return ! ((bestPattern == null) || (bestPattern.EqualContents(tb)));
	}

	// FIXME: untested
	// TODO: change file syntax from CSV to Lisp
	public void saveToFile(string fname) {
		FileStream fs = new FileStream(fname, FileMode.Create);
		TextWriter tw = new StreamWriter(fs);

		patterns.Sort();

		foreach (TileBlock m1 in patterns) {
			tw.WriteLine(m1[0, 0] + "," + m1[1, 0] + "," + m1[2, 0] + "," + m1[0, 1] + "," + m1[1, 1] + "," + m1[2, 1] + "," + m1[0, 2] + "," + m1[1, 2] + "," + m1[2, 2]);
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
				TileBlock tb = (TileBlock)new TileBlock(3,3,0);
				tb[0, 0] = int.Parse(v[0]);
				tb[1, 0] = int.Parse(v[1]);
				tb[2, 0] = int.Parse(v[2]);
				tb[0, 1] = int.Parse(v[3]);
				tb[1, 1] = int.Parse(v[4]);
				tb[2, 1] = int.Parse(v[5]);
				tb[0, 2] = int.Parse(v[6]);
				tb[1, 2] = int.Parse(v[7]);
				tb[2, 2] = int.Parse(v[8]);
				if (brush.FindPattern(tb) == null) brush.patterns.Add(tb);
			}
		} finally {
			trd.Close();
			fs.Close();
		}

		return brush;
	}

}
