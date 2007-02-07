//  $Id$
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
