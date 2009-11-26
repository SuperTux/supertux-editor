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
using OpenGl;
using DataStructures;
using SceneGraph;

public sealed class ControlPoint : IObject, Node
{
	public enum AttachPoint {
		TOP = 1,
		BOTTOM = 2,
		LEFT = 4,
		RIGHT = 8
	};

	private RectangleF area;
	private AttachPoint attachPoint;
	public IObject Object;

	private static float DISTANCE = 12;
	private static float SIZE = 16;

	public ControlPoint(IObject Object, AttachPoint attachPoint)
	{
		this.Object = Object;
		this.attachPoint = attachPoint;
	}

	public void Draw(Gdk.Rectangle cliprect)
	{
		UpdatePosition();
		gl.Color4f(0, 0, 1, 0.7f);
		gl.Disable(gl.TEXTURE_2D);

		gl.Begin(gl.QUADS);
		gl.Vertex2f(Area.Left, Area.Top);
		gl.Vertex2f(Area.Right, Area.Top);
		gl.Vertex2f(Area.Right, Area.Bottom);
		gl.Vertex2f(Area.Left, Area.Bottom);
		gl.End();

		gl.Enable(gl.TEXTURE_2D);
		gl.Color4f(1, 1, 1, 1);
	}

	public void UpdatePosition()
	{
		Vector pos;
		if((attachPoint & AttachPoint.TOP) != 0) {
			pos.Y = Object.Area.Top - DISTANCE;
		} else if((attachPoint & AttachPoint.BOTTOM) != 0) {
			pos.Y = Object.Area.Bottom + DISTANCE;
		} else {
			pos.Y = (Object.Area.Top + Object.Area.Bottom) / 2f;
		}
		if((attachPoint & AttachPoint.LEFT) != 0) {
			pos.X = Object.Area.Left - DISTANCE;
		} else if((attachPoint & AttachPoint.RIGHT) != 0) {
			pos.X = Object.Area.Right + DISTANCE;
		} else {
			pos.X = (Object.Area.Left + Object.Area.Right) / 2f;
		}
		area = new RectangleF(pos.X - SIZE/2f, pos.Y - SIZE/2f, SIZE, SIZE);
	}

	public void ChangeArea(RectangleF Area)
	{
		float adjust = SIZE/2f + DISTANCE;

		RectangleF newArea = Object.Area;
		if((attachPoint & AttachPoint.TOP) != 0) {
			newArea.Top = Area.Top + adjust;
		} else if((attachPoint & AttachPoint.BOTTOM) != 0) {
			newArea.Bottom = Area.Bottom - adjust;
		}
		if((attachPoint & AttachPoint.LEFT) != 0) {
			newArea.Left = Area.Left + adjust;
		} else if((attachPoint & AttachPoint.RIGHT) != 0) {
			newArea.Right = Area.Right - adjust;
		}
		Object.ChangeArea(newArea);
		area = Area;
	}

	public bool Resizable {
		get {
			return false;
		}
	}

	public RectangleF Area {
		get {
			return area;
		}
	}

	public Node GetSceneGraphNode() {
		return this;
	}
}

/* EOF */
