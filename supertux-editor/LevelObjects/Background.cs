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
using DataStructures;
using LispReader;
using Drawing;
using SceneGraph;

[SupertuxObject("background", "images/engine/editor/background.png")]
public sealed class Background : IGameObject, Node, IDrawableLayer {
	public string Name {
		get {
			return "Background IMG";
		}
	}

	[PropertyProperties(Tooltip = ToolTipStrings.ScriptingName)]
	[LispChild("name", Optional = true, Default = "")]
	public string EntityName = String.Empty;

	[LispChild("x", Optional = true, Default = 0f)]
	public float X;
	[LispChild("y", Optional = true, Default = 0f)]
	public float Y;

	public enum Alignments {
		none,
		left,
		right,
		top,
		bottom
	}

	/// <summary>
	/// Controls which direction(s) if any that the background is repeated in.
	/// Top and bottom images are only used for "none".
	/// </summary>
	[LispChild("alignment", Optional = true, Default = Alignments.none)]
	public Alignments Alignment = Alignments.none;

	[LispChild("image-top", Optional = true, Default = "")]
	[ChooseResourceSetting]
	public string ImageTop {
		get {
			return imageTop;
		}
		set {
			if (String.IsNullOrEmpty(value)) {
				surfaceTop = null;
			} else {
				surfaceTop = new Surface(value);
			}
			imageTop = value;
		}
	}
	private string imageTop;
	private Surface surfaceTop;

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

	[LispChild("image-bottom", Optional = true, Default = "")]
	[ChooseResourceSetting]
	public string ImageBottom {
		get {
			return imageBottom;
		}
		set {
			if (String.IsNullOrEmpty(value)) {
				surfaceBottom = null;
			} else {
				surfaceBottom = new Surface(value);
			}
			imageBottom = value;
		}
	}
	private string imageBottom;
	private Surface surfaceBottom;

	[LispChild("scroll-offset-x", Optional = true, Default = 0.0f)]
	public float OffsetX = 0;

	[LispChild("scroll-offset-y", Optional = true, Default = 0.0f)]
	public float OffsetY = 0;

	[LispChild("scroll-speed-x", Optional = true, Default = 0.5f)]
	public float SpeedX = 0.5f;

	[LispChild("scroll-speed-y", Optional = true, Default = 0.5f)]
	public float SpeedY = 0.5f;

	[LispChild("speed")]
	public float Speed = 0.5f;

	[LispChild("fill", Optional = true, Default = false)]
	public bool Fill = false;

	private int layer = -300;
	[LispChild("z-pos", Optional = true, Default = -300, AlternativeName = "layer")]
	public int Layer {
		get {
			return layer;
		}
		set {
			layer = value;
		}
	}

	[LispChild("blend", Optional = true, Default = BlendMode.blend)]
	public BlendMode BlendMode = BlendMode.blend;

	[LispChild("target", Optional = true, Default = DrawTargets.normal)]
	public DrawTargets DrawTarget = DrawTargets.normal;

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

		Surface sm = surface;
		Surface st = (surfaceTop != null)?(surfaceTop):(surface);
		Surface sb = (surfaceBottom != null)?(surfaceBottom):(surface);

		// TODO:
		// * Clip images that are partially outside the sector to make the edges
		//   "neat"? How?

		// This is quite a hack, since we can't get our parent sector.
		// However we will only draw if we are the current sector, thus
		// we get the size from the Application.
		Gdk.Rectangle sectorbounds = new Gdk.Rectangle(0, 0,
		                                               (int) Application.EditorApplication.CurrentSector.Width*32,
		                                               (int) Application.EditorApplication.CurrentSector.Height*32);

		int minX;
		int maxX;
		if (Alignment == Alignments.none) {
			// Calc min and max *tiles*, including one *tile* overlap on both sides.
			minX = -((int)this.X / (int)st.Width) - 1;
			// Fix rounding with integer division...
			if (this.X > 0)
				minX--;
			maxX = Math.Abs(sectorbounds.Width - (int)this.X) / (int)st.Width;

			int minY = -((int)this.Y / (int)st.Height) - 1;
			// Fix rounding with integer division...
			if (this.Y > 0)
				minY--;
			int maxY = -1;	//tile position 0 belongs to middle surface

			for (int tileX = minX; tileX <= maxX; tileX++) {
				for (int tileY = minY; tileY <= maxY; tileY++) {
					st.Draw(new Vector(X + st.Width * tileX, Y + st.Height * tileY));
				}
			}
		}

		// Calc min and max *tiles*, including one *tile* overlap on both sides.
		minX = -((int)this.X / (int)sm.Width) - 1;
		// Fix rounding with integer division...
		if (this.X > 0)
			minX--;
		maxX = Math.Abs(sectorbounds.Width - (int)this.X) / (int)sm.Width;

		for (int tileX = minX; tileX <= maxX; tileX++) {
			sm.Draw(new Vector(X + sm.Width * tileX, Y));
		}

		if (Alignment == Alignments.none) {
			// Calc min and max *tiles*, including one *tile* overlap on both sides.
			minX = -((int)this.X / (int)sb.Width) - 1;
			// Fix rounding with integer division...
			if (this.X > 0)
				minX--;
			maxX = Math.Abs(sectorbounds.Width - (int)this.X) / (int)sb.Width;

			int maxY = Math.Abs(sectorbounds.Height - (int)this.Y - (int)sm.Height) / (int)sb.Height;

			for (int tileX = minX; tileX <= maxX; tileX++) {
				for (int tileY = 0; tileY <= maxY; tileY++) {
					sb.Draw(new Vector(X + sb.Width * tileX, Y + sm.Height + sb.Height * tileY));
				}
			}
		}
	}

	public Node GetSceneGraphNode() {
		return this;
	}

}

[SupertuxObject("gradient", "images/engine/editor/gradient.png")]
public sealed class Gradient : IGameObject, ILayer {
	public string Name {get {return "";}}	//= it can't have a name

	[LispChild("name", Optional = true, Default = "")]
	public string EntityName = String.Empty;

	private int layer = -300;
	[LispChild("z-pos", Optional = true, Default = -300, AlternativeName = "layer")]
	public int Layer {
		get {
			return layer;
		}
		set {
			layer = value;
		}
	}

	[LispChild("blend", Optional = true, Default = BlendMode.blend)]
	public BlendMode BlendMode = BlendMode.blend;

	[ChooseColorSetting]
	[LispChild("top_color")]
	public Color TopColor;
	[ChooseColorSetting]
	[LispChild("bottom_color")]
	public Color BottomColor;

	[LispChild("target", Optional = true, Default = DrawTargets.normal)]
	public DrawTargets DrawTarget = DrawTargets.normal;
}

/* EOF */
