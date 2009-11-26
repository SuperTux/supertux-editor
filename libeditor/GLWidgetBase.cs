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
using DataStructures;
using Drawing;

public abstract class GLWidgetBase : GLArea
{
	public static GLWidgetBase ShareArea = null;

	private float _Zoom = 1.0f;
	protected float Zoom
	{
		get
		{
			return _Zoom;
		}
		set
		{
			_Zoom = value;
			QueueDraw();
		}
	}

	private Vector _Translation;
	protected Vector Translation
	{
		get
		{
			return _Translation;
		}
		set
		{
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
		: base(attrlist, ShareArea)
	{
		GlUtil.ContextValid = false;
		ExposeEvent += OnExposed;
		ConfigureEvent += OnConfigure;

		if(ShareArea == null) {
			ShareArea = this;
		}
	}

	private void OnExposed(object o, ExposeEventArgs args)
	{
		if(!MakeCurrent()) {
			LogManager.Log(LogLevel.Warning, "Make Current - OnExposed failed");
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

	private void OnConfigure(object o, ConfigureEventArgs args)
	{
		if(!MakeCurrent()) {
			LogManager.Log(LogLevel.Warning, "MakeCurrent() - OnConfigure failed");
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

/* EOF */
