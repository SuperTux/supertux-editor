//  $Id$
using System;
using System.Collections.Generic;
using Drawing;
using Lisp;
using DataStructures;

namespace Sprites
{

internal class SpriteData {
	public class Action {
		public string Name;
		public List<Surface> Frames = new List<Surface>();
		public float Speed = 1.0f;
		public Vector Offset = new Vector();
		public float Width;
		public float Height;
		public RectangleF Hitbox;

		public Action(string Name, Surface Surface) {
			this.Name = Name;
			Frames.Add(Surface);
			Width = Surface.Width;
			Height = Surface.Height;
		}

		public Action(List Data, string BaseDir, SpriteData spriteData) {
			Properties Props = new Properties(Data);
			if(!Props.Get("name", ref Name))
				throw new Exception("Action without name specified");
			Props.Get("fps", ref Speed);
			if(Props.Exists("hitbox")) {
				List<float> hitbox = new List<float>();
				Props.GetFloatList("hitbox", hitbox);
				Hitbox = new RectangleF(hitbox[0], hitbox[1], hitbox[2], hitbox[3]);
				Offset.X = Hitbox.Left;
				Offset.Y = Hitbox.Top;
			}
			List<string> ImageFileNames = new List<string>();
			Props.GetStringList("images", ImageFileNames);

			Props.PrintUnusedWarnings();

			foreach(string ImageFile in ImageFileNames) {
				Surface surface = new Surface(BaseDir + "/" + ImageFile);
				Width = Math.Max(Width, surface.Width);
				Height = Math.Max(Height, surface.Height);
				Frames.Add(surface);
			}

			string MirrorActionName = null;
			Props.Get("mirror-action", ref MirrorActionName);
			if(MirrorActionName != null) {
				Action MirrorAction = spriteData.Actions[MirrorActionName];
				foreach(Surface surface in MirrorAction.Frames) {
					Surface flippedSurface = new Surface(surface);
					flippedSurface.Left = surface.Right;
					flippedSurface.Right = surface.Left;
					Width = Math.Max(Width, surface.Width);
					Height = Math.Max(Height, surface.Height);
					Frames.Add(flippedSurface);
				}
			}
		}
	}

	public Dictionary<string, Action> Actions = new Dictionary<string, Action>();

	public SpriteData(List Data, string BaseDir) {
		LispIterator iter = new LispIterator(Data);
		while(iter.MoveNext()) {
			if(iter.Key == "action") {
				Action Action = new Action(iter.List, BaseDir, this);
				Actions.Add(Action.Name, Action);
			} else {
				Console.WriteLine("Unknown tag in sprite: " + iter.Key);
			}
		}
	}

	public SpriteData(Surface Surface, Vector offset) {
		Action Action = new Action("default", Surface);
		Action.Offset = offset;
		Actions.Add(Action.Name, Action);
	}
}

}
