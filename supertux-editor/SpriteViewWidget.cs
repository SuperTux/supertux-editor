//  $Id$
using OpenGl;
using Sprites;
using Gtk;

/// <summary>Simple class that draws one sprite and can embed itself in a window</summary>
public class SpriteViewWidget : GLWidgetBase
{
	private Sprite sprite;

	/// <summary>Returns SpriteViewWidget inserted in a window</summary>
	public static Window CreateWindow(Sprite sprite)
	{
		Gtk.Window window = new Window(WindowType.Popup);
		SpriteViewWidget widget = new SpriteViewWidget(sprite);
		window.SetSizeRequest( (int)sprite.Width, (int)sprite.Height);
		window.Add(widget);
		window.ShowAll();
		return window;		
	}

	public SpriteViewWidget(Sprite sprite)
	{
		this.sprite = sprite;
		SetSizeRequest( (int)sprite.Width, (int)sprite.Height);
	}
	
	/// <summary>Redraw Widget</summary>
	protected override void DrawGl()
	{
		gl.Clear(gl.COLOR_BUFFER_BIT);
		if( sprite != null ){
			sprite.Draw(sprite.Offset);
		}
	}
}
