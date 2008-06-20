//  $Id$
//
//  Copyright (C) 2008 Milos Kloucek <MMlosh@supertuxdev.elektromaniak.cz>
//
//  This program is free software; you can redistribute it and/or
//  modify it under the terms of the GNU General Public License
//  as published by the Free Software Foundation; either version 2
//  of the License, or (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA
//  02111-1307, USA.
using System;
using Drawing;
using OpenGl;
using Sprites;
using DataStructures;
using Gtk;
using Gdk;
using LispReader;
using System.Collections.Generic;
using Undo;


public class BadguyChooserWidget : GLWidgetBase
{
	private const int TILE_WIDTH = 32;
	private const int TILE_HEIGHT = 32;
	private const int SPACING_X = 1;
	private const int SPACING_Y = 1;
	private const int ROW_HEIGHT = TILE_HEIGHT + SPACING_Y;
	private const int COLUMN_WIDTH = TILE_WIDTH + SPACING_X;
	private const int TILES_PER_ROW = 4;
	private const int NONE = -1;

	private List<Sprite> badguySprites = new List<Sprite>();
	private List<String> badguys;
	private int SelectedObjectNr = NONE;
	private int FirstRow = 0;

	public static TargetEntry [] DragTargetEntries = new TargetEntry[] {
		new TargetEntry("GameObject", TargetFlags.App, 0)
	};

	public BadguyChooserWidget(List<String> badguys)
	{
		this.badguys = badguys;

		foreach(string name in badguys){
			Type type = this.GetType().Assembly.GetType(name, false, true); //case-insensitive

			if (type == null) {
				LogManager.Log(LogLevel.Warning, "No type found for " + name);
				continue;
			}

			SupertuxObjectAttribute objectAttribute	= (SupertuxObjectAttribute) Attribute.GetCustomAttribute(type, typeof(SupertuxObjectAttribute));

			if(objectAttribute == null) {
				LogManager.Log(LogLevel.Warning, "No objectAttribute found for " + name);
				continue;
			}

			Sprite sprite = CreateSprite(objectAttribute.IconSprite, objectAttribute.ObjectListAction);

			if(sprite == null) {
				LogManager.Log(LogLevel.Warning, "No sprite found for " + name);
				continue;
			}

			badguySprites.Add(sprite);
		}

		SetSizeRequest( -1, ROW_HEIGHT);

		ButtonPressEvent += OnButtonPress;
		AddEvents((int) Gdk.EventMask.ButtonPressMask);
		AddEvents((int) Gdk.EventMask.AllEventsMask);

		Gtk.Drag.SourceSet (this, Gdk.ModifierType.Button1Mask,
		                    DragTargetEntries, DragAction.Default);

		DragBegin += OnDragBegin;
	}

	/// <summary>Redraw Widget</summary>
	protected override void DrawGl()
	{

		gl.Clear(gl.COLOR_BUFFER_BIT);
		int x = 0;
		int y = 0;
		float scalex = 1;
		float scaley = 1;
		Sprite objectSprite = null;
		for( int i = 0 + FirstRow * TILES_PER_ROW; i < badguys.Count; i++ ){
			objectSprite = badguySprites[i];
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

	private void OnButtonPress(object o, ButtonPressEventArgs args)
	{
		if(args.Event.Button == 1) {
			Vector MousePos = new Vector((float) args.Event.X,
			                             (float) args.Event.Y);
			int row = FirstRow + (int) Math.Floor( MousePos.Y / ROW_HEIGHT );
			int column = (int) Math.Floor (MousePos.X / COLUMN_WIDTH);
			if( column >= TILES_PER_ROW ){
				return;
			}
			int selected = TILES_PER_ROW * row + column;
			if( selected  < badguys.Count ){
				if( SelectedObjectNr != selected ){
					SelectedObjectNr = selected;
					//find type by name, case-unsensitive
					QueueDraw();
				}
			}
		}
	}

	private void OnDragBegin(object o, DragBeginArgs args)
	{
		LogManager.Log(LogLevel.Debug, "Dragstart");
	}
}

/// <summary>
/// Creator for widget editing Dispenser.badguy field.
/// </summary>
public sealed class ChooseBadguyWidget : CustomSettingsWidget
{
	private Entry entry;
	private bool changed = false;

	public override Widget Create(object caller)
	{
		VBox vBox = new VBox(false, 3);

		List<string> val = (List<string>) field.GetValue(Object);

		entry = new Entry();
		BadguyChooserWidget editor = new BadguyChooserWidget(val);

		string String = "";
		for (int i = 0; i < val.Count; i++) {
			String += val[i];
			if (i < val.Count -1) 
				String += ", ";
		}
		entry.Text = String; 

		entry.Changed += OnBadguyChanged;
		entry.FocusOutEvent += OnBadguyChangeDone;

		vBox.PackEnd(entry, true, true, 0);
		vBox.PackEnd(editor, true, true, 0);

		// Create a tooltip if we can.
		CreateToolTip(caller, vBox);

		vBox.Name = field.Name;

		return vBox;
	}

	private void OnBadguyChanged(object sender, EventArgs args)
	{
		changed = true;
	}

	private void OnBadguyChangeDone(object sender, EventArgs args)
	{
		if (!changed) return; 
		changed = false;
		List<string> bad = new List<string>();
		string String1 = entry.Text;
		string String2 = "";

		while (String1.IndexOf(", ")>-1) {	//at least two not yet added items
			String2 = String1.Substring(0, String1.IndexOf(", "));
			String1 = String1.Substring(String1.IndexOf(", ")+2, String1.Length-String1.IndexOf(", ")-2);
			bad.Add(String2);
		}

		if (String1.Length>0){
			bad.Add(String1);

		}

		PropertyChangeCommand command = new PropertyChangeCommand(
			"Changed " + field.Name,
			field,
			Object,
			bad);
		command.Do();
		UndoManager.AddCommand(command);
	}
}

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property,
                AllowMultiple=false)]
public sealed class ChooseBadguySettingAttribute : CustomSettingsWidgetAttribute
{
	public ChooseBadguySettingAttribute() : base(typeof(ChooseBadguyWidget))
	{
	}
}
