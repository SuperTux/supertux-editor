using System;
using Gtk;

public class EditorStock
{
	static Gtk.IconFactory stock = new Gtk.IconFactory ();

	public static string Eye = "eye";
	public static string EyeHalf = "eye-half";
	
	static EditorStock ()
	{
		AddIcon (Eye, "stock-eye-12.png");
		AddIcon (EyeHalf, "stock-eye-half-12.png");
		
		stock.AddDefault ();
	}
	
	static void AddIcon (string stockid, string resource)
	{
		Gtk.IconSet iconset = stock.Lookup (stockid);
		
		if (iconset == null) {
			iconset = new Gtk.IconSet ();
			Gdk.Pixbuf img = Gdk.Pixbuf.LoadFromResource (resource);
			IconSource source = new IconSource ();
			source.Size = Gtk.IconSize.Menu;
			source.SizeWildcarded = false;
			source.Pixbuf = img;
			iconset.AddSource (source);
			stock.Add (stockid, iconset);
		}
	}
}
