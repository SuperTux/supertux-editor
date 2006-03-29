using DataStructures;
using SceneGraph;

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
