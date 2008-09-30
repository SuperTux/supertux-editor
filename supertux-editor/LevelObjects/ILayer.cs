//  $Id$
//
//  Copyright (C) 2008 Milos Kloucek <TuxMMlosh@elektromaniak.wz.cz>
//
//  This program is free software; you can redistribute it and/or
//  modify it under the terms of the GNU General Public License
//  as published by the Free Software Foundation; either version 2
//  of the License, or (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA
//  02111-1307, USA.

/// <summary>
/// Interface for all objects that belongs to the LayerList => have Z-Position, now known as Layer.
/// </summary>
public interface ILayer
{
	int Layer {
		get;
	}
	string Name {
		get;
	}
}

//TODO: this name is bad, find better one; It should express, that it can be drawn in Editor but stay short enough..
public interface IDrawableLayer : ILayer
{
	SceneGraph.Node GetSceneGraphNode();
}
