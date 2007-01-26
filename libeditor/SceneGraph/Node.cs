//  $Id$
namespace SceneGraph
{
	/// <summary>
	/// This is the basic object of a scene graph: A single node with a Draw()
	/// command. The nodes form a graph (in our case it's a tree). Each node
	/// will trigger Draw() commands on it subnodes.
	/// </summary>
	/// <remarks>
	///   Some introduction to scenegraphs can be found in our wiki:
	///   http://supertux.lethargik.org/wiki/SceneGraph
	/// </remarks>
	public interface Node
	{
		void Draw(Gdk.Rectangle cliprect);
	}

}
