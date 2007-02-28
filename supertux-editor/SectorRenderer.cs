//  $Id$
using System;
using SceneGraph;
using Drawing;
using DataStructures;
using System.Collections;
using Gtk;

public sealed class SectorRenderer : RenderView
{
	private Hashtable colors = new Hashtable();
	private ColorNode objectsColorNode;
	private ColorNode backgroundColorNode;
	private NodeWithChilds objectsNode;
	private NodeWithChilds backgroundNode;
	private SceneGraph.Rectangle sectorBBox;
	private SceneGraph.Rectangle sectorFill;
	private Level level;

	public SectorRenderer(Level level, Sector sector)
	{
		this.level = level;
		Layer layer = new Layer();

		backgroundNode = new NodeWithChilds();
		backgroundColorNode = new ColorNode(backgroundNode, new Color(1f, 1f, 1f, 1f));
		layer.Add(-900, backgroundColorNode);
		foreach(Background background in sector.GetObjects(typeof(Background))) {
			Node node = background.GetSceneGraphNode();
			if(node == null) continue;
			backgroundNode.AddChild(node);
		}

		foreach(Tilemap tilemap in sector.GetObjects(typeof(Tilemap))) {
			Node node = new TilemapNode(tilemap, level.Tileset);
			ColorNode colorNode = new ColorNode(node, new Color(1f, 1f, 1f, 1f), true);
			layer.Add(tilemap.ZPos, colorNode);
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

	/// <summary>
	///		Change color of a tilemap. Useful to hide tilemaps (but they are still drawn that way...)
	/// </summary>
	/// <remarks>
	///		Used to hide tilemaps in <see cref="LayerListWidget.OnVisibilityChange"/>.
	/// </remarks>
	/// <param name="tilemap">The tilemap to change color of.</param>
	/// <param name="color">The new color.</param>
	public void SetTilemapColor(Tilemap tilemap, Color color)
	{
		LogManager.Log(LogLevel.Debug, "Set color of tilemap {0}", tilemap.GetHashCode());
		ColorNode colorNode = (ColorNode) colors[tilemap];
		colorNode.Color = color;
		QueueDraw();
	}

	public void SetBackgroundColor(Color color)
	{
		backgroundColorNode.Color = color;
		QueueDraw();
	}

	public void SetObjectsColor(Color color)
	{
		objectsColorNode.Color = color;
		QueueDraw();
	}

	private void OnObjectAdded(Sector sector, IGameObject Object)
	{
		if(Object is IObject) {
			IObject iObject = (IObject) Object;
			Node node = iObject.GetSceneGraphNode();
			if(node != null)
				objectsNode.AddChild(node);
		}

		Layer layer = (Layer) SceneGraphRoot;

		if(Object is Tilemap) {
			Tilemap tilemap = (Tilemap) Object;
			Node tnode = new TilemapNode(tilemap, level.Tileset);
			ColorNode colorNode = new ColorNode(tnode, new Color(1f, 1f, 1f, 1f));
			layer.Add(tilemap.ZPos, colorNode);
			LogManager.Log(LogLevel.Debug, "Adding tilemap color: {0}", Object.GetHashCode());
			colors[tilemap] = colorNode;
		}

		if (Object is Background) {
			Background background = (Background) Object;

			Node mynode = background.GetSceneGraphNode();
			if(mynode != null) {
				ColorNode colorNode = new ColorNode(mynode, new Color(1f, 1f, 1f, 1f));
				layer.Add(background.Layer, colorNode);
			}
		}
	}

	private void OnObjectRemoved(Sector sector, IGameObject Object)
	{
		//handle tilemaps
		if( Object is Tilemap ){
			Tilemap tm = (Tilemap) Object;
			tm.Resize( 0, 0, 0);
			QueueDraw();
			return;
		}
		//handle backgrounds
		if( Object is Background ){
			Background bg = (Background) Object;
			bg.Image = String.Empty;
			QueueDraw();
			return;
		}

		if(! (Object is IObject)){
			LogManager.Log(LogLevel.Error, "SectorRenderer:OnObjectRemoved unhandled object " + Object);
			return;
		}
		IObject iObject = (IObject) Object;
		Node node = iObject.GetSceneGraphNode();
		if(node != null)
			objectsNode.RemoveChild(node);
	}

	private void OnDragMotion(object o, DragMotionArgs args)
	{
		LogManager.Log(LogLevel.Debug, "Motion: " + args.X + " - " + args.Y);
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
		
		minx = -500;
		maxx = width * Tileset.TILE_WIDTH + 500;
		miny = -500;
		maxy = height * Tileset.TILE_HEIGHT + 500;
	}
}
