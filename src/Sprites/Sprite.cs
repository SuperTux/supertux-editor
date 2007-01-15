using SceneGraph;
using DataStructures;
using System;
using Drawing;
using System.Collections;
using System.Collections.Generic;

namespace Sprites
{

public class Sprite : Node {
    private SpriteData Data;
    private SpriteData.Action CurrentAction;
    private SpriteData.Action _NextAction;
    private float ActionTimeOffset;

	public float Width {
		get {
			return CurrentAction.Width;
		}
	}

	public float Height {
		get {
			return CurrentAction.Height;
		}
	}

	public Vector Offset {
		get {
			return CurrentAction.Offset;
		}
	}

    internal Sprite(SpriteData Data)
    {
        this.Data = Data;
    	CurrentAction = null;
    	if(Data.Actions.Count > 0) {
    		IEnumerator<KeyValuePair<string, SpriteData.Action>> enumerator = Data.Actions.GetEnumerator();
    		enumerator.MoveNext();
    		CurrentAction = enumerator.Current.Value;
    	}
    }

    public string Action {
        set {
            SpriteData.Action NewAction = Data.Actions[value];
            if(NewAction == null)
                throw new Exception("No action with Name '" + value + "' defined");
			if(NewAction == CurrentAction)
				return;

            CurrentAction = NewAction;
            ActionTimeOffset = Timer.CurrentTime;
        }
        get {
			if(CurrentAction == null)
				return "";
            return CurrentAction.Name;
        }
    }
    public string NextAction {
        set{
            _NextAction = Data.Actions[value];
			if(_NextAction == CurrentAction) {
				_NextAction = null;
				return;
			}
            if(_NextAction == null)
                throw new Exception("No Action with Name '" + value + "' defined");
        }
        get {
            if(_NextAction == null)
                return "";
            return _NextAction.Name;
        }
    }

    public ICollection Actions {
        get {
            return Data.Actions.Keys;
        }
    }

    public virtual void Draw()
    {
    	Draw(new Vector(0, 0));
    }

    public virtual void Draw(Vector pos)
    {
		if(CurrentAction == null || CurrentAction.Frames.Count == 0)
			return;

        float AnimationTime = (Timer.CurrentTime - ActionTimeOffset) * CurrentAction.Speed;
        if(_NextAction != null && AnimationTime > (float) CurrentAction.Frames.Count) {
            AnimationTime -= CurrentAction.Frames.Count;
            AnimationTime /= CurrentAction.Speed;
            CurrentAction = _NextAction;
            _NextAction = null;
            AnimationTime *= CurrentAction.Speed;
        }
        int AnimationFrame = ((int) AnimationTime) % CurrentAction.Frames.Count;
        Surface Surface = CurrentAction.Frames[AnimationFrame];
        Surface.Draw(pos - CurrentAction.Offset);
    }
}

}
