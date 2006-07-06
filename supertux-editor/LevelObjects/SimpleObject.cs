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
	[CustomTooltip("X position of object")]
	[LispChild("x")]
	public float X;
	[CustomTooltip("Y position of object")]
	[LispChild("y")]
	public float Y;

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

	public virtual void Draw() {
		if(Sprite == null)
			return;
		
		Sprite.Draw(new Vector(X, Y));
	}
	
	public virtual Node GetSceneGraphNode() {
		return this;
	}
	
	public object Clone() {
		return MemberwiseClone();	
	}
}

/// <summary>Base class for objects with a direction. (Like most badguys)</summary>
public abstract class SimpleDirObject : SimpleObject
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

	/// <summary>
	/// Direction the badguy will be facing initaly. 
	/// </summary>
	[CustomTooltip("Direction the badguy will be facing initaly.")]
	[LispChild("direction", Optional = true, Default = Directions.auto)]
	public Directions Direction = Directions.auto;
}

/// <summary>Base class for area objects in levels</summary>
public abstract class SimpleObjectArea : SimpleObject
{
	[CustomTooltip("How wide the object is.")]
	[LispChild("width")]
	public float Width = 32;
	[CustomTooltip("How high the object is.")]
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
	
	public override void Draw() {
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
	
	public override void ChangeArea(RectangleF Area) {
		X = Area.Left;
		Y = Area.Top;
		Width = Area.Width;
		Height = Area.Height;
	}
	
	public sealed override Node GetSceneGraphNode() {
		return this;
	}
}

