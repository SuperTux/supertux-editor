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
using System.Collections;

namespace SceneGraph
{

	/// <summary>
	/// A scene graph node which organizes it's childs in layers
	/// You can put a child in a layer. The layers are drawn in ascending order
	/// ("You can place stuff in foreground and background layers")
	/// </summary>
	public sealed class Layer : Node
	{
		private SortedList Layers = new SortedList();

		public void Add(float Layer, Node Child)
		{
			if(Layers[Layer] == null)
				Layers[Layer] = new List<Node>();
			if(Child == null)
				throw new Exception("Somebody has passed Child==null to Layer.Add");
			List<Node> Childs = (List<Node>) Layers[Layer];
			Childs.Add(Child);
		}

		public void Remove(float Layer, Node Child)
		{
			if(Layers[Layer] == null)
				throw new Exception("Failed to Remove SceneGraph node - No object was found in layer \""+ Layer.ToString() +"\"");
			if(Child == null)
				throw new Exception("Somebody has passed Child==null to Layer.Remove");
			List<Node> Childs = (List<Node>) Layers[Layer];
			Childs.Remove(Child);
		}

		public void Clear()
		{
			Layers.Clear();
		}

		public void Draw(Gdk.Rectangle cliprect)
		{
			foreach(DictionaryEntry Entry in Layers) {
				List<Node> List = (List<Node>) Entry.Value;
				foreach(Node Child in List) {
					Child.Draw(cliprect);
				}
			}
		}
	}

}

/* EOF */
