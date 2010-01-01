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
using SceneGraph;
using Lisp;
using LispReader;
using System.Collections.Generic;

		//tilemaps are no longer created as badguys, it is looking bad
[SupertuxObject("tilemap", "images/engine/editor/tilemap.png",
                Target = SupertuxObjectAttribute.Usage.None)]
public sealed class Tilemap : TileBlock, IGameObject, IPathObject, IDrawableLayer
{
	//TODO: If we want to store X coordinate to level file, we must uncomment this and add support for it
	//If you do that, please remove " else X = Y = 0;" in UpdatePos();
	//[LispChild("x", Optional = true, Default = 0.0f)]
	public float X = 0;

	//TODO: If we want to store Y coordinate to level file, we must uncomment this and add support for it
	//If you do that, please remove " else X = Y = 0;" in UpdatePos();
	//[LispChild("y", Optional = true, Default = 0.0f)]
	public float Y = 0;

	[PropertyProperties(Tooltip = "If selected Tux will interact with tiles in this tilemap.")]
	[LispChild("solid")]
	public bool Solid = false;

	private int ZPos = 0;
	[LispChild("z-pos")]
	public int Layer {
		get {
			return ZPos;
		}
		set {
			ZPos = value;
		}
	}

	private string name = String.Empty;
	[PropertyProperties(Tooltip = ToolTipStrings.ScriptingName)]
	[LispChild("name", Optional = true, Default = "")]
	public string Name {
		get {
			return name;
		}
		set {
			name = value;
		}
	}

	[LispChild("speed", Optional = true, Default = 1.0f)]
	public float Speed = 1.0f;

	[LispChild("speed-y", Optional = true, Default = 1.0f)]
	public float SpeedY = 1.0f;

	private Path path;
	[LispChild("path", Optional = true, Default = null)]
	public Path Path
	{
		get {
			return path;
		}
		set {
			path = value;
		}
	}

	public bool PathRemovable
	{
		get { return true; }
	}

	public enum DrawTargets
	{
		/// <summary>
		/// Normal tilemap.
		/// </summary>
		normal,
		/// <summary>
		/// Used for lightmap.
		/// </summary>
		lightmap
	}

	/// <summary>
	/// Target for tilemap.
	/// </summary>
	[LispChild("draw-target", Optional = true, Default = DrawTargets.normal)]
	public DrawTargets DrawTarget = DrawTargets.normal;

	[PropertyProperties(Tooltip = "Opacity of this Tilemap, ranges from 0.0 (transparent) to 1.0 (fully opaque)")]
	[LispChild("alpha", Optional = true, Default = 1.0f)]
	public float Alpha = 1.0f;

	public Tilemap() : base() {
	}

	public void UpdatePos()
	{
		if (path != null && path.Nodes.Count > 0){
			X = path.Nodes[0].X;
			Y = path.Nodes[0].Y;
		} else X = Y = 0;
	}

	public Node GetSceneGraphNode()
	{
		return null;	//Tilemap can't create it's node
	}
}

/* EOF */
