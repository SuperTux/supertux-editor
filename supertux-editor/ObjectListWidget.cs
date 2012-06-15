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
using System.Collections.Generic;
using OpenGl;
using Sprites;
using DataStructures;
using Gtk;
using Gdk;

public class ObjectListWidget : GLWidgetBase
{
	private const int TILE_WIDTH = 32;
	private const int TILE_HEIGHT = 32;
	private const int SPACING_X = 1;
	private const int SPACING_Y = 1;
	private const int ROW_HEIGHT = TILE_HEIGHT + SPACING_Y;
	private const int COLUMN_WIDTH = TILE_WIDTH + SPACING_X;
	private const int NONE = -1;

	private int TILES_PER_ROW = 4;
	private int ROWS_PER_PAGE = 10;
	private bool objectsLoaded;
	private List<Type> gameObjectTypes = new List<Type>();
	private List<Sprite> gameObjectSprites = new List<Sprite>();
	private int SelectedObjectNr = NONE;
	private int FirstRow = 0;
	private Application application;
	private Level level;
	private Adjustment vadjustment;
	private int LastObjectHovered;

	public static TargetEntry [] DragTargetEntries = new TargetEntry[] {
		new TargetEntry("GameObject", TargetFlags.App, 0),
		new TargetEntry("BadguyName", TargetFlags.App, 1)
	};

	public ObjectListWidget(Application application, Adjustment adjv)
	{
		this.application = application;

		SetSizeRequest( COLUMN_WIDTH * TILES_PER_ROW, -1);

		ButtonPressEvent += OnButtonPress;
		AddEvents((int) Gdk.EventMask.ButtonPressMask);
		AddEvents((int) Gdk.EventMask.AllEventsMask);
		AddEvents((int) Gdk.EventMask.ScrollMask);

		Gtk.Drag.SourceSet (this, Gdk.ModifierType.Button1Mask,
		                    DragTargetEntries, DragAction.Copy | DragAction.Default);

		SizeAllocated += OnSizeAllocated;

		DragBegin += OnDragBegin;
		DragEnd += OnDragEnd;
		ScrollEvent += OnScroll;
		DragDataGet += OnDragDataGet;
		application.LevelChanged += OnLevelChanged;
		vadjustment = adjv;
		vadjustment.ValueChanged += OnVAdjustmentChangedValue;
		HasTooltip = true;
		QueryTooltip += OnQueryTooltip;
	}

	/// <summary>Redraw Widget</summary>
	protected override void DrawGl()
	{
		LoadObjectImages();

		gl.Clear(gl.COLOR_BUFFER_BIT);
		int x = 0;
		int y = 0;
		float scalex = 1;
		float scaley = 1;
		Sprite objectSprite = null;
		for( int i = 0 + FirstRow * TILES_PER_ROW; i < gameObjectTypes.Count; i++ ){
			objectSprite = gameObjectSprites[i];
			//Draw Image
			if( objectSprite != null ){
				gl.PushMatrix();
				//Adjust Size
				scalex = scaley = 1;
				if( objectSprite.Width > TILE_WIDTH ) {
					scalex = TILE_WIDTH / objectSprite.Width;
				}
				if( objectSprite.Height > TILE_HEIGHT ){
					scaley = TILE_HEIGHT / objectSprite.Height;
				}
				//keep aspect ratio
				if( scalex < scaley ) {
					scaley = scalex;
				} else {
					scalex = scaley;
				}

				gl.Translatef(x, y, 0);
				gl.Scalef( scalex, scaley, 1 );
				objectSprite.Draw(objectSprite.Offset);
				gl.PopMatrix();
			}
			//mark the selected object
			if( i == SelectedObjectNr ){
				gl.Color4f(0, 1, 1, 0.4f);
				gl.Disable(gl.TEXTURE_2D);
				gl.Begin(gl.QUADS);
					gl.Vertex2f( x, y );
					gl.Vertex2f( x + TILE_WIDTH, y );
					gl.Vertex2f( x + TILE_WIDTH, y + TILE_HEIGHT);
					gl.Vertex2f( x, y + TILE_HEIGHT);
				gl.End();
				gl.Enable(gl.TEXTURE_2D);
				gl.Color4f(1, 1, 1, 1);
			}

			x += COLUMN_WIDTH;
			if( x >= TILES_PER_ROW * COLUMN_WIDTH ) {
				x = 0;
				y += ROW_HEIGHT;
			}
		}
	}

	/// <summary>Create object list</summary>
	/// <remarks>Loading Images need Gl context so this has to be called from DrawGl</remarks>
	private void LoadObjectImages()
	{
		if(objectsLoaded)
			return;

		// Reinitialize
		gameObjectTypes.Clear();
		gameObjectSprites.Clear();

		// The null object (arrow)
		gameObjectTypes.Add(null);
		gameObjectSprites.Add(CreateSprite("images/engine/editor/arrow.png", null));

		foreach(Type type in this.GetType().Assembly.GetTypes()) {
			SupertuxObjectAttribute objectAttribute
				= (SupertuxObjectAttribute) Attribute.GetCustomAttribute(type, typeof(SupertuxObjectAttribute));
			if(objectAttribute == null)
				continue;

			if (objectAttribute.Target == SupertuxObjectAttribute.Usage.None)
				continue;

			// We load all objects if no level is loaded to avoid crash
			// when accessing the level object (as that is null
			// when no level is loaded).
			if (this.level != null) {
				if ( (objectAttribute.Target == SupertuxObjectAttribute.Usage.WorldmapOnly) &&
				     (!level.isWorldmap) ) {
					continue;
				} else if ( (objectAttribute.Target == SupertuxObjectAttribute.Usage.LevelOnly) &&
				            (level.isWorldmap) ) {
					continue;
				}
			}

			Sprite icon = CreateSprite(objectAttribute.IconSprite, objectAttribute.ObjectListAction);
			if( icon == null ) { //no sprite, no image, no can do.
				LogManager.Log(LogLevel.Warning, "ObjectListWidget: Can't create an icon for " + objectAttribute.Name
				                     + " from " +objectAttribute.IconSprite);
			} else {
				gameObjectTypes.Add(type);
				gameObjectSprites.Add(icon);
			}
		}

		objectsLoaded = true;
		updateScrollbar();
	}

	private static Sprite CreateSprite(string name, string action)
	{
		Sprite result = null;

		// Might be a sprite
		try{
			result = SpriteManager.Create(name);
		} catch {
		}

		if( result != null ){ // Try to find a nice action.
			// Check if we were passed an action to use and if not set it to left.
			if (String.IsNullOrEmpty(action))
				action = "left";
			try { result.Action = action; }
			catch { try { result.Action = "normal"; }
				catch { try { result.Action = "default"; }
					catch {
						LogManager.Log(LogLevel.DebugWarning, "ObjectListWidget: No action selected for " + name);
					}
				}
			}
		} else { // Not a sprite so it has to be an Image.
			try{
				result = SpriteManager.CreateFromImage(name);
			} catch(Exception) {
				result = null;
			}
		}

		return result;
	}

	/// <summary>Called when a new level is loaded</summary>
	private void OnLevelChanged(Level level)
	{
		if(this.level != null)
			this.level.TilesetChanged -= OnTilesetChanged;
		if(level != null)
			level.TilesetChanged += OnTilesetChanged;
		this.level = level;
		OnTilesetChanged(level);
	}

	/// <summary>Called when tileset changes.</summary>
	private void OnTilesetChanged(Level level)
	{
		// Force a reload of object list (so correct objects are loaded).
		objectsLoaded = false;
		QueueDraw();
	}

	private void OnButtonPress(object o, ButtonPressEventArgs args)
	{
		if(args.Event.Button == 1) {
			application.SetToolObjects();

			Vector MousePos = new Vector((float) args.Event.X,
			                             (float) args.Event.Y);
			int? objectNr = GetObjectNrByPosition(MousePos);
			int selected;
			if (!objectNr.HasValue) {
				return ;
			} else {
				selected = (int)objectNr;
			}
			if( selected  < gameObjectTypes.Count ){
				if( SelectedObjectNr != selected ){
					SelectedObjectNr = selected;
					if( application.CurrentSector != null ) {
						Type type = gameObjectTypes[selected];
						Sprite Icon = gameObjectSprites[selected];
						if(type != null) {
							ITool editor = new ObjectCreationTool(application, application.CurrentSector, type, Icon);
							application.SetTool(editor);
							application.PrintStatus("ObjectListWidget: last selected \"" + gameObjectTypes[selected].Name +"\"");
						} else {
							ITool editor = new ObjectSelectTool(application, application.CurrentSector);
							application.SetTool(editor);
							application.PrintStatus("ObjectListWidget: none selected ");
						}
					}
					QueueDraw();
				}
			}
		}
	}
	
	private int? GetObjectNrByPosition (Vector MousePos)
	{
		int row = FirstRow + (int) Math.Floor( MousePos.Y / ROW_HEIGHT );
		int column = (int) Math.Floor (MousePos.X / COLUMN_WIDTH);
		if( column >= TILES_PER_ROW ){
			return null;
		} else {
		 return TILES_PER_ROW * row + column;
		}
	}
	
	private void OnQueryTooltip (object o, QueryTooltipArgs args)
	{
		int? objectNr = GetObjectNrByPosition(new Vector(
				(float)args.X, 
				(float)args.Y));
		if (objectNr.HasValue) {
			int selected = (int)objectNr;
			if (selected != LastObjectHovered) {
				//HACK: disable tooltip if hovered object changed
				LastObjectHovered = selected;
				return;
			}
			args.Tooltip.Text = "";
			if (selected < gameObjectTypes.Count) {
				Type type = gameObjectTypes[selected];
				if (type != null) {
					args.Tooltip.Text = type.Name;
					args.RetVal = true;
				}
			}
		}
	}

	private void OnDragBegin(object o, DragBeginArgs args)
	{
		Gtk.Drag.SetIconWidget( args.Context, SpriteViewWidget.CreateWindow(gameObjectSprites[SelectedObjectNr]), -15, -15);
		LogManager.Log(LogLevel.Debug, "Dragstart");
	}

	private void OnDragEnd(object o, DragEndArgs args)
	{
		SelectedObjectNr = NONE;
		QueueDraw();
	}

	private void OnDragDataGet (object o, DragDataGetArgs args)
	{

		if (SelectedObjectNr != NONE){
			Atom[] Targets = args.Context.Targets;

			foreach (Atom target in Targets){
				if (target.Name == "BadguyName") {
					if (SelectedObjectNr != 0) { //HACK: don't try to send the name of the arrow (null)
	                    //TODO: Send only badguys into dispensers, no Doors, no Tilemaps..
						args.SelectionData.Set (target, 8, System.Text.Encoding.UTF8.GetBytes (gameObjectTypes[SelectedObjectNr].Name.ToLower()));
	              	}
				}
				//if (target.Name == "GameObject")
					//no data transmitted
			}
		}
	}

	private void OnScroll(object o, ScrollEventArgs args)
	{
		if(args.Event.Direction == ScrollDirection.Up && FirstRow > 0 ) {
			FirstRow -= 1;
			args.RetVal = true;
			vadjustment.Value = FirstRow;
			QueueDraw();
		} else if( args.Event.Direction == ScrollDirection.Down &&
			         FirstRow + ROWS_PER_PAGE -1 < Math.Floor( (double)gameObjectTypes.Count / (double)TILES_PER_ROW )) {
			FirstRow += 1;
			args.RetVal = true;
			vadjustment.Value = FirstRow;
			QueueDraw();
		}
	}

	/// <summary>Vertical Scroll Bar was scrolled</summary>
	private void OnVAdjustmentChangedValue(object sender, EventArgs e)
	{
		FirstRow = (int) vadjustment.Value;
		QueueDraw();
	}

	/// <summary>Calculate TILES_PER_ROW, when we know how wide we are</summary>
	private void OnSizeAllocated  (object o, SizeAllocatedArgs args)
	{
		TILES_PER_ROW = args.Allocation.Width /  COLUMN_WIDTH;
		ROWS_PER_PAGE = args.Allocation.Height / ROW_HEIGHT;
		updateScrollbar();
	}

	/// <summary>adjust the scrollbar</summary>
	private void updateScrollbar()
	{
		double upper = Math.Ceiling( (double)gameObjectTypes.Count / (double)TILES_PER_ROW );
		vadjustment.SetBounds(0, upper, 1, ROWS_PER_PAGE-1, ROWS_PER_PAGE);
	}
}

/* EOF */
