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

using System.Collections.Generic;

namespace SceneGraph
{

	/// <summary>
	/// A base class that allows constructing scene graph nodes that have
	/// several childs
	/// </summary>
	public class NodeWithChilds : Node
	{
		private List<Node> Childs = new List<Node>();

		protected void DrawChilds(Gdk.Rectangle cliprect)
		{
			foreach(Node Child in Childs) {
				Child.Draw(cliprect);
			}
		}

		public void AddChild(Node Child)
		{
			Childs.Add(Child);
		}

		public void RemoveChild(Node Child)
		{
			Childs.Remove(Child);
		}

		public virtual void Draw(Gdk.Rectangle cliprect)
		{
			DrawChilds(cliprect);
		}
	}

}

/* EOF */
