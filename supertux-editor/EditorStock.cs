//  SuperTux Editor
//  Copyright (C) 2006 Matthias Braun <matze@braunis.de>
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

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
	public static string ToolPath = "ToolPath";
	public static string Camera = "Camera";
	public static string CameraMenuImage = "Camera";
	/// <summary>Icon for windows and dialogs</summary>
	public static Gdk.Pixbuf WindowIcon;

	static EditorStock ()
	{
		// Load Window icon.
		WindowIcon = Gdk.Pixbuf.LoadFromResource ("supertux-editor.png");

		AddIcon (Eye, Gtk.IconSize.Menu, "stock-eye-12.png");
		AddIcon (EyeHalf, Gtk.IconSize.Menu, "stock-eye-half-12.png");

		// HACK: This is needed to make tool icons show up on Windows, no idea why.
		// TODO: test if this is still needed with additional SizeWildcarded.
		// SizeWildcarded only gives fuzzy images, at least for stock-eye-12.png
		// It looks like windows are confusing large and small toolbar size ("SmallToolbar" causes 2x bigger buttons that "LargeToolbar"), bug in GTK for windows? bug in windows themselves?
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
		AddIcon(ToolPath, ToolBarIconSize, "stock-tool-path-24.png");
		AddIcon(Camera, ToolBarIconSize, "stock-camera-24.png");

		stock.AddDefault();
	}

	static void AddIcon (string stockid, Gtk.IconSize iconSize, string resource)
	{
		Gtk.IconSet iconset = stock.Lookup (stockid);

		if (iconset == null) {
			iconset = new Gtk.IconSet ();
			Gdk.Pixbuf img = Gdk.Pixbuf.LoadFromResource (resource);
			//no scaling in the given size, ...
			IconSource source = new IconSource ();
			source.Size = iconSize;
			source.SizeWildcarded = false;
			source.Pixbuf = img;
			iconset.AddSource (source);
			//... but allow to use the image for all other sizes, too.
			source = new IconSource ();
			source.SizeWildcarded = true;
			source.Pixbuf = img;
			iconset.AddSource (source);

			stock.Add (stockid, iconset);
		}
	}
}

/* EOF */
