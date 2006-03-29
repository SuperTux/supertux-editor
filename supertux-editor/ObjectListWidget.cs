using System;
using System.Collections.Generic;
using OpenGl;
using Sprites;
using DataStructures;
	
public class ObjectListWidget : GLWidgetBase
{
	private const int TILE_WIDTH = 32;
	private const int TILE_HEIGHT = 32;
	private const int SPACING_X = 1;
	private const int SPACING_Y = 1;
	private const int ROW_HEIGHT = TILE_HEIGHT + SPACING_Y;
	private const int COLUMN_WIDTH = TILE_WIDTH + SPACING_X;
	private const int TILES_PER_ROW = 4;
	
	private bool objectsLoaded;
	private List<string> GameObjectName = new List<string>();
	private List<Sprite> GameObjectSprite = new List<Sprite>();
	
	public ObjectListWidget()
	{
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
		for( int i = 0; i < GameObjectName.Count; i++ ){
		
			//TODO: draw Object Image at x/y
			Console.Write(GameObjectName[i] + "(" +x+ "/"+y+ ")" );
			try{
				GameObjectSprite[i].Pos.X = x;
				GameObjectSprite[i].Pos.Y = y;
				GameObjectSprite[i].Draw();
			} catch {
				Console.Write(" FAILED");
			}
			Console.WriteLine();
			x += COLUMN_WIDTH;
			if( x >= TILES_PER_ROW * COLUMN_WIDTH ) {
				x = 0;
				y += ROW_HEIGHT;
			}
		}
	}
	
		
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
			if( Icon == null ) { //not a sprite so it has to be an Image.
				try{
					Icon = SpriteManager.CreateFromImage(objectAttribute.IconSprite);
				} catch(Exception) {
					Icon = null;	
				}
			}
			if( Icon == null ) { //no sprite, no image, no can do.
			Console.WriteLine("Can't create an icon for " + objectAttribute.Name
			       + " from " +objectAttribute.IconSprite);
			}
			GameObjectSprite.Add( Icon );
		}

		objectsLoaded = true;
	}
	
	
}
