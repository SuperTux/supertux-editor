//  $Id$
using DataStructures;
using OpenGl;
using Gdk;

/// <summary>
/// Smooths Tilemaps by changing tiles to one of several stored valid patterns.
/// Left-click and drag to apply brush.
/// Right-click and drag to select an area with patterns to learn.
/// </summary>
public sealed class BrushEditor : TileEditorBase, IEditor {
	private Brush brush;

	/// <summary>
	/// Contains position where last preview was generated.
	/// </summary>
	private FieldPos LastPreviewPos;
	/// <summary>
	/// A cache of the preview of changing the current "active" area
	/// </summary>
	private TileBlock LastPreview;
	/// <summary>
	/// Stores if the last preview would be a change or not.
	/// </summary>
	private bool LastPreviewIsChange;

	public BrushEditor(IEditorApplication application, Tileset Tileset, string brushFile)
		: base(application, Tileset, new Selection()) {
		brush = Brush.loadFromFile(brushFile, Tileset);
		ActionName = "Tile Brush";
	}

	/// <summary>
	/// Brush currently in use
	/// </summary>
	public Brush Brush {
		get {
			return brush;
		}
	}

	/// <summary>
	/// Updates the LastPreview if the current mouse position has changed.
	/// </summary>
	private void UpdatePreview() {
		if (LastPreviewPos != MouseTilePos) {
			LastPreviewIsChange = brush.FindBestPattern(MouseTilePos, application.CurrentTilemap, ref LastPreview);
			LastPreviewPos = MouseTilePos;
		}
	}

	public new void Draw(Gdk.Rectangle cliprect)
	{
		// When not selecting, draw white rectangle over affected tiles
		if(state == State.DRAWING) {

			// Calculate rectangle to color
			float px = MouseTilePos.X * Tileset.TILE_WIDTH + application.CurrentTilemap.X;
			float py = MouseTilePos.Y * Tileset.TILE_HEIGHT + application.CurrentTilemap.Y;
			float w = Tileset.TILE_WIDTH;
			float h = Tileset.TILE_HEIGHT;

			// Draw rectangle
			gl.Color4f(1, 1, 1, 0.25f);
			gl.Disable(gl.TEXTURE_2D);
			gl.Begin(gl.QUADS);
			gl.Vertex2f(px, py);
			gl.Vertex2f(px+w, py);
			gl.Vertex2f(px+w, py+h);
			gl.Vertex2f(px, py+h);
			gl.End();
			gl.Enable(gl.TEXTURE_2D);

			// Draw a preview if we can.
			UpdatePreview();
			if ((LastPreview != null) && (px > 0) && (py > 0)) {
				gl.Color4f(1, 1, 1, 0.7f);
				Vector pos = new Vector(px, py);
				Tileset.Get(LastPreview[LastPreview.Width/2,LastPreview.Height/2]).DrawEditor(pos);
			}

			// Draw a red rectangle around if the preview is a change
			if (LastPreviewIsChange) {
				gl.Color4f(1, 0, 0, 1);
				gl.Disable(gl.TEXTURE_2D);
				gl.PolygonMode(gl.FRONT_AND_BACK, gl.LINE);

				gl.Begin(gl.QUADS);
				gl.Vertex2f(px, py);
				gl.Vertex2f(px+w, py);
				gl.Vertex2f(px+w, py+h);
				gl.Vertex2f(px, py+h);
				gl.End();

				gl.PolygonMode(gl.FRONT_AND_BACK, gl.FILL);
				gl.Enable(gl.TEXTURE_2D);

			}
			gl.Color4f(1, 1, 1, 1);
		}

		// When selecting, draw blue rectangle over selected area
		if(state == State.SELECTING || state == State.FILLING) {
			base.Draw(cliprect);
		}
	}

	public override void EditorAction(ModifierType Modifiers)
	{
		brush.ApplyToTilemap(MouseTilePos, application.CurrentTilemap);
	}

	public override void SelectionDoneAction(Selection selection)
	{
		brush.LearnPatterns(selection);
	}

	public new void OnMouseMotion(Vector mousePos, ModifierType Modifiers)
	{
		if (UpdateMouseTilePos(mousePos)) {
			if (state == State.DRAWING) {
				if (LastDrawPos != MouseTilePos) {
					LastDrawPos = MouseTilePos;
					brush.ApplyToTilemap(MouseTilePos, application.CurrentTilemap);
				}
			}
			if (state == State.FILLING || state == State.SELECTING) {
				UpdateSelection();
			}
			FireRedraw();
		}
	}
}
