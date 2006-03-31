using System;
using System.Collections.Generic;
using OpenGl;
using Sprites;
using DataStructures;
using Gtk;

public class ObjectListWidget : GLWidgetBase
{
	private const int TILE_WIDTH = 32;
	private const int TILE_HEIGHT = 32;
	private const int SPACING_X = 1;
	private const int SPACING_Y = 1;
	private const int ROW_HEIGHT = TILE_HEIGHT + SPACING_Y;
	private const int COLUMN_WIDTH = TILE_WIDTH + SPACING_X;
	private const int TILES_PER_ROW = 4;
	private const int NONE = -1;
	
	private bool objectsLoaded;
	private List<string> GameObjectName = new List<string>();
	private List<Sprite> GameObjectSprite = new List<Sprite>();
	private int SelectedObjectNr = NONE;
	
	public ObjectListWidget()
	{
		SetSizeRequest( COLUMN_WIDTH * TILES_PER_ROW, -1);
		
		ButtonPressEvent += OnButtonPress;
		AddEvents((int) Gdk.EventMask.ButtonPressMask);
	}
	
	/*
	*   Redraw Widget
	*/
	protected override void DrawGl()
	{
		LoadObjectImages();
		
		gl.Clear(gl.COLOR_BUFFER_BIT);
		int x = 0;
		int y = 0;
		float scalex = 1;
		float scaley = 1;
		Sprite ObjectSprite = null;
		for( int i = 0; i < GameObjectName.Count; i++ ){
			ObjectSprite = GameObjectSprite[i];
			//Draw Image
			if( ObjectSprite != null ){
				gl.PushMatrix();
				//Adjust Size
				scalex = scaley = 1;
				if( ObjectSprite.Width > TILE_WIDTH ) {
					scalex = TILE_WIDTH / ObjectSprite.Width;
				}
				if( ObjectSprite.Height > TILE_HEIGHT ){
					scaley = TILE_HEIGHT / ObjectSprite.Height;
				}
				//keep aspect ratio
				if( scalex < scaley ) {
					scaley = scalex;
				} else {
					scalex = scaley;	
				}
				
				gl.Scalef( scalex, scaley, 1 );	
				ObjectSprite.Pos.X = x / scalex;
				ObjectSprite.Pos.Y = y / scaley;				
				ObjectSprite.Draw();
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
	
	/*
	* Loading Images need Gl context so this has to be called from DrawGl
	*/
	private void LoadObjectImages()
	{
		if(objectsLoaded)
			return;
		
		foreach(Type type in this.GetType().Assembly.GetTypes()) {
			SupertuxObjectAttribute objectAttribute
			= (SupertuxObjectAttribute) Attribute.GetCustomAttribute(type, typeof(SupertuxObjectAttribute));
			if(objectAttribute == null)
				continue;
			//this should give us all objects
			GameObjectName.Add( objectAttribute.Name );
			Sprite Icon;
			//might be a sprite
			try{
				Icon = SpriteManager.Create(objectAttribute.IconSprite);
			} catch(Exception) {
				Icon = null;	
			}
			if( Icon != null ){ //Try to find a nice action.
				try { Icon.Action = "left"; }
				catch { try { Icon.Action = "normal"; }
					catch { try { Icon.Action = "default"; }
						catch {
							// Console.WriteLine("ObjectListWidget: No action selected for " + objectAttribute.Name );
						}
					}
				}
			} else { //not a sprite so it has to be an Image.
				try{
					Icon = SpriteManager.CreateFromImage(objectAttribute.IconSprite);
				} catch(Exception) {
					Icon = null;	
				}
			}
			if( Icon == null ) { //no sprite, no image, no can do.
			Console.WriteLine("ObjectListWidget: Can't create an icon for " + objectAttribute.Name
			       + " from " +objectAttribute.IconSprite);
			}
			GameObjectSprite.Add( Icon );
		}

		objectsLoaded = true;
	}
	
	private void OnButtonPress(object o, ButtonPressEventArgs args)
	{
		if(args.Event.Button == 1) {
			Vector MousePos = new Vector((float) args.Event.X,
	    								 (float) args.Event.Y);	
	    	int row = (int) Math.Floor( MousePos.Y / ROW_HEIGHT ); 
	    	int column = (int) Math.Floor (MousePos.X / COLUMN_WIDTH);
	    	if( column >= TILES_PER_ROW ){
	    		return;	
	    	}
	    	int selected = TILES_PER_ROW * row + column;
			if( selected  < GameObjectName.Count ){
				if( SelectedObjectNr != selected ){
					SelectedObjectNr = selected;
					//TODO: tell the rest of te application
					Console.WriteLine("ObjectListWidget: selection changed to " + GameObjectName[ selected ] );
					QueueDraw();
				}
			}
		}
	}
}
