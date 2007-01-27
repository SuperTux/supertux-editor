//  $Id$
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
