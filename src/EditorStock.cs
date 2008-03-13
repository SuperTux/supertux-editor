//  $Id$
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
	public static string Background = "Background";
	/// <summary>Icon for windows and dialogs</summary>
	public static Gdk.Pixbuf WindowIcon;

	static EditorStock ()
	{
		// Load Window icon.
		WindowIcon = Gdk.Pixbuf.LoadFromResource ("supertux-editor.png");

		AddIcon (Eye, Gtk.IconSize.Menu, "stock-eye-12.png");
		AddIcon (EyeHalf, Gtk.IconSize.Menu, "stock-eye-half-12.png");

		// HACK: This is needed to make tool icons show up on Windows, no idea why.
#if WINDOWS
		Gtk.IconSize ToolBarIconSize = Gtk.IconSize.SmallToolbar;
#else
		Gtk.IconSize ToolBarIconSize = Gtk.IconSize.LargeToolbar;
#endif
		AddIcon(ToolSelect, ToolBarIconSize, "stock-tool-select-24.png");
		AddIcon(ToolTiles, ToolBarIconSize, "stock-tool-tiles-24.png");
		AddIcon(ToolObjects, ToolBarIconSize, "stock-tool-objects-24.png");
		AddIcon(ToolBrush, ToolBarIconSize, "stock-tool-brush-24.png");
		AddIcon(ToolFill, ToolBarIconSize, "stock-tool-fill-24.png");
		AddIcon(ToolReplace, ToolBarIconSize, "stock-tool-replace-24.png");
		AddIcon(Background, ToolBarIconSize, "stock-background-24.png");

		stock.AddDefault();
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
