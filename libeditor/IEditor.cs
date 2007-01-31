//  $Id$
using DataStructures;
using Gdk;
using Gtk;

public interface IEditor
{
	/// <summary>
	/// Draw it
	/// </summary>
	/// <param name="cliprect"></param>
	void Draw(Gdk.Rectangle cliprect);
	void OnMouseButtonPress(Vector mousePos, int button, ModifierType Modifiers);
	void OnMouseButtonRelease(Vector mousePos, int button, ModifierType Modifiers);
	void OnMouseMotion(Vector mousePos, ModifierType Modifiers);

	event RedrawEventHandler Redraw;
}
public delegate void RedrawEventHandler();

public interface IEditorCursorChange
{
	event CursorChangeHandler CursorChange;
}
public delegate void CursorChangeHandler(Cursor cursor);
