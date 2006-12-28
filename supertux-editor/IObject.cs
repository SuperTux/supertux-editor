//  $Id$
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
