using System.Collections.Generic;

namespace SceneGraph
{

	public class NodeWithChilds : Node
	{
		private List<Node> Childs = new List<Node>();

		protected void DrawChilds()
		{
			foreach(Node Child in Childs) {
				Child.Draw();
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

		public virtual void Draw()
		{
			DrawChilds();
		}
	}

}
