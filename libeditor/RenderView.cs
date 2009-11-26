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
using Gtk;
using Gdk;
using OpenGl;
using Drawing;
using SceneGraph;
using DataStructures;

public class RenderView : GLWidgetBase
{
	public static TargetEntry [] DragTargetEntries = new TargetEntry[] {
		new TargetEntry("GameObject", TargetFlags.App, 0)
	};

	public Node SceneGraphRoot;

	private bool dragging;
	private Vector dragStartMouse;
	private Vector dragStartTranslation;
	private Vector MousePos;

	private IEditor editor;
	public IEditor Editor {
		set {
			if(this.editor != null) {
				this.editor.Redraw -= QueueDraw;
				if(this.editor is IEditorCursorChange)
					((IEditorCursorChange) this.editor).CursorChange -= CursorChange;
				if(this.editor is IDisposable)
					((IDisposable) this.editor).Dispose();
			}

			this.editor = value;
			if(this.editor != null) {
				this.editor.Redraw += QueueDraw;
				if(this.editor is IEditorCursorChange)
					((IEditorCursorChange) this.editor).CursorChange += CursorChange;
			}
		}
		get {
			return editor;
		}
	}

	public RenderView()
	{
		ButtonPressEvent += OnButtonPress;
		ButtonReleaseEvent += OnButtonRelease;
		MotionNotifyEvent += OnMotionNotify;
		ScrollEvent += OnScroll;
		AddEvents((int) Gdk.EventMask.ButtonPressMask);
		AddEvents((int) Gdk.EventMask.ButtonReleaseMask);
		AddEvents((int) Gdk.EventMask.PointerMotionMask);
		AddEvents((int) Gdk.EventMask.ScrollMask);
		CanFocus = true;
		Gtk.Drag.DestSet(this, DestDefaults.Drop | DestDefaults.Motion, DragTargetEntries, Gdk.DragAction.Copy);
		DragMotion += OnDragMotion;
		DragDataReceived += OnDragDataReceived;
	}

	protected override void DrawGl()
	{
		gl.ClearColor(0.4f, 0, 0.4f, 1);
		gl.Clear(gl.COLOR_BUFFER_BIT);

		if(SceneGraphRoot != null)
			SceneGraphRoot.Draw(GetClipRect());

		if(!dragging && Editor != null)
			Editor.Draw(GetClipRect());
	}

	private void OnDragMotion(object o, DragMotionArgs args)
	{
		//pass motion event to editor
		if(Editor != null) {
			MousePos = MouseToWorld(new Vector((float) args.X, (float) args.Y));
			Editor.OnMouseMotion(MousePos, ModifierType.Button1Mask);
		}
	}

	private void OnDragDataReceived(object o, DragDataReceivedArgs args)
	{
//		string data = System.Text.Encoding.UTF8.GetString (args.SelectionData.Data);	//no data transmitted on "fake drag"s

		if(Editor != null) {
			MousePos = MouseToWorld(new Vector((float) args.X, (float) args.Y));

			//emulated click on current editor to place object and perform "fake drag"
			Editor.OnMouseButtonPress(MousePos, 1, ModifierType.Button1Mask);
			Editor.OnMouseButtonRelease(MousePos, 1, ModifierType.Button1Mask);

			Gtk.Drag.Finish (args.Context, true, false, args.Time);
		} else	{
			LogManager.Log(LogLevel.Warning, "DragDataRecieved OK, but there is no editor that can handle it");
			Gtk.Drag.Finish (args.Context, false, false, args.Time);
		}
	}

	private void OnButtonPress(object o, ButtonPressEventArgs args)
	{
		try {
			MousePos = MouseToWorld(
					new Vector((float) args.Event.X, (float) args.Event.Y));

			if(args.Event.Button == 2) {
				dragStartMouse = new Vector((float) args.Event.X, (float) args.Event.Y);
				dragStartTranslation = Translation;
				dragging = true;
				QueueDraw();
			} else if(Editor != null) {
				Editor.OnMouseButtonPress(MousePos, (int) args.Event.Button, args.Event.State);
			}

			args.RetVal = true;
		} catch(Exception e) {
			ErrorDialog.Exception(e);
		}
	}

	private void OnButtonRelease(object o, ButtonReleaseEventArgs args)
	{
		try {
			MousePos = MouseToWorld(
					new Vector((float) args.Event.X, (float) args.Event.Y));

			if(args.Event.Button == 2) {
				dragging = false;
				QueueDraw();
			} else if(Editor != null) {
				Editor.OnMouseButtonRelease(MousePos, (int) args.Event.Button, args.Event.State);
			}

			args.RetVal = true;
		} catch(Exception e) {
			ErrorDialog.Exception(e);
		}
	}

	private void OnMotionNotify(object o, MotionNotifyEventArgs args)
	{
		try {
			Vector pos = new Vector((float) args.Event.X, (float) args.Event.Y);
			MousePos = MouseToWorld(pos);

			if(dragging) {
				SetTranslation(dragStartTranslation	+
				               (pos - dragStartMouse) / Zoom);
			} else if(Editor != null) {
				Editor.OnMouseMotion(MousePos, args.Event.State);
			}

			args.RetVal = true;
		} catch(Exception e) {
			ErrorDialog.Exception(e);
		}
	}

	private void OnScroll(object o, ScrollEventArgs args)
	{
		float oldZoom = Zoom;
		Vector realMousePos = (MousePos + Translation) * Zoom;

		if(args.Event.Direction == ScrollDirection.Down) {
			Zoom /= (float) Math.Sqrt(2);
		} else if(args.Event.Direction == ScrollDirection.Up) {
			Zoom *= (float) Math.Sqrt(2);
		}

		//Limit the Zoom to useful values;
		if( Zoom < 0.002 || Zoom > 500 ){
			Zoom = oldZoom;
		}

		SetTranslation(Translation
		                 + realMousePos / Zoom - realMousePos / oldZoom);

		MousePos = MouseToWorld(realMousePos);
		if(Editor != null) {
			Editor.OnMouseMotion(MousePos, args.Event.State);
		}
		args.RetVal = true;
	}

	private void UpdateAdjustments()
	{
		if(hadjustment != null) {
			hadjustment.SetBounds(minx, maxx, 32/Zoom, 256/Zoom, Allocation.Width/Zoom);
			hadjustment.ClampPage(-Translation.X, -Translation.X + (Allocation.Width/Zoom));
		}
		if(vadjustment != null) {
			vadjustment.SetBounds(miny, maxy, 32/Zoom, 256/Zoom, Allocation.Height/Zoom);
			vadjustment.ClampPage(-Translation.Y, -Translation.Y + (Allocation.Height/Zoom));
		}
	}

	public void SetZoom(float newZoom)
	{
		float oldZoom = Zoom;
		Zoom = newZoom;

		// Limit the Zoom to useful values;
		if( Zoom < 0.002 || Zoom > 500 ){
			Zoom = oldZoom;
		}

		UpdateAdjustments();
		QueueDraw();
	}

	public float GetZoom()
	{
		return Zoom;
	}

	public void ZoomIn()
	{
		SetZoom( Zoom * (float) Math.Sqrt(2));
	}

	public void ZoomOut()
	{
		SetZoom( Zoom / (float) Math.Sqrt(2));
	}

	public void SetTranslation(Vector tr)
	{
		Translation = tr;

		UpdateAdjustments();
		QueueDraw();
	}

	public Vector GetTranslation()
	{
		return Translation;
	}

	/// <summary>
	///		Returns a <see cref="Gdk.Rectangle"/> for the currently
	///		visible area in world coordinates.
	/// </summary>
	/// <returns>A <see cref="Gdk.Rectangle"/>.</returns>
	public Gdk.Rectangle GetClipRect() {
		return new Gdk.Rectangle(
		           (int)-Translation.X, (int)-Translation.Y,
		           (int)(Allocation.Width / Zoom), (int)(Allocation.Height / Zoom));
	}

	private Vector MouseToWorld(Vector MousePos)
	{
		return MousePos / Zoom - Translation;
	}

	private void CursorChange(Cursor cursor)
	{
		GdkWindow.Cursor = cursor;
	}

	/**
	 * Add gtk adjustments for vertical and horizontal scrolling
	 * (This is used for scrollbars)
	 */
	public void SetAdjustments(Adjustment hadjustment, Adjustment vadjustment)
	{
		this.vadjustment = vadjustment;
		this.hadjustment = hadjustment;
		if(vadjustment != null) {
			vadjustment.ValueChanged += OnVAdjustmentChanged;
		}
		if(hadjustment != null) {
			hadjustment.ValueChanged += OnHAdjustmentChanged;
		}
		UpdateAdjustments();
	}

	private void OnHAdjustmentChanged(object sender, EventArgs e)
	{
		SetTranslation(new Vector((float) -hadjustment.Value, Translation.Y));
	}

	private void OnVAdjustmentChanged(object sender, EventArgs e)
	{
		SetTranslation(new Vector(Translation.X, (float) -vadjustment.Value));
	}

	private Adjustment vadjustment;
	private Adjustment hadjustment;
	protected float minx = -1000, maxx = 1000;
	protected float miny = -1000, maxy = 1000;
}

/* EOF */
