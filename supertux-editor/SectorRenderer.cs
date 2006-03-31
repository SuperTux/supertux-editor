using SceneGraph;
using Drawing;
using System.Collections;

public class SectorRenderer : RenderView
{
	private Hashtable colors = new Hashtable();
	private ColorNode objectsColorNode;
	private NodeWithChilds objectsNode;
	
	public SectorRenderer(Level level, Sector sector)
	{
		Layer Layer = new Layer();
		
		foreach(Tilemap tilemap in sector.GetObjects(typeof(Tilemap))) {
			Node node = new TilemapNode(tilemap, level.Tileset);
			ColorNode colorNode = new ColorNode(node, new Color(1f, 1f, 1f, 1f));
			Layer.Add(tilemap.Layer, colorNode);
			colors[tilemap] = colorNode;
		}
		
		objectsNode = new NodeWithChilds();
		objectsColorNode = new ColorNode(objectsNode, new Color(1f, 1f, 1f, 1f));
		Layer.Add(1, objectsColorNode);
		
		foreach(IObject Object in sector.GetObjects(typeof(IObject))) {
			Node node = Object.GetSceneGraphNode();
			if(node != null)
				objectsNode.AddChild(node);
		}
		
		this.SceneGraphRoot = Layer;
		
		sector.ObjectAdded += OnObjectAdded;
		sector.ObjectRemoved += OnObjectRemoved;
	}
	
	public void SetTilemapColor(Tilemap tilemap, Color color)
	{
		ColorNode colorNode = (ColorNode) colors[tilemap];
		colorNode.Color = color;
		QueueDraw();
	}
	
	public void SetObjectsColor(Color color)
	{
		objectsColorNode.Color = color;
		QueueDraw();
	}
	
	private void OnObjectAdded(Sector sector, IGameObject Object)
	{
		if(! (Object is IObject))
			return;
		
		IObject iObject = (IObject) Object;
		Node node = iObject.GetSceneGraphNode();
		if(node != null)
			objectsNode.AddChild(node);
	}
	
	private void OnObjectRemoved(Sector sector, IGameObject Object)
	{
		if(! (Object is IObject))
			return;
		
		IObject iObject = (IObject) Object;
		Node node = iObject.GetSceneGraphNode();
		if(node != null)
			objectsNode.RemoveChild(node);
	}
}
