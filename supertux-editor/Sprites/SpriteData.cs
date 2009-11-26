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
using Drawing;
using Lisp;
using DataStructures;

namespace Sprites
{
/// <summary>
/// Stores the "static" data (images, size and so on) for sprites.
/// </summary>
internal class SpriteData {
	/// <summary>
	/// An action, and it's images and other data.
	/// </summary>
	public class Action {
		public string Name;
		/// <summary>
		/// A list of <see cref="Surface"/> objects for the
		/// images in the action.
		/// </summary>
		public List<Surface> Frames = new List<Surface>();
		public float Speed = 1.0f;
		/// <summary>
		/// Offset for coordinates that should be used in level file compared
		/// to coordinates for image. Calculated from <see cref="Hitbox"/>
		/// </summary>
		public Vector Offset = new Vector();
		/// <summary>
		/// The width of the widest of the images that this action consists of.
		/// </summary>
		public float Width;
		/// <summary>
		/// The height of the highest of the images that this action consists of.
		/// </summary>
		public float Height;
		/// <summary>
		/// The hitbox (if any) for this action.
		/// </summary>
		/// <remarks>
		/// The game uses float for this so we do as well, no sprite
		/// currently use anything but integers for this though.
		/// </remarks>
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
				if (hitbox.Count != 4)
					throw new Exception("hitbox must specify exactly 4 coordinates");
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

/* EOF */
