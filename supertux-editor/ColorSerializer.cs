using System;
using Drawing;
using Lisp;
using LispReader;

[LispCustomSerializer(typeof(Color))]
public class ColorSerializer : ILispSerializer
{
	public object Read(List list)
	{
		if(list.Length < 4 || list.Length > 5)
			throw new Exception("Lisp list must have 4 or 5 entries for color");
		
		Color result = new Color();
		result.Red = GetFloat(list[1]);
		result.Green = GetFloat(list[2]);
		result.Blue = GetFloat(list[3]);
		if(list.Length == 5)
			result.Alpha = GetFloat(list[4]);
		else
			result.Alpha = 1.0f;
		
		return result;
	}
	
	public void Write(Writer writer, string name, object Object)
	{
		Color color = (Color) Object;
		float[] vals;
		if(color.Alpha != 1.0f)
			vals = new float[] { color.Red, color.Green, color.Blue, color.Alpha };
		else
			vals = new float[] { color.Red, color.Green, color.Blue };
		
		writer.Write(name, vals);
	}
	
	private static float GetFloat(object obj)
	{
		if(obj is int)
			return (float) ((int) obj);
		
		return (float) obj;
	}
}
