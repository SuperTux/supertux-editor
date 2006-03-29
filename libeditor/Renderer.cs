using System;
using Gtk;
using Gdk;
using OpenGl;
using Drawing;
using SceneGraph;
using DataStructures;

public class RenderView : GLWidgetBase {
	public Node SceneGraphRoot;

	private bool dragging;
	private Vector dragStartMouse;
	private Vector dragStartTranslation;
	private Vector MousePos;

	private IEditor editor;
	public IEditor Editor {
		set {
			if(this.editor != null)
				this.editor.Redraw -= QueueDraw;
			this.editor = value;
			this.editor.Redraw += QueueDraw;
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
		GrabDefault();
		GrabFocus();
	}

	protected override void DrawGl() {
		gl.Clear(gl.COLOR_BUFFER_BIT);

		if(SceneGraphRoot != null)
			SceneGraphRoot.Draw();
		
		if(!dragging && Editor != null)
			Editor.Draw();
	}

	private void OnButtonPress(object o, ButtonPressEventArgs args) {
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
	}

	private void OnButtonRelease(object o, ButtonReleaseEventArgs args) {
		MousePos = MouseToWorld(
				new Vector((float) args.Event.X, (float) args.Event.Y));
	
		if(args.Event.Button == 2) {
			dragging = false;			
			QueueDraw();
		} else if(Editor != null) {
			Editor.OnMouseButtonRelease(MousePos, (int) args.Event.Button, args.Event.State);
		}

		args.RetVal = true;
	}

	private void OnMotionNotify(object o, MotionNotifyEventArgs args) {
		Vector pos = new Vector((float) args.Event.X, (float) args.Event.Y);
		MousePos = MouseToWorld(pos);
		
		if(dragging) {
			Translation = dragStartTranslation 
				+ (pos - dragStartMouse) / Zoom;
			QueueDraw();
		} else if(Editor != null) {
			Editor.OnMouseMotion(MousePos, args.Event.State);
		}

		args.RetVal = true;
	}

	private void OnScroll(object o, ScrollEventArgs args) {
		float oldZoom = Zoom;
		Vector realMousePos = (MousePos + Translation) * Zoom;

		if(args.Event.Direction == ScrollDirection.Down) {
			Zoom /= (float) Math.Sqrt(2);
		} else if(args.Event.Direction == ScrollDirection.Up) {
			Zoom *= (float) Math.Sqrt(2);
		}
	
		Translation += realMousePos/Zoom - realMousePos/oldZoom;
		
		MousePos = MouseToWorld(realMousePos);
		if(Editor != null) {
			Editor.OnMouseMotion(MousePos, args.Event.State);
		}
		args.RetVal = true;	
	}

	private Vector MouseToWorld(Vector MousePos) {
		return MousePos / Zoom - Translation;
	}
}

