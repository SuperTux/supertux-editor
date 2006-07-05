using DataStructures;
using OpenGl;
using System;
using Gdk;

// TODO: More things should be moved into this class.
/// <summary>
/// Base class for all editors.
/// </summary>
public abstract class EditorBase {
	protected IEditorApplication application;
}

// TODO: More things should be moved into this class.
/// <summary>
/// Base class for editors editing objects.
/// </summary>
public abstract class ObjectEditorBase : EditorBase {
	protected Sector sector;
}

// TODO: More things should be moved into this class.
/// <summary>
/// Base class for editors editing tilemaps.
/// </summary>
public abstract class TileEditorBase : EditorBase {
	protected Selection selection;
	protected bool drawing;
	protected bool selecting;
	protected FieldPos MouseTilePos;
	protected FieldPos SelectStartPos;
	protected FieldPos LastDrawPos;
	protected FieldPos SelectionP1;
	protected FieldPos SelectionP2;

	protected Tilemap Tilemap;
	protected Tileset Tileset;

	public void Draw() {
		if (!selecting) {
			gl.Color4f(1, 1, 1, 0.7f);
			Vector pos = new Vector(MouseTilePos.X * 32f, MouseTilePos.Y * 32f);
			selection.Draw(pos, Tileset);
			gl.Color4f(1, 1, 1, 1);
		}
		if (selecting) {
			gl.Color4f(0, 0, 1, 0.7f);
			gl.Disable(gl.TEXTURE_2D);

			float left = SelectionP1.X * 32f;
			float top = SelectionP1.Y * 32f;
			float right = SelectionP2.X * 32f + 32f;
			float bottom = SelectionP2.Y * 32f + 32f;

			gl.Begin(gl.QUADS);
			gl.Vertex2f(left, top);
			gl.Vertex2f(right, top);
			gl.Vertex2f(right, bottom);
			gl.Vertex2f(left, bottom);
			gl.End();

			gl.Enable(gl.TEXTURE_2D);
			gl.Color4f(1, 1, 1, 1);
		}
	}

}