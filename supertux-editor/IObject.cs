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
using SceneGraph;

/// <summary>Object which is draw and occupies an area in the sector</summary>
/// <remarks>TODO: think of a better name for this...</remarks>
public interface IObject {
	void ChangeArea(RectangleF NewArea);

	RectangleF Area {
		get;
	}

	bool Resizable {
		get;
	}

	Node GetSceneGraphNode();
}

/* EOF */
