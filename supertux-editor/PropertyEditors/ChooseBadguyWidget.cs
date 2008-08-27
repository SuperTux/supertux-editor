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
	private const int BORDER_LEFT = 6;
	private const int BORDER_RIGHT = 6;
	private const int ROW_HEIGHT = TILE_HEIGHT + SPACING_Y;
	private const int COLUMN_WIDTH = TILE_WIDTH + SPACING_X;
	private const int NONE = -1;

	private FieldOrProperty field;
	private object _object;
	private int TILES_PER_ROW = 4;
	private static Dictionary<string, Sprite> badguySprites = new Dictionary<string, Sprite>();
	private List<string> badguys;
	private string draggedBadguy = "";
	private bool dragging = false;
	private int draggedID = NONE;
	private bool insertOnEnd = false;
	private int SelectedObjectNr = NONE;

	//do not allow dragging our badguys away from this editor
	public static TargetEntry [] source_table = new TargetEntry[] {
		new TargetEntry("BadguyName", TargetFlags.Widget, 0)
	};

	//allow incomming badguys from any widget in this application
	public static TargetEntry [] target_table = new TargetEntry[] {
		new TargetEntry("BadguyName", TargetFlags.App, 0)
	};

	public BadguyChooserWidget(FieldOrProperty field, object _object)
	{
		this.field = field;
		this._object = _object;
		//HACK: two "this." on the following two lines are there only for hiding 2 compilation warnings and can be removed after implementation of undo mechanism.
		this.Name = this.field.Name;
		badguys = (List<string>)field.GetValue(this._object);

		foreach(string name in badguys){	//process each badguy name and crate sprite for it

			if(!badguySprites.ContainsKey(name)) {
				badguySprites.Add(name, CrateSprite(name));
			}
		}

		SetSizeRequest( -1, ROW_HEIGHT * ((badguys.Count  - 1) / TILES_PER_ROW + 1));

		ButtonPressEvent += OnButtonPress;
		AddEvents((int) Gdk.EventMask.ButtonPressMask);

		if (badguys.Count > 0)		//we need at least one badguy for dragging
			Gtk.Drag.SourceSet (this, Gdk.ModifierType.Button1Mask,
		                    source_table, DragAction.Move);

		Gtk.Drag.DestSet (this, Gtk.DestDefaults.All,
		                    target_table, DragAction.Move | DragAction.Copy);

		DragBegin += OnDragBegin;
		DragMotion += OnDragMotion;
		DragEnd += OnDragEnd;

		DragDataReceived += OnDragDataReceived;
		DragDataGet += OnDragDataGet;
		DragLeave += OnDragLeave;

		SizeAllocated += OnSizeAllocated;
	}

	/// <summary>Redraw Widget</summary>
	protected override void DrawGl()
	{

//		gl.ClearColor(1,1,1,1);		//possible clear with other color
		gl.Clear(gl.COLOR_BUFFER_BIT);
//		gl.ClearColor(0,0,0,1);
		int x = BORDER_LEFT;
		int y = 0;
		float scalex = 1;
		float scaley = 1;
		Sprite objectSprite = null;
		for( int i = 0; i < badguys.Count; i++ ){
			objectSprite = badguySprites[badguys[i]];	//find sprite in the dictionary
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

				gl.Translatef(x, y, 0);
				gl.Scalef( scalex, scaley, 1 );
				objectSprite.Draw(objectSprite.Offset);
				gl.PopMatrix();
			}
			//mark the selected badguy
			if( i == SelectedObjectNr && !dragging && draggedID == NONE){
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

			//mark currently dragged badguy
			if( i == draggedID){
				gl.Color4f(0, 0, 0, 0.6f);
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
				x = BORDER_LEFT;
				y += ROW_HEIGHT;
			}
		}

		//draw insert mark if dragging
		if (dragging){
			int offset_x = SelectedObjectNr % TILES_PER_ROW * COLUMN_WIDTH + BORDER_LEFT;

			int offset_y = SelectedObjectNr / TILES_PER_ROW * ROW_HEIGHT;

			if (insertOnEnd) {
				//compensations for drawing sign on end of the line
				offset_x += TILES_PER_ROW * COLUMN_WIDTH;
				offset_y -= ROW_HEIGHT;
			}

			gl.Color4f(1, 1, 1, 1);
			gl.Disable(gl.TEXTURE_2D);
			gl.LineWidth(3);
			gl.Begin(gl.LINES);
				gl.Vertex2f( offset_x - 5, offset_y + 4);
				gl.Vertex2f( offset_x + 5, offset_y + 4);
				gl.Vertex2f( offset_x    , offset_y + 4);
				gl.Vertex2f( offset_x    , offset_y + TILE_HEIGHT - 4);
				gl.Vertex2f( offset_x + 5, offset_y + TILE_HEIGHT - 4);
				gl.Vertex2f( offset_x - 5, offset_y + TILE_HEIGHT - 4);
			gl.End();
			gl.LineWidth(1);
			gl.Enable(gl.TEXTURE_2D);
			gl.Color4f(1, 1, 1, 1);
		}
	}

	/// <summary>Create sprite from name of badguy's class</summary>
	private Sprite CrateSprite(string classname)
	{
		Type type = this.GetType().Assembly.GetType(classname, false, true); //case-insensitive search

		if (type == null) {
			LogManager.Log(LogLevel.Warning, "No type found for " + classname);
			return null;
		}

		SupertuxObjectAttribute objectAttribute	= (SupertuxObjectAttribute) Attribute.GetCustomAttribute(type, typeof(SupertuxObjectAttribute));

		if(objectAttribute == null) {
			LogManager.Log(LogLevel.Warning, "No objectAttribute found for " + classname);
			return null;
		}

		Sprite result = null;

		// Might be a sprite
		try{
			result = SpriteManager.Create(objectAttribute.IconSprite);
		} catch {
		}

		if( result != null ){ // Try to find a nice action.

			try { result.Action =objectAttribute.ObjectListAction; }
			catch { try { result.Action = "left"; }
				catch { try { result.Action = "normal"; }
					catch { try { result.Action = "default"; }
						catch {
							LogManager.Log(LogLevel.DebugWarning, "BadguyChooserWidget: No action selected for " + objectAttribute.IconSprite);
						}
					}
				}
			}
		} else { // Not a sprite so it has to be an Image.
			try{
				result = SpriteManager.CreateFromImage(objectAttribute.IconSprite);
			} catch(Exception) {
				result = null;
			}
		}

		if(result == null) {
			LogManager.Log(LogLevel.Warning, "No editor image found for " + classname);
			return null;
		}

		return result;
	}

	private void OnButtonPress(object o, ButtonPressEventArgs args)
	{
		if(args.Event.Button == 1) {
			Vector MousePos = new Vector((float) args.Event.X - BORDER_LEFT,
			                             (float) args.Event.Y);
			int row = (int) Math.Floor( MousePos.Y / ROW_HEIGHT );
			int column = (int) Math.Floor (MousePos.X / COLUMN_WIDTH);
			if( column >= TILES_PER_ROW ){
				return;
			}
			int selected = TILES_PER_ROW * row + column;
			if( selected  < badguys.Count ){
				if( SelectedObjectNr != selected ){
					SelectedObjectNr = selected;
					QueueDraw();
				}
			}
		}
	}

	private void OnDragBegin(object o, DragBeginArgs args)
	{
		//TODO: set dragged icon here

		if (SelectedObjectNr > -1){
			draggedID = SelectedObjectNr;
			draggedBadguy = badguys[SelectedObjectNr];

			dragging = true;
		}
		//update heigth
		SetSizeRequest( -1, ROW_HEIGHT * ((badguys.Count - 1) / TILES_PER_ROW + 1));
		LogManager.Log(LogLevel.Debug, "Dragstart of " + draggedBadguy);
	}

	private void OnDragMotion(object o, DragMotionArgs args)
	{
		dragging = true;

		Vector MousePos = new Vector((float) args.X - BORDER_LEFT + TILE_WIDTH / 2, (float) args.Y);
		int row = (int) Math.Floor( MousePos.Y / ROW_HEIGHT );
		int column = (int) Math.Floor (MousePos.X / COLUMN_WIDTH);
		insertOnEnd = ( column >= TILES_PER_ROW );	//This is true on every end of line
		int selected = TILES_PER_ROW * row + column;
		if( selected  > badguys.Count )
			selected = badguys.Count;
		if( SelectedObjectNr != selected ){
			SelectedObjectNr = selected;
			QueueDraw();		//redraw on any change of selected ID
		}
	}

	private void OnDragEnd (object o, DragEndArgs args)
	{
		if (draggedID > NONE) {		//Widget.DragFailed is not aviable for windows users
			Command command = new SortedListRemoveCommand<string>("Removed badguy \"" + draggedBadguy + "\" from the queue", badguys, draggedID);
			command.Do();
			UndoManager.AddCommand(command);

			draggedID = NONE;
			draggedBadguy = "";
			dragging = false;
			if (badguys.Count == 0)
				Gtk.Drag.SourceUnset(this);
		}
	}

	private void OnDragDataReceived(object o, DragDataReceivedArgs args)
	{
		string data = System.Text.Encoding.UTF8.GetString (args.SelectionData.Data);

		if(!badguySprites.ContainsKey(data)) {
				badguySprites.Add(data, CrateSprite(data));
			}

		if (data != ""){
			if (badguys.Count == 0)
				Gtk.Drag.SourceSet (this, Gdk.ModifierType.Button1Mask,
		                    source_table, DragAction.Move);

			Command command;
			if (draggedID > NONE){		//We were moving
				if (SelectedObjectNr > draggedID)
					SelectedObjectNr--;
				if (SelectedObjectNr == draggedID) {
					draggedID = NONE;
					return;
				}
				command = new SortedListMoveCommand<string>("Changed position of badguy  \"" + data + "\" in the queue", badguys, draggedID, SelectedObjectNr);
				draggedID = NONE;
			} else				//We were adding
				command = new SortedListAddCommand<string>("Added badguy \"" + data + "\" into the queue", badguys, data, SelectedObjectNr);
			command.Do();
			UndoManager.AddCommand(command);

			dragging = false;

			//update heigth
			SetSizeRequest( -1, ROW_HEIGHT * ((badguys.Count - 1) / TILES_PER_ROW + 1));
		}


		Gtk.Drag.Finish (args.Context, true, false, args.Time);
	}

	private void OnDragDataGet (object o, DragDataGetArgs args)
	{
		Atom[] Targets = args.Context.Targets;

		args.SelectionData.Set (Targets[0], 8, System.Text.Encoding.UTF8.GetBytes (draggedBadguy));
		draggedBadguy = "";	//badguy was succesfully moved
	}

	private void OnDragLeave (object o, DragLeaveArgs args)
	{
		dragging = false;
	}

	/// <summary>Calculate TILES_PER_ROW and heigth, when we know how long we are</summary>
	private void OnSizeAllocated  (object o, SizeAllocatedArgs args)
	{
		TILES_PER_ROW = (args.Allocation.Width - BORDER_LEFT - BORDER_RIGHT) /  COLUMN_WIDTH;
		SetSizeRequest( -1, ROW_HEIGHT * ((badguys.Count - 1) / TILES_PER_ROW + 1));
	}
}

/// <summary>
/// Creator for widget editing Dispenser.badguy field.
/// </summary>
public sealed class ChooseBadguyWidget : CustomSettingsWidget
{
	public override Widget Create(object caller)
	{
		BadguyChooserWidget editor = new BadguyChooserWidget(Field, Object);

		// Create a tooltip if we can.
		CreateToolTip(caller, editor);

		return editor;
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
