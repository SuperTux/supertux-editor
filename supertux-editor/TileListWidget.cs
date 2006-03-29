using System;
using System.Collections.Generic;
using Gtk;
using Gdk;
using OpenGl;
using Drawing;
using SceneGraph;
using DataStructures;

public class TileListWidget : GLWidgetBase {
	private Tileset Tileset;
	private List<uint> Tiles = new List<uint>();
	private Selection Selection;

	private const int TILE_WIDTH = 32;
	private const int TILE_HEIGHT = 32;
	private const int SPACING_X = 1;
	private const int SPACING_Y = 1;
	private const int ROW_HEIGHT = TILE_HEIGHT + SPACING_Y;
	private const int COLUMN_WIDTH = TILE_WIDTH + SPACING_X;
	private const int TILES_PER_ROW = 4;

	private int hovertile = -1;

	public TileListWidget(IEditorApplication Application, Selection Selection)
	{
		this.Selection = Selection;
		Selection.Changed += OnSelectionChanged;
		
		Tileset.LoadEditorImages = true;
		SetSizeRequest((TILE_WIDTH + SPACING_X) * TILES_PER_ROW, -1);
		
		ButtonPressEvent += OnButtonPress;
		ButtonReleaseEvent += OnButtonRelease;
		MotionNotifyEvent += OnMotionNotify;
		ScrollEvent += OnScroll;
		
		AddEvents((int) Gdk.EventMask.ButtonPressMask);
		AddEvents((int) Gdk.EventMask.ButtonReleaseMask);
		AddEvents((int) Gdk.EventMask.PointerMotionMask);						
		AddEvents((int) Gdk.EventMask.ScrollMask);	

		Application.LevelChanged += OnLevelChanged;
	}

	private void OnLevelChanged(Level Level)
	{
		Tileset = Level.Tileset;
		Translation = new Vector(0, 0);
		Zoom = 1.0f;

		Tiles.Clear();
		for(uint Id = 0; Id < Tileset.LastTileId; ++Id) {
			if(Tileset.IsValid(Id))
				Tiles.Add(Id);
		}
			
		QueueDraw();
	}

	private void OnSelectionChanged()
	{
		QueueDraw();
	}

	protected override void DrawGl()
	{
		if(Tileset == null)
			return;
	
		gl.Clear(gl.COLOR_BUFFER_BIT);
		
		int starttile = (int) -Translation.Y / (ROW_HEIGHT) 
						* TILES_PER_ROW;
		Vector pos = new Vector(0,
				(starttile / TILES_PER_ROW) * (ROW_HEIGHT));
		float maxwidth = (TILE_WIDTH + SPACING_X) * TILES_PER_ROW;
		for(int i = starttile; i < Tiles.Count; i++) {
			Tile Tile = Tileset.Get(Tiles[i]);

			Tile.DrawEditor(pos);

			bool selected = false;
			if(Selection.TileListFirstTile > 0
					&& i > Selection.TileListFirstTile
					&& i - Selection.TileListFirstTile 
						< Selection.TileListH * TILES_PER_ROW
					&& i - Selection.TileListFirstTile % TILES_PER_ROW 
						< Selection.TileListW-1) {
				selected = true;
			} else if(Selection.Width == 1 && Selection.Height == 1 &&
					Tiles[i] == Selection[0, 0]) {
				selected = true;
			}

			if(i == hovertile || selected) {
				if(selected)
					gl.Color4f(0, 0, 1, 0.4f);
				else
					gl.Color4f(0, 0, 1, 0.08f);
				gl.Disable(gl.TEXTURE_2D);
				gl.Begin(gl.QUADS);
				gl.Vertex2f(pos.X, pos.Y);
				gl.Vertex2f(pos.X + TILE_WIDTH, pos.Y);
				gl.Vertex2f(pos.X + TILE_WIDTH, pos.Y + TILE_HEIGHT);
				gl.Vertex2f(pos.X, pos.Y + TILE_HEIGHT);
				gl.End();
				gl.Enable(gl.TEXTURE_2D);
				gl.Color4f(1, 1, 1, 1);
			}
			pos.X += TILE_WIDTH + SPACING_X;
			if(pos.X >= maxwidth) {
				pos.X = 0;
				pos.Y += ROW_HEIGHT;
				if(pos.Y + Translation.Y > Allocation.Height)
					break;
			}
		}
	}

	private void OnButtonPress(object o, ButtonPressEventArgs args)
	{
		if(args.Event.Button == 1) {
			Vector MousePos = new Vector((float) args.Event.X,
	    								 (float) args.Event.Y);
			int tile = PosToTile(MousePos);
			if(tile < 0)
				return;
			
			Selection.Resize(1, 1, 0);
			Selection[0, 0] = Tiles[tile];
			Selection.FireChangedEvent();
			QueueDraw();
		}
	}

	private void OnButtonRelease(object o, ButtonReleaseEventArgs args)
	{
	}

	private void OnMotionNotify(object o, MotionNotifyEventArgs args)
	{
		Vector MousePos = new Vector((float) args.Event.X,
									 (float) args.Event.Y);
		int newtile = PosToTile(MousePos);
		if(newtile != hovertile) {
			QueueDraw();
		}
		hovertile = newtile;
	}

	private int PosToTile(Vector pos)
	{
		int tile = (int) (pos.Y - Translation.Y) / ROW_HEIGHT * TILES_PER_ROW
			+ (int) (pos.X - Translation.X) / COLUMN_WIDTH;
		if(tile < 0) {
			Console.WriteLine("Warning: PosToTile < 0?!?");
			return -1;
		}
		if(tile >= Tiles.Count)
			return -1;

		return tile;
	}

	private void OnScroll(object o, ScrollEventArgs args)
	{
		if(args.Event.Direction == ScrollDirection.Up &&
				Translation.Y <= (float) -ROW_HEIGHT) {
			Translation = Translation + new Vector(0, ROW_HEIGHT);
			args.RetVal = true;
		} else if(args.Event.Direction == ScrollDirection.Down) {
			Translation = Translation - new Vector(0, ROW_HEIGHT);
			args.RetVal = true;
		}
	}
}

