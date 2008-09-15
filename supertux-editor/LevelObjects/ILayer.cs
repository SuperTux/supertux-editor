//  $Id$
/// <summary>
/// Interface for all objects that belongs to the LayerList => have Z-Position, now known as Layer.
/// </summary>
public interface ILayer
{
	int Layer {
		get;
	}
	string Name {
		get;
	}
}

//TODO: this name is bad, find better one; It should express, that it can be drawn in Editor but stay short enough..
public interface IDrawableLayer : ILayer
{
	SceneGraph.Node GetSceneGraphNode();
}

