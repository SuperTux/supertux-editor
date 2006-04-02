using System;
using Gtk;
using Gdk;
using OpenGl;
using DataStructures;
using Drawing;

public abstract class GLWidgetBase : GLArea {
	private static GLWidgetBase ShareArea = null;
	
	private float _Zoom = 1.0f;
	protected float Zoom {
		get {
			return _Zoom;
		}
		set {
			_Zoom = value;
			QueueDraw();
		}
	}
	
	private Vector _Translation;
	protected Vector Translation {
		get {
			return _Translation;
		}
		set {
			_Translation = value;
			QueueDraw();
		}
	}

	private static int[] attrlist = {
		GLContextAttributes.Rgba,
 		GLContextAttributes.RedSize, 1,
		GLContextAttributes.GreenSize, 1,
		GLContextAttributes.BlueSize, 1,
		// not really needed but some opengl drivers are buggy and need this
		GLContextAttributes.DepthSize, 1,
		GLContextAttributes.DoubleBuffer,
		GLContextAttributes.None
	};

	public GLWidgetBase() 
		: base(attrlist, ShareArea) {
		GlUtil.ContextValid = false;
		ExposeEvent += OnExposed;
		ConfigureEvent += OnConfigure;
		
		if(ShareArea == null) {
			ShareArea = this;
		}
	}

	private void OnExposed(object o, ExposeEventArgs args) {	
		if(!MakeCurrent()) {
			Console.WriteLine("Make Current - OnExposed failed");
			return;
		}

		GlUtil.ContextValid = true;

		gl.MatrixMode(gl.MODELVIEW);
		gl.LoadIdentity();
		gl.Scalef(Zoom, Zoom, 1f);
		gl.Translatef(Translation.X, Translation.Y, 0f);

		DrawGl();
		GlUtil.Assert("After Drawing");

		SwapBuffers();

		GlUtil.ContextValid = false;
	}

	private void OnConfigure(object o, ConfigureEventArgs args) {
		if(!MakeCurrent()) {
			Console.WriteLine("Warning: MakeCurrent() - OnConfigure failed");
			return;
		}

		GlUtil.ContextValid = true;

		// setup opengl state and transform
        gl.Disable(gl.DEPTH_TEST);
        gl.Disable(gl.CULL_FACE);
        gl.Enable(gl.TEXTURE_2D);
        gl.Enable(gl.BLEND);
        gl.BlendFunc(gl.SRC_ALPHA, gl.ONE_MINUS_SRC_ALPHA);

		gl.ClearColor(0f, 0f, 0f, 0f);
		gl.Viewport(0, 0, Allocation.Width, Allocation.Height);
        gl.MatrixMode(gl.PROJECTION);
        gl.LoadIdentity();
        gl.Ortho(0, Allocation.Width, Allocation.Height, 0,
                -1.0f, 1.0f);
        gl.MatrixMode(gl.MODELVIEW);
        gl.LoadIdentity();
		
		GlUtil.Assert("After setting opengl transforms");

		GlUtil.ContextValid = false;
	}

	protected abstract void DrawGl();
}
