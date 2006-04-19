using DataStructures;
using Sprites;
using System;
using Lisp;
using LispReader;
using Drawing;
using OpenGl;
using SceneGraph;

public abstract class SimpleObject : IGameObject, IObject, Node, ICloneable {
	[LispChild("x")]
	public float X;
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

public class SimpleObjectArea : SimpleObject
{
	[LispChild("width")]
	public float Width = 32;
	[LispChild("height")]
	public float Height = 32;
	
	protected Drawing.Color Color;
	
	public override RectangleF Area {
		get {
			return new RectangleF(X, Y, Width, Height);
		}
	}
	
	public override bool Resizable {
		get {
			return true;
		}
	}
	
	public override void Draw() {
		float left = X;
		float right = X + Width;
		float top = Y;
		float bottom = Y + Height;
			
		gl.Color4f(Color.Red, Color.Green, Color.Blue, Color.Alpha);
		gl.Disable(gl.TEXTURE_2D);
		
		gl.Begin(gl.QUADS);
		gl.Vertex2f(left, top);
		gl.Vertex2f(right, top);
		gl.Vertex2f(right, bottom);
		gl.Vertex2f(left, bottom);
		gl.End();
			
		gl.Enable(gl.TEXTURE_2D);
		gl.Color4f(1, 1, 1, 1);			
	}
	
	public override void ChangeArea(RectangleF Area) {
		X = Area.Left;
		Y = Area.Top;
		Width = Area.Width;
		Height = Area.Height;
	}
	
	public override Node GetSceneGraphNode() {
		return this;
	}
}

