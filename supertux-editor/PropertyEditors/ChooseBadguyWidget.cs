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

	private static Dictionary<string, Sprite> badguySprites = new Dictionary<string, Sprite>();
	private List<string> badguys;
	private string draggedBadguy = "";
	private int draggedIndex;
	private int SelectedObjectNr = NONE;
	private int FirstRow = 0;

	public static TargetEntry [] DragTargetEntries = new TargetEntry[] {
		new TargetEntry("GameObject", TargetFlags.App, 0)
	};

	public BadguyChooserWidget(List<string> badguys)
	{
		this.badguys = badguys;

		foreach(string name in badguys){

			if(!badguySprites.ContainsKey(name)) {
				badguySprites.Add(name, CrateSprite(this, name));
			}
		}

		SetSizeRequest( -1, ROW_HEIGHT);

		ButtonPressEvent += OnButtonPress;
//		ButtonReleaseEvent += OnButtonRelease;
//		MotionNotifyEvent += OnMotionNotify;
		AddEvents((int) Gdk.EventMask.AllEventsMask);

		Gtk.Drag.SourceSet (this, Gdk.ModifierType.Button1Mask,
		                    DragTargetEntries, DragAction.Default);

		Gtk.Drag.DestSet (this, Gtk.DestDefaults.All,
		                    DragTargetEntries, DragAction.Default);

		DragBegin += OnDragBegin;
		DragMotion += OnDragMotion;
		DragEnd += OnDragEnd;
	}

	/// <summary>Redraw Widget</summary>
	protected override void DrawGl()
	{

//		gl.ClearColor(1,1,1,1);		//possible clear with other color
		gl.Clear(gl.COLOR_BUFFER_BIT);
//		gl.ClearColor(0,0,0,1);
		int x = 0;
		int y = 0;
		float scalex = 1;
		float scaley = 1;
		Sprite objectSprite = null;
		for( int i = 0 + FirstRow * TILES_PER_ROW; i < badguys.Count; i++ ){
			objectSprite = badguySprites[badguys[i]];
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

	private static Sprite CrateSprite(BadguyChooserWidget widgetInstance, string classname)
	{
		Type type = widgetInstance.GetType().Assembly.GetType(classname, false, true); //case-insensitive search

		if (type == null) {
			LogManager.Log(LogLevel.Warning, "No type found for " + classname);
			return null;
		}

		SupertuxObjectAttribute objectAttribute	= (SupertuxObjectAttribute) Attribute.GetCustomAttribute(type, typeof(SupertuxObjectAttribute));

		if(objectAttribute == null) {
			LogManager.Log(LogLevel.Warning, "No objectAttribute found for " + classname);
			return null;
		}

		Sprite sprite = CreateSprite(objectAttribute.IconSprite, objectAttribute.ObjectListAction);

		if(sprite == null) {
			LogManager.Log(LogLevel.Warning, "No sprite found for " + classname);
			return null;
		}
		return sprite;
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
//		if(args.Event.Button == 3 && dragging) {
//			dragging = false;
//			badguySprites.Insert(draggedIndex, draggedSprite);	
//			badguys.Insert(draggedIndex, draggedBadguy);	
//		}
	}

//	private void OnButtonRelease(object o, ButtonReleaseEventArgs args)
//	{
//		LogManager.Log(LogLevel.Debug, "Mouse button released");
//		if(args.Event.Button == 1) {
//			dragging = false;
//		}
//	}

//	private void OnMotionNotify(object o, MotionNotifyEventArgs args)
//	{
//
//		if (args.Event.State.CompareTo(ModifierType.Button1Mask) > -1){
//			LogManager.Log(LogLevel.Debug, "Mouse moved with button 1 pressed, X: " + args.Event.X + ", Y: " + args.Event.Y);
//		}
//	}

	private void OnDragBegin(object o, DragBeginArgs args)
	{
		LogManager.Log(LogLevel.Debug, "Dragstart of ID " + SelectedObjectNr.ToString());// + DragBeginArgs.Context.);
		if (SelectedObjectNr > -1){
			draggedIndex = SelectedObjectNr;
			draggedBadguy = badguys[SelectedObjectNr];
			badguys.RemoveAt(SelectedObjectNr);
		}
	}

	private void OnDragMotion(object o, DragMotionArgs args)
	{
		LogManager.Log(LogLevel.Debug, "Drag motion, X: " + args.X + ", Y: " + args.Y);// + DragBeginArgs.Context.);
		Vector MousePos = new Vector((float) args.X, (float) args.Y);
		int row = FirstRow + (int) Math.Floor( MousePos.Y / ROW_HEIGHT );
		int column = (int) Math.Floor (MousePos.X / COLUMN_WIDTH);
		if( column >= TILES_PER_ROW ){
			SelectedObjectNr = NONE;
			return;
		}
		int selected = TILES_PER_ROW * row + column;
		if( selected  < badguys.Count ){
			if( SelectedObjectNr != selected ){
				SelectedObjectNr = selected;
				//find type by name, case-unsensitive
				QueueDraw();
			} else {
				SelectedObjectNr = NONE;
			}
		}	
	}

	private void OnDragEnd(object o, DragEndArgs args)
	{
		LogManager.Log(LogLevel.Debug, "Dragstop");
		if (draggedBadguy != ""){
			if (SelectedObjectNr == NONE)
				badguys.Add(draggedBadguy);
			else
				badguys.Insert(SelectedObjectNr, draggedBadguy);
			draggedBadguy = "";
		}
//		Gtk.Drag.Finish(Gdk.DragContext, bool, bool, uint);
//		GetSourceWidget(Gdk.DragContext) : Widget
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
