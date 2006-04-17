using DataStructures;
using SceneGraph;

/**
 * Object which is draw and occupies an area in the sector
 * TODO: think of a better name for this...
 */
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
