using DataStructures;
using Gdk;

public interface IEditor {
	void Draw();
	void OnMouseButtonPress(Vector pos, int button, ModifierType Modifiers);
	void OnMouseButtonRelease(Vector pos, int button, ModifierType Modifiers);
	void OnMouseMotion(Vector pos, ModifierType Modifiers);

	event RedrawEventHandler Redraw;
}

public delegate void RedrawEventHandler();
