//  $Id$
using DataStructures;
using OpenGl;
using System;
using Gdk;

/// <summary>
/// Smoothes Tilemaps by changing tiles to one of several stored valid patterns.
/// Left-click and drag to apply brush.
/// Right-click and drag to select an area with patterns to learn.
/// </summary>
public sealed class BrushEditor : TileEditorBase, IEditor {
	private new Selection selection = new Selection();
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

	public event RedrawEventHandler Redraw;

	public BrushEditor(IEditorApplication application, Tilemap Tilemap, Tileset Tileset, string brushFile)
	{
		selection = new Selection();
		selection.Changed += OnSelectionChanged;
		this.application = application;
		this.Tilemap = Tilemap;
		this.Tileset = Tileset;
		brush = Brush.loadFromFile(brushFile, Tileset);
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
			LastPreviewIsChange = brush.FindBestPattern(MouseTilePos, Tilemap, ref LastPreview);
			LastPreviewPos = MouseTilePos;
		}
	}

	public new void Draw(Gdk.Rectangle cliprect)
	{
		// When not selecting, draw white rectangle over affected tiles
		if(!selecting) {

			// Calculate rectangle to color
			float px = (MouseTilePos.X - (int)(brush.Width / 2)) * 32f;
			float py = (MouseTilePos.Y - (int)(brush.Height / 2)) * 32f;
			float w = brush.Width * 32f;
			float h = brush.Height * 32f;

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
				LastPreview.Draw(pos, Tileset);
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
		if(selecting) {

			// Calculate rectangle to color
			float left = SelectionP1.X * 32f;
			float top = SelectionP1.Y * 32f;
			float right = SelectionP2.X * 32f + 32f;
			float bottom = SelectionP2.Y * 32f + 32f;

			// Draw rectangle
			gl.Color4f(0, 0, 1, 0.7f);
			gl.Disable(gl.TEXTURE_2D);
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

	public void OnMouseButtonPress(Vector MousePos, int button, ModifierType Modifiers)
	{
		UpdateMouseTilePos(MousePos);

		// left mouse button means apply brush
		if(button == 1) {
			application.TakeUndoSnapshot("Brush Tool");
			brush.ApplyToTilemap(MouseTilePos, Tilemap);
			LastDrawPos = MouseTilePos;
			drawing = true;
			Redraw();
		}

		// right mouse button means select area to learn
		if(button == 3) {
			if(MouseTilePos.X < 0 || MouseTilePos.Y < 0
				|| MouseTilePos.X >= Tilemap.Width
				|| MouseTilePos.Y >= Tilemap.Height)
				return;

			SelectStartPos = MouseTilePos;
			selecting = true;
			UpdateSelection();
			Redraw();
		}
	}

	public void OnMouseButtonRelease(Vector MousePos, int button, ModifierType Modifiers)
	{
		UpdateMouseTilePos(MousePos);

		// left mouse button means apply brush
		if(button == 1) {
			drawing = false;
		}

		// right mouse button means select area to learn
		if(button == 3) {
			UpdateSelection();

			uint NewWidth = (uint) (SelectionP2.X - SelectionP1.X) + 1;
			uint NewHeight = (uint) (SelectionP2.Y - SelectionP1.Y) + 1;
			selection.Resize(NewWidth, NewHeight, 0);
			for(uint y = 0; y < NewHeight; y++) {
				for(uint x = 0; x < NewWidth; ++x) {
					selection[x, y]
						= Tilemap[(uint) SelectionP1.X + x,
						          (uint) SelectionP1.Y + y];
				}
			}
			brush.LearnPatterns(selection);

			selection.FireChangedEvent();
			selecting = false;
		}

		Redraw();
	}

	public void OnMouseMotion(Vector MousePos, ModifierType Modifiers)
	{
		if(UpdateMouseTilePos(MousePos)) {
			if (drawing) {
				if (LastDrawPos != MouseTilePos) {
					LastDrawPos = MouseTilePos;
					brush.ApplyToTilemap(MouseTilePos, Tilemap);
				}
			}
			if (selecting) {
				UpdateSelection();
			}
			Redraw();
		}
	}

	private bool UpdateMouseTilePos(Vector MousePos)
	{
		FieldPos NewMouseTilePos = new FieldPos(
				(int) (MousePos.X) / 32,
				(int) (MousePos.Y) / 32);
		if(NewMouseTilePos != MouseTilePos) {
			MouseTilePos = NewMouseTilePos;
			return true;
		}

		return false;
	}

	private void UpdateSelection()
	{
		if(MouseTilePos.X < SelectStartPos.X) {
			if(MouseTilePos.X < 0)
				SelectionP1.X = 0;
			else
				SelectionP1.X = MouseTilePos.X;
			SelectionP2.X = SelectStartPos.X;
		} else {
			SelectionP1.X = SelectStartPos.X;
			if(MouseTilePos.X >= Tilemap.Width)
				SelectionP2.X = (int) Tilemap.Width - 1;
			else
				SelectionP2.X = MouseTilePos.X;
		}

		if(MouseTilePos.Y < SelectStartPos.Y) {
			if(MouseTilePos.Y < 0)
				SelectionP1.Y = 0;
			else
				SelectionP1.Y = MouseTilePos.Y;
			SelectionP2.Y = SelectStartPos.Y;
		} else {
			SelectionP1.Y = SelectStartPos.Y;
			if(MouseTilePos.Y >= Tilemap.Height)
				SelectionP2.Y = (int) Tilemap.Height - 1;
			else
				SelectionP2.Y = MouseTilePos.Y;
		}
	}

	private void OnSelectionChanged() {
		Redraw();
	}
}
