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
using System.Collections.Generic;
using OpenGl;
using DataStructures;
using SceneGraph;
using Gtk;
using Gdk;
using Undo;

public sealed class ZoomTool : ITool
{
	private IToolApplication application;
	private enum State {
		NONE,
		ZOOM_RECT
	};
	private State state;
	private Vector pressPoint;
	private Vector mousePoint;

	public ZoomTool(IToolApplication application)
	{
		this.application = application;
		this.state = State.NONE;
		
	}

	public void Draw(Gdk.Rectangle cliprect)
	{
		if (state == State.ZOOM_RECT)
		{
			// FIXME: We shouldn't have to mess around here with raw OpenGL
			gl.Disable(gl.TEXTURE_2D);

			// Draw the background box
			gl.Color4f(0.5f, 0.5f, 0.0f, 0.5f);
			gl.Begin(gl.QUADS);
			gl.Vertex2f(pressPoint.X, pressPoint.Y);
			gl.Vertex2f(mousePoint.X, pressPoint.Y);
			gl.Vertex2f(mousePoint.X, mousePoint.Y);
			gl.Vertex2f(pressPoint.X, mousePoint.Y);
			gl.End();

			// Draw an outline around it
			gl.Color4f(1.0f, 1.0f, 0.0f, 1.0f);
			gl.Begin(gl.LINE_LOOP);
			gl.Vertex2f(pressPoint.X, pressPoint.Y);
			gl.Vertex2f(mousePoint.X, pressPoint.Y);
			gl.Vertex2f(mousePoint.X, mousePoint.Y);
			gl.Vertex2f(pressPoint.X, mousePoint.Y);
			gl.End();

			gl.Enable(gl.TEXTURE_2D);
			gl.Color4f(1, 1, 1, 1);
		}
	}
	
	public void OnMouseButtonPress(Vector mousePos, int button, ModifierType Modifiers)
	{
		switch (button)
		{
		case 1:
			state = State.ZOOM_RECT;
			pressPoint = mousePos;
			mousePoint = mousePos;
			LogManager.Log(LogLevel.Info, "ZoomTool::OnMouseButtonPress");
			break;
			
		case 3:
			LogManager.Log(LogLevel.Info, "ZoomTool: ZoomOut");
			break;
		}

		Redraw();
	}

	public void OnMouseButtonRelease(Vector mousePos, int button, ModifierType Modifiers)
	{
		if (button == 1)
		{
			state = State.NONE;
			LogManager.Log(LogLevel.Info, "ZoomTool::OnMouseButtonRelease");
		}
		Redraw();
	}

	public void OnMouseMotion(Vector mousePos, ModifierType Modifiers)
	{
		switch(state)
		{
		case State.NONE:
			break;

		case State.ZOOM_RECT:
			mousePoint = mousePos;
			LogManager.Log(LogLevel.Info, "ZoomTool::OnMouseMove");
			Redraw();
			break;
		}
	}

	public event RedrawEventHandler Redraw;
}

/* EOF */
