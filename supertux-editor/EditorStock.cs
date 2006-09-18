using System;
using Gtk;

public static class EditorStock
{
	static Gtk.IconFactory stock = new Gtk.IconFactory ();

	public static string Eye = "eye";
	public static string EyeHalf = "eye-half";
	public static string ToolSelect = "ToolSelect";
	public static string ToolTiles = "ToolTiles";
	public static string ToolObjects = "ToolObjects";
	public static string ToolBrush = "ToolBrush";
	public static string ToolFill = "ToolFill";
	public static string ToolReplace = "ToolReplace";

	static EditorStock ()
	{
		AddIcon (Eye, Gtk.IconSize.Menu, "stock-eye-12.png");
		AddIcon (EyeHalf, Gtk.IconSize.Menu, "stock-eye-half-12.png");
		AddIcon (ToolSelect, Gtk.IconSize.LargeToolbar, "stock-tool-select-24.png");
		AddIcon (ToolTiles, Gtk.IconSize.LargeToolbar, "stock-tool-tiles-24.png");
		AddIcon (ToolObjects, Gtk.IconSize.LargeToolbar, "stock-tool-objects-24.png");
		AddIcon (ToolBrush, Gtk.IconSize.LargeToolbar, "stock-tool-brush-24.png");
		AddIcon (ToolFill, Gtk.IconSize.LargeToolbar, "stock-tool-fill-24.png");
		AddIcon (ToolReplace, Gtk.IconSize.LargeToolbar, "stock-tool-replace-24.png");

		stock.AddDefault ();
	}

	static void AddIcon (string stockid, Gtk.IconSize iconSize, string resource)
	{
		Gtk.IconSet iconset = stock.Lookup (stockid);

		if (iconset == null) {
			iconset = new Gtk.IconSet ();
			Gdk.Pixbuf img = Gdk.Pixbuf.LoadFromResource (resource);
			IconSource source = new IconSource ();
			source.Size = iconSize;
			source.SizeWildcarded = false;
			source.Pixbuf = img;
			iconset.AddSource (source);
			stock.Add (stockid, iconset);
		}
	}
}
