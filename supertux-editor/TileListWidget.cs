using System;
using System.Collections.Generic;
using Gtk;
using Gdk;
using OpenGl;
using Drawing;
using SceneGraph;
using DataStructures;

public class TileListWidget : GLWidgetBase {
	private Tileset tileset;
	private Tilegroup tilegroup;
	private Selection selection;
	private Level level;

	private const int TILE_WIDTH = 32;
	private const int TILE_HEIGHT = 32;
	private const int SPACING_X = 1;
	private const int SPACING_Y = 1;
	private const int ROW_HEIGHT = TILE_HEIGHT + SPACING_Y;
	private const int COLUMN_WIDTH = TILE_WIDTH + SPACING_X;
	private const int TILES_PER_ROW = 4;

	private int hovertile = -1;
	
	private IEditorApplication application;

	public TileListWidget(IEditorApplication application, Selection selection)
	{
		this.selection = selection;
		selection.Changed += OnSelectionChanged;
		this.application = application;
		
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

		application.LevelChanged += OnLevelChanged;
	}
	
	public void ChangeTilegroup(Tilegroup tilegroup)
	{
		this.tilegroup = tilegroup;
		Translation = new Vector(0, 0);
		Zoom = 1.0f;
		hovertile = -1;
		QueueDraw();
	}

	private void OnLevelChanged(Level level)
	{
		if(this.level != null)
			this.level.TilesetChanged -= OnTilesetChanged;
		if(level != null)
			level.TilesetChanged += OnTilesetChanged;
		
		this.level = level;
		OnTilesetChanged(level);
	}
	
	private void OnTilesetChanged(Level level)
	{
		tileset = level.Tileset;
		Translation = new Vector(0, 0);
		Zoom = 1.0f;
		
		tilegroup = tileset.Tilegroups["All"];
			
		QueueDraw();
	}

	private void OnSelectionChanged()
	{
		QueueDraw();

		if( selection.Width == 1 && selection.Height == 1 ){
			application.PrintStatus( "TileListWidget: Selected tile: " + selection[0, 0] );
		} else if( selection.Width <= 0 || selection.Height <= 0 ){
			application.PrintStatus( "TileListWidget: No tile selected tile." );
		} else {
			application.PrintStatus( "TileListWidget: Multiple tiles selected." );
		}
	}

	protected override void DrawGl()
	{
		if(tileset == null)
			return;
	
		gl.Clear(gl.COLOR_BUFFER_BIT);
		
		int starttile = (int) -Translation.Y / (ROW_HEIGHT) 
						* TILES_PER_ROW;
		Vector pos = new Vector(0,
				(starttile / TILES_PER_ROW) * (ROW_HEIGHT));
		float maxwidth = (TILE_WIDTH + SPACING_X) * TILES_PER_ROW;
		List<int> tiles = tilegroup.Tiles;
		for(int i = starttile; i < tiles.Count; i++) {
			Tile tile = tileset.Get(tiles[i]);

			tile.DrawEditor(pos);

			bool selected = false;
			if(selection.TileListFirstTile > 0
					&& i > selection.TileListFirstTile
					&& i - selection.TileListFirstTile 
						< selection.TileListH * TILES_PER_ROW
					&& i - selection.TileListFirstTile % TILES_PER_ROW 
						< selection.TileListW-1) {
				selected = true;
			} else if(selection.Width == 1 && selection.Height == 1 &&
					tiles[i] == selection[0, 0]) {
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
			if(tilegroup == null)
				return;

			Vector MousePos = new Vector((float) args.Event.X,
	    								 (float) args.Event.Y);
			int tile = PosToTile(MousePos);
			if(tile < 0)
				return;
			
			selection.Resize(1, 1, 0);
			selection[0, 0] = tilegroup.Tiles[tile];
			selection.FireChangedEvent();
			QueueDraw();
		}
	}

	private void OnButtonRelease(object o, ButtonReleaseEventArgs args)
	{
	}

	private void OnMotionNotify(object o, MotionNotifyEventArgs args)
	{
		if(tilegroup == null)
			return;

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
		if(tile >= tilegroup.Tiles.Count)
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

