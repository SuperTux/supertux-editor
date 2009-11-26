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

/* EOF */
