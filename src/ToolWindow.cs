// $Id$
using System;
using Gtk;

/**
 * This class represents a window that floats above the application
 */
public class ToolWindow : Window
{
	public ToolWindow(Window parent, string title)
		: base(title)
	{
		KeepAbove = true;
		Resizable = true;
		SkipPagerHint = true;
		SkipTaskbarHint = true;
		TransientFor = parent;
		WindowPosition = WindowPosition.None;
	}

	/// saves current layout of the window to disk
	public void Save()
	{
		// TODO
	}

	/// restores the layout of the window
	public void Restore()
	{
		// TODO
	}
}
