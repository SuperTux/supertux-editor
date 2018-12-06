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

using DataStructures;
using Sprites;
using System;
using Lisp;
using LispReader;
using Drawing;
using OpenGl;
using SceneGraph;

/// <summary>Base class for objects in levels</summary>
public abstract class SimpleObject : IGameObject, IObject, Node, ICloneable {
	[PropertyProperties(Tooltip = ToolTipStrings.ScriptingName)]
	[LispChild("name", Optional = true, Default = "")]
	public string EntityName = String.Empty;

	private float x, y;
	[PropertyProperties(Tooltip = "X position of object", RedrawOnChange = true)]
	[LispChild("x")]
	public virtual float X {
	 	get {
			return x;
		}
		set {
			x = value;
		}
	}
	[PropertyProperties(Tooltip = "Y position of object", RedrawOnChange = true)]
	[LispChild("y")]
	public virtual float Y {
	 	get {
			return y;
		}
		set {
			y = value;
		}
	}

	public virtual RectangleF Area {
		get {
			if(Sprite != null)
				return new RectangleF(X - Sprite.Offset.X, Y - Sprite.Offset.Y,
				                          Sprite.Width, Sprite.Height);
			else
				return new RectangleF(X, Y, 32, 32);
		}
	}

	/// <summary>
	/// If true object is resizable.
	/// </summary>
	public virtual bool Resizable {
		get {
			return false;
		}
	}

	private Sprite sprite;
	protected Sprite Sprite {
		get {
			return sprite;
		}
		set {
			sprite = value;
		}
	}

	public virtual void ChangeArea(RectangleF NewArea) {
		X = NewArea.Left;
		Y = NewArea.Top;
		if(Sprite != null) {
			X += Sprite.Offset.X;
			Y += Sprite.Offset.Y;
		}
	}

	public virtual void Draw(Gdk.Rectangle cliprect) {
		if(Sprite == null)
			return;
		if (cliprect.IntersectsWith((Gdk.Rectangle) Area))
			Sprite.Draw(new Vector(X, Y));
	}

	public virtual Node GetSceneGraphNode() {
		return this;
	}

	public virtual object Clone() {
		return MemberwiseClone();
	}
}

/// <summary>Base class for objects that can have a sprite change.</summary>
public abstract class SimpleSpriteObject : SimpleObject
{
  [PropertyProperties(Tooltip = "File describing \"skin\" for object.", RedrawOnChange = true)]
	[ChooseResourceSetting]
	[LispChild("sprite", Optional = true)]
	public string SpriteFile {
		get {
			return spriteFile;
		}
		set {
			if(! String.IsNullOrEmpty(value))
				Sprite = SpriteManager.Create(value);
			spriteFile = value;
		}
	}
	private string spriteFile;
}

/// <summary>Base class for objects that draw a color box.</summary>
public abstract class SimpleColorObject : SimpleObject
{
	public virtual void DrawColor(Drawing.Color color) {
		//draw a color rectangle
		float left = X + 8;
		float right = X + 24;
		float top = Y + 8;
		float bottom = Y + 24;

		float[] current_color = new float[4];
		gl.GetFloatv( gl.CURRENT_COLOR, current_color );
		//get current color, might be transparent
		gl.Color4f(color.Red, color.Green, color.Blue, color.Alpha*current_color[3]);
		gl.Disable(gl.TEXTURE_2D);

		gl.Begin(gl.QUADS);
		gl.Vertex2f(left, top);
		gl.Vertex2f(right, top);
		gl.Vertex2f(right, bottom);
		gl.Vertex2f(left, bottom);
		gl.End();

		gl.Enable(gl.TEXTURE_2D);
		gl.Color4fv( current_color );
	}
}

/// <summary>Base class for badguys</summary>
public abstract class SimpleBadguyObject : SimpleSpriteObject
{
  [PropertyProperties(Tooltip = "Squirrel script to be run when badguy dies")]
  [LispChild("dead-script", Optional = true, Default = "")]
	[EditScriptSetting]
	public String DeadScript = String.Empty;
}

/// <summary>Base class for objects with a direction. (Like most badguys)</summary>
public abstract class SimpleDirObject : SimpleBadguyObject
{
	public enum Directions {
		/// <summary>
		/// Automatic.
		/// </summary>
		auto,
		/// <summary>
		/// Start out turend towards left side.
		/// </summary>
		left,
		/// <summary>
		/// Start out turend towards right side.
		/// </summary>
		right
	}

	// Override to change sprite on direction change.
	protected virtual void DirectionChanged () {
		string oldaction = Sprite.Action;
		if (direction == Directions.right) {
			try { Sprite.Action = "right"; }
			catch { try { Sprite.Action = "walking-right"; }
				catch {
					LogManager.Log(LogLevel.Warning, "SimpleDirObject: No action found for object.");
					Sprite.Action = oldaction;
				}
			}
		} else {
			try { Sprite.Action = "left"; }
			catch { try { Sprite.Action = "walking-left"; }
				catch {
					LogManager.Log(LogLevel.Warning, "SimpleDirObject: No action found for object.");
					Sprite.Action = oldaction;
				}
			}
		}
	}

	protected Directions direction = Directions.auto;

	/// <summary>
	/// Direction the badguy will be facing initially.
	/// </summary>
	[PropertyProperties(Tooltip = "Direction the badguy will be facing initially.", RedrawOnChange = true)]
	[LispChild("direction", Optional = true, Default = Directions.auto)]
	public Directions Direction {
		get {
			return direction;
		}
		set {
			direction = value;
			DirectionChanged();
		}
	}
}

/// <summary>Base class for platforms and other moving objects.</summary>
public abstract class SimplePathObject : SimpleObject, IPathObject
{
	[PropertyProperties(Tooltip = "If enabled the platform will be moving initially.")]
	[LispChild("running", Optional = true, Default = true)]
	public bool Running = true;
	[PropertyProperties(Tooltip = "File describing \"skin\" for object.", RedrawOnChange = true)]
	[ChooseResourceSetting]
	[LispChild("sprite")]
	public string SpriteFile {
		get {
			return spriteFile;
		}
		set {
			if(! String.IsNullOrEmpty(value))
				Sprite = SpriteManager.Create(value);
			spriteFile = value;
		}
	}
	private string spriteFile;

	private Path path = null;
	[LispChild("path")]
	public Path Path {
		get { return path; }
		set { path = value; }
	}

	private string pathRef = String.Empty;

	[LispChild("path-ref", Optional = true, Default="")]
	public string PathRef {
		get { return pathRef; }
		set { pathRef = value; }
	}

	[LispChild("x", Optional = true, Transient = true)]
	public override float X {
	 	get {
			return path.Nodes[0].X;
		}
		set {
			path.Nodes[0].X = value;
		}
	}
	[LispChild("y", Optional = true, Transient = true)]
	public override float Y {
	 	get {
			return path.Nodes[0].Y;
		}
		set {
			path.Nodes[0].Y = value;
		}
	}

	public virtual bool PathRemovable {
		get { return false; }
	}

	public SimplePathObject()
	{
	}

	public override void ChangeArea(RectangleF NewArea) {
		base.ChangeArea(NewArea);
		Vector translation = new Vector(NewArea.Left - Path.Nodes[0].X,
		                                NewArea.Top - Path.Nodes[0].Y);
		Path.Move(translation);
	}

	public override RectangleF Area {
		get {
			if (Path != null) {
				X = Path.Nodes[0].X;	//object is always at the first path node
				Y = Path.Nodes[0].Y;
			}

			return base.Area;
		}
	}

	public override object Clone() {
		SimplePathObject aClone = (SimplePathObject) MemberwiseClone();
		if (Path != null) {
			aClone.Path = new Path();
			foreach(Path.Node node in this.Path.Nodes) {
				aClone.Path.Nodes.Add((Path.Node) node.Clone());
			}
		}
		return aClone;
	}
}

/// <summary>Base class for area objects in levels</summary>
public abstract class SimpleObjectArea : SimpleObject
{
	[PropertyProperties(Tooltip = "How wide the object is.", RedrawOnChange = true)]
	[LispChild("width")]
	public float Width = 32;
	[PropertyProperties(Tooltip = "How high the object is.", RedrawOnChange = true)]
	[LispChild("height")]
	public float Height = 32;

	protected Drawing.Color Color;

	public override RectangleF Area {
		get {
			return new RectangleF(X, Y, Width, Height);
		}
	}

	/// <summary>
	/// If true object is resizable.
	/// </summary>
	public sealed override bool Resizable {
		get {
			return true;
		}
	}

	public override void Draw(Gdk.Rectangle cliprect) {
		if (!cliprect.IntersectsWith(new Gdk.Rectangle((int) X, (int) Y, (int) Width, (int) Height)))
			return;
		float left = X;
		float right = X + Width;
		float top = Y;
		float bottom = Y + Height;

		float[] current_color = new float[4];
		gl.GetFloatv( gl.CURRENT_COLOR, current_color );
		//get current color, might be transparent
		gl.Color4f(Color.Red, Color.Green, Color.Blue, current_color[3] * Color.Alpha);
		gl.Disable(gl.TEXTURE_2D);

		gl.Begin(gl.QUADS);
		gl.Vertex2f(left, top);
		gl.Vertex2f(right, top);
		gl.Vertex2f(right, bottom);
		gl.Vertex2f(left, bottom);
		gl.End();

		gl.Enable(gl.TEXTURE_2D);
		gl.Color4fv( current_color );
	}

	public override void ChangeArea(RectangleF NewArea) {
		X = NewArea.Left;
		Y = NewArea.Top;
		Width = NewArea.Width;
		Height = NewArea.Height;
	}

	public sealed override Node GetSceneGraphNode() {
		return this;
	}
}

/* EOF */
