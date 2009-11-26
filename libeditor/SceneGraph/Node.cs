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

namespace SceneGraph
{
	/// <summary>
	///		This is the basic object of a scene graph: A single node with a Draw()
	///		command. The nodes form a graph (in our case it's a tree). Each node
	///		will trigger Draw() commands on it subnodes.
	/// </summary>
	/// <remarks>
	///   Some introduction to scenegraphs can be found in our wiki:
	///   http://supertux.lethargik.org/wiki/SceneGraph
	/// </remarks>
	public interface Node
	{
		/// <summary>
		///		When called should draw the node
		/// </summary>
		/// <param name="cliprect">
		///		The area that is visible in the <see cref="RenderView"/>
		///		we are drawing to. Check with this to see if you can skip
		///		drawing.
		/// </param>
		void Draw(Gdk.Rectangle cliprect);
	}

}

/* EOF */
