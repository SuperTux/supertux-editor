//  $Id$
using System;
using Gtk;
using Gdk;
using OpenGl;
using Drawing;
using DataStructures;

public abstract class RenderView : GLWidgetBase
{
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
			this.editor.Redraw += QueueDraw;
			if(this.editor is IEditorCursorChange)
				((IEditorCursorChange) this.editor).CursorChange += CursorChange;
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
	}

	protected override void DrawGl()
	{
		gl.ClearColor(0.4f, 0, 0.4f, 1);
		gl.Clear(gl.COLOR_BUFFER_BIT);

		DrawingContext context = new DrawingContext();
		/* TODO: setup transform */
		DrawContents(context);

		if(!dragging && Editor != null)
			Editor.Draw(context);
		context.DoDrawing();
	}

	public abstract void DrawContents(DrawingContext context);

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
		if(Translation != tr) {
			Translation = tr;
	
			UpdateAdjustments();
			QueueDraw();
		}
	}

	public Vector GetTranslation()
	{
		return Translation;
	}

	/// <summary>
	///		Returns a <see cref="RectangleF"/> for the currently
	///		visible area in world coordinates.
	/// </summary>
	/// <returns>A <see cref="RectangleF"/>.</returns>
	public RectangleF GetClipRect() {
		return new RectangleF(-Translation.X, -Translation.Y,
		                      Allocation.Width / Zoom,
		                      Allocation.Height / Zoom);
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
