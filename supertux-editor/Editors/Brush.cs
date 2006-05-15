//  $Id: foo.cpp 2979 2006-01-10 00:00:04Z sommer $
// 
//  Cobble - A simple SuperTux level editor
//  Copyright (C) 2006 Christoph Sommer <supertux@2006.expires.deltadevelopment.de>
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

class Brush 
{
    protected List<TileBlock> patterns;
	protected uint width;
    protected uint height;
    protected Tileset tileset;

	public Brush(uint width, uint height, List<TileBlock> patterns, Tileset tileset)
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
        this.patterns = new List<TileBlock>();
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
    public void LearnPattern(TileBlock tileBlock, int startX, int startY) {
        TileBlock tb = (TileBlock)tileBlock.CloneSubset(startX, startY, width, height);
        if (!patterns.Contains(tb)) patterns.Add(tb);
    }

    /// <summary>
    /// Add all tiles of the given tileBlock to the list of valid patterns
    /// </summary>
    /// <param name="tileBlock">tileBlock that specifies the patterns to add</param>
    public void LearnPatterns(TileBlock tileBlock) {
        for (int tx = 0; tx < tileBlock.Width - width; tx++) {
            for (int ty = 0; ty < tileBlock.Height - height; ty++) {
                LearnPattern(tileBlock, tx, ty);
            }
        }
    }

    /// <summary>
    /// Remove the tiles of the given tileBlock from the list of valid patterns
    /// </summary>
    /// <param name="tileBlock">tileBlock that specifies the pattern to remove</param>
    public void ForgetPattern(TileBlock tileBlock, int startX, int startY) {
        TileBlock tb = (TileBlock)tileBlock.CloneSubset(startX, startY, width, height);
        if (patterns.Contains(tb)) patterns.Remove(tb);
    }

    protected float calculateSimilarity(TileBlock t1, TileBlock t2) {
        float sim = 0;
        if (t1.Width != t2.Width) throw new ArgumentException("TileBlocks had different widths");
        if (t1.Height != t2.Height) throw new ArgumentException("TileBlocks had different heights");
        for (int px = 0; px < t1.Width; px++) {
            for (int py = 0; py < t1.Height; py++) {
                int id1 = t1[px, py];
                int id2 = t2[px, py];
                bool solid1 = ((tileset.Get(id1).Attributes & Tile.Attribute.SOLID) != 0);
                bool solid2 = ((tileset.Get(id2).Attributes & Tile.Attribute.SOLID) != 0);
                if (id1 == id2) sim += 1;
                if (solid1 == solid2) sim += 100;
            }
        }
        return sim;
    }

	public void ApplyToTilemap(FieldPos pos, Tilemap tilemap, Tileset tileset) {
        int px = pos.X - (int)(width/2);
        int py = pos.Y - (int)(height/2);
		if (px < 0) return;
		if (py < 0) return;
		if (px+width > tilemap.Width) return;
		if (py+width > tilemap.Height) return;

        TileBlock tb = (TileBlock)tilemap.CloneSubset(px, py, width, height);

        float bestSimilarity = 0;
        TileBlock bestPattern = null;
        foreach (TileBlock pattern in patterns) {
            float sim = calculateSimilarity(pattern, tb);
            if (sim > bestSimilarity) {
                bestSimilarity = sim;
                bestPattern = pattern;
            }
        }
        if (bestPattern != null) {
            bestPattern.ApplyToTilemap(new FieldPos(px, py), tilemap);
        }
	}
	
	public void saveToFile(string fname) {
		FileStream fs = new FileStream(fname, FileMode.Create);
		TextWriter tw = new StreamWriter(fs);

		foreach (TileBlock m1 in patterns) {
			tw.WriteLine("" + m1[0, 0] + "," + m1[0, 1] + "," + m1[0, 2] + "," + m1[1, 0] + "," + m1[1, 1] + "," + m1[1, 2] + "," + m1[2, 0] + "," + m1[2, 1] + "," + m1[2, 2] + "");
		}

		tw.Close();
		fs.Close();
	}

	public static Brush loadFromFile(string fname, Tileset tileset) {
		FileStream fs = new FileStream(fname, FileMode.Open);
		TextReader trd = new StreamReader(fs);

        Brush brush = new Brush(3, 3, tileset);

		try {
			string s;
			while ((s = trd.ReadLine()) != null) {
				string[] v = s.Split(',');
				if (v.Length < 9) continue;
				TileBlock tb = (TileBlock)new Field<int>(3,3,0);
                tb[0, 0] = int.Parse(v[0]);
                tb[0, 1] = int.Parse(v[1]);
                tb[0, 2] = int.Parse(v[2]);
                tb[1, 0] = int.Parse(v[3]);
                tb[1, 1] = int.Parse(v[4]);
                tb[1, 2] = int.Parse(v[5]);
                tb[1, 0] = int.Parse(v[6]);
                tb[1, 1] = int.Parse(v[7]);
                tb[1, 2] = int.Parse(v[8]);
                if (!brush.patterns.Contains(tb)) brush.patterns.Add(tb);
			}
		} finally {
			trd.Close();
			fs.Close();
		}

        return brush;
   }	

}
