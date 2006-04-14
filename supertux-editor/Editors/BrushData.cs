using System;
using System.Collections.Generic;

/*
public class BrushData
{
	protected List<int[,]> id_matrices = new List<int[,]>();
	protected List<bool[,]> solid_matrices = new List<bool[,]>();	
	
	public BrushData()
	{
	}
	
	public void saveToFile(string fname) {
		FileStream fs = new FileStream(fname, FileMode.Create);
		TextWriter tw = new StreamWriter(fs);

		foreach (int[,] m1 in id_matrices) {
			tw.WriteLine("" + m1[0, 0] + "," + m1[0, 1] + "," + m1[0, 2] + "," + m1[1, 0] + "," + m1[1, 1] + "," + m1[1, 2] + "," + m1[2, 0] + "," + m1[2, 1] + "," + m1[2, 2] + "");
		}

		tw.Close();
		fs.Close();
	}

	public void loadFromFile(string fname) {
		FileStream fs = new FileStream(fname, FileMode.Open);
		TextReader trd = new StreamReader(fs);
		
		try {
			string s;
			while ((s = trd.ReadLine()) != null) {
				string[] v = s.Split(',');
				if (v.Length < 9)
					continue;
				int[,] ids = new int[3, 3] { 
				    {int.Parse(v[0]), int.Parse(v[1]), int.Parse(v[2])},
				    {int.Parse(v[3]), int.Parse(v[4]), int.Parse(v[5])},
				    {int.Parse(v[6]), int.Parse(v[7]), int.Parse(v[8])}
				};
				bool[,] sols = new bool[3, 3] {
				    {tr.isSolid[int.Parse(v[0])], tr.isSolid[int.Parse(v[1])], tr.isSolid[int.Parse(v[2])]},
				    {tr.isSolid[int.Parse(v[3])], tr.isSolid[int.Parse(v[4])], tr.isSolid[int.Parse(v[5])]},
				    {tr.isSolid[int.Parse(v[6])], tr.isSolid[int.Parse(v[7])], tr.isSolid[int.Parse(v[8])]}
				};
				id_matrices.Add(ids);
				solid_matrices.Add(sols);
			}
		} finally {
			trd.Close();
			fs.Close();
		}
   }	
}
*/
