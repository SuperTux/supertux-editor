using SceneGraph;
using Drawing;
using DataStructures;
using System.Collections;
using Gtk;

public class SectorRenderer : RenderView
{
	private Hashtable colors = new Hashtable();
	private ColorNode objectsColorNode;
	private NodeWithChilds objectsNode;
	private SceneGraph.Rectangle sectorBBox;
	private SceneGraph.Rectangle sectorFill;
	
	public SectorRenderer(Level level, Sector sector)
	{
		Layer layer = new Layer();
		
		foreach(Tilemap tilemap in sector.GetObjects(typeof(Tilemap))) {
			Node node = new TilemapNode(tilemap, level.Tileset);
			ColorNode colorNode = new ColorNode(node, new Color(1f, 1f, 1f, 1f));
			layer.Add(tilemap.Layer, colorNode);
			colors[tilemap] = colorNode;
		}
		
		objectsNode = new NodeWithChilds();
		objectsColorNode = new ColorNode(objectsNode, new Color(1f, 1f, 1f, 1f));
		layer.Add(1, objectsColorNode);
		
		foreach(IObject Object in sector.GetObjects(typeof(IObject))) {
			Node node = Object.GetSceneGraphNode();
			if(node != null)
				objectsNode.AddChild(node);
		}
		
		// draw border around sector...
		sectorFill = new SceneGraph.Rectangle();
		sectorFill.Fill = true;
		ColorNode color = new ColorNode(sectorFill, new Drawing.Color(0.4f, 0.3f, 0.4f));		
		layer.Add(-10000, color);
		
		sectorBBox = new SceneGraph.Rectangle();
		sectorBBox.Fill = false;
		color = new ColorNode(sectorBBox, new Drawing.Color(1, 0.3f, 1));
		layer.Add(1000, color);
		
		OnSizeChanged(sector);
		
		this.SceneGraphRoot = layer;
		
		sector.ObjectAdded += OnObjectAdded;
		sector.ObjectRemoved += OnObjectRemoved;
		sector.SizeChanged += OnSizeChanged;
		
		Drag.DestSet(this, DestDefaults.All, ObjectListWidget.DragTargetEntries, Gdk.DragAction.Default);
		DragMotion += OnDragMotion;		
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
		
		// TODO handle tilemaps
	}
	
	private void OnObjectRemoved(Sector sector, IGameObject Object)
	{
		if(! (Object is IObject))
			return;
		
		IObject iObject = (IObject) Object;
		Node node = iObject.GetSceneGraphNode();
		if(node != null)
			objectsNode.RemoveChild(node);
		
		// TODO handle tilemaps
	}
	
	private void OnDragMotion(object o, DragMotionArgs args)
	{
		System.Console.WriteLine("Motion: " + args.X + " - " + args.Y);
		//Console.WriteLine("Blup: " + args.Context
	}
	
	public void OnSizeChanged(Sector sector)
	{
		uint width = 0;
		uint height = 0;
		foreach(Tilemap tilemap in sector.GetObjects(typeof(Tilemap))) {
			if(tilemap.Width > width)
				width = tilemap.Width;
			if(tilemap.Height > height)
				height = tilemap.Height;
		}
		
		sectorBBox.Rect = new RectangleF(-1, -1,
		                             width * Tileset.TILE_WIDTH + 1,
		                             height * Tileset.TILE_HEIGHT + 1);		
		sectorFill.Rect = new RectangleF(-1, -1,
		                             width * Tileset.TILE_WIDTH + 1,
		                             height * Tileset.TILE_HEIGHT + 1);
	}
}
