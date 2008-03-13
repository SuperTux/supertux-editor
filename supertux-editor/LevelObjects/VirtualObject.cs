// $Id$
using System;
using DataStructures;
using Drawing;
using Sprites;

/// <summary>
/// A base class for objects that don't really have a "position"
/// (examples are background images or the camera)
/// These objects are represented by icons that are arranged at the upper left
/// edge of the sector.
/// </summary>
public abstract class VirtualObject : IGameObject, IObject {
	private float X, Y;

	public virtual RectangleF Area {
		get {
			return new RectangleF(X - Sprite.Offset.X, Y - Sprite.Offset.Y,
		                          Sprite.Width, Sprite.Height);
		}
	}

	public VirtualObject() {
		// query the SupertuxObject attribute and grab the sprite...
		Console.WriteLine("MyType: {0}", GetType());
		SupertuxObjectAttribute objectAttribute
			= (SupertuxObjectAttribute)	Attribute.GetCustomAttribute(GetType(),
					typeof(SupertuxObjectAttribute));
		if(objectAttribute == null)
			throw new Exception("VirtualObject childclasses need a SupertuxObject attribute!");

		sprite = CreateSprite(objectAttribute.IconSprite, objectAttribute.ObjectListAction);
	}

    private static Sprite CreateSprite(string name, string action)
    {
        Sprite result = null;

        // Might be a sprite
        try{
            result = SpriteManager.Create(name);
        } catch {
        }

        if( result != null ){ // Try to find a nice action.
            // Check if we were passed an action to use and if not set it to left.
            if (String.IsNullOrEmpty(action))
                action = "left";
            try { result.Action = action; }
            catch { try { result.Action = "normal"; }
                catch { try { result.Action = "default"; }
                    catch {
                        LogManager.Log(LogLevel.DebugWarning, "ObjectListWidget: No action selected for " + name);
                    }
                }
            }
        } else { // Not a sprite so it has to be an Image.
            try{
                result = SpriteManager.CreateFromImage(name);
            } catch(Exception) {
                result = null;
            }
        }

        return result;
    }

	public virtual void ChangeArea(RectangleF NewArea)
	{
		// you can't move VirtualObjects around
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

	public virtual bool Resizable {
		get {
			return false;
		}
	}

	public virtual void Draw(DrawingContext context) {
		Sprite.Draw(context, new Vector(X, Y), 1);
	}

	/* should only be used by the Sector class... */
	public void SetPos(float x, float y)
	{
		this.X = x;
		this.Y = y;
	}
}

