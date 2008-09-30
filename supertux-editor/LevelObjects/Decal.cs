//  $Id$
/*
 * Copyright (C) 2008 Christoph Sommer <christoph.sommer@2008.expires.deltadevelopment.de>
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 *
 */
using System;
using DataStructures;
using LispReader;
using Drawing;
using SceneGraph;

[SupertuxObject("decal", "images/engine/editor/decal.png")]
public sealed class Decal : IGameObject, IObject, Node {
	[LispChild("x", Optional = true, Default = 0f)]
	public float X;
	[LispChild("y", Optional = true, Default = 0f)]
	public float Y;

	[LispChild("image")]
	[ChooseResourceSetting]
	public string Image {
		get {
			return image;
		}
		set {
			if (String.IsNullOrEmpty(value)) {
				surface = null;
			} else {
				surface = new Surface(value);
			}
			image = value;
		}
	}
	private string image;
	private Surface surface;

	[LispChild("layer", Optional = true, Default = 50)]
	public int Layer = 50;

	public RectangleF Area {
		get {
			if(surface != null) {
				return new RectangleF(X, Y, surface.Width, surface.Height);
			} else {
				return new RectangleF(X, Y, 32, 32);
			}
		}
	}

	public bool Resizable {
		get {
			return false;
		}
	}

	public void ChangeArea(RectangleF NewArea) {
		X = NewArea.Left;
		Y = NewArea.Top;
	}

	public void Draw(Gdk.Rectangle cliprect)
	{
		if (surface == null) return;
		surface.Draw(new Vector(X, Y));

	}

	public Node GetSceneGraphNode() {
		return this;
	}

}
