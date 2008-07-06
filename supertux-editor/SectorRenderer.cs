//  $Id$
using System;
using SceneGraph;
using Drawing;
using DataStructures;
using System.Collections;
using Gtk;

public sealed class SectorRenderer : RenderView
{
	public static TargetEntry [] DragTargetEntries = new TargetEntry[] {
		new TargetEntry("GameObject", TargetFlags.App, 0)
	};

	private Hashtable colors = new Hashtable();
	private ColorNode objectsColorNode;
	private ColorNode backgroundColorNode;
	private NodeWithChilds objectsNode;
	private NodeWithChilds backgroundNode;
	private SceneGraph.Rectangle sectorBBox;
	private SceneGraph.Rectangle tilemapBBox;
	private SceneGraph.Rectangle sectorFill;
	private Level level;
	private Tilemap currentTilemap;

	public SectorRenderer(IEditorApplication application, Level level, Sector sector)
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

		// fill remaining place with one color
		sectorFill = new SceneGraph.Rectangle();
		sectorFill.Fill = true;
		ColorNode color = new ColorNode(sectorFill, new Drawing.Color(0.4f, 0.3f, 0.4f));
		layer.Add(-10000, color);

		// draw border around sector...
		sectorBBox = new SceneGraph.Rectangle();
		sectorBBox.Fill = false;
		color = new ColorNode(sectorBBox, new Drawing.Color(1, 0.3f, 1));
		layer.Add(1000, color);

		// draw border around selected layer...
		tilemapBBox = new SceneGraph.Rectangle();
		tilemapBBox.Fill = false;
		color = new ColorNode(tilemapBBox, new Drawing.Color(1, 1, 0));
		layer.Add(1001, color);

		OnSizeChanged(sector);

		this.SceneGraphRoot = layer;

		sector.ObjectAdded += OnObjectAdded;
		sector.ObjectRemoved += OnObjectRemoved;
		sector.SizeChanged += OnSizeChanged;
		application.TilemapChanged += OnTilemapChanged;

		Drag.DestSet(this, DestDefaults.All, DragTargetEntries, Gdk.DragAction.Default);
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
			Layer layer = (Layer) SceneGraphRoot;
			Tilemap tm = (Tilemap) Object;
			layer.Remove(tm.ZPos, (ColorNode) colors[tm]);
			colors.Remove(tm);
			QueueDraw();
			return;
		}
		//handle backgrounds
		if( Object is Background ){
			Node bgNode = ((Background) Object).GetSceneGraphNode();
			backgroundNode.RemoveChild(bgNode);
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
		sectorBBox.Rect = new RectangleF(-1, -1,
		                                 sector.Width * Tileset.TILE_WIDTH + 1,
		                                 sector.Height * Tileset.TILE_HEIGHT + 1);
		sectorFill.Rect = new RectangleF(-1, -1,
		                                 sector.Width * Tileset.TILE_WIDTH + 1,
		                                 sector.Height * Tileset.TILE_HEIGHT + 1);

		UpdateTilemapBox(currentTilemap);

		minx = -500;
		maxx = sector.Width * Tileset.TILE_WIDTH + 500;
		miny = -500;
		maxy = sector.Height * Tileset.TILE_HEIGHT + 500;
	}

	public void UpdateTilemapBox(Tilemap tilemap)
	{
		if (tilemap == null)
			tilemapBBox.Rect = new RectangleF(0, 0, 0, 0);	//hide the border
		else
			tilemapBBox.Rect = new RectangleF(-1, -1,
		                                 tilemap.Width * Tileset.TILE_WIDTH + 1,
		                                 tilemap.Height * Tileset.TILE_HEIGHT + 1);

		if (tilemapBBox.Rect.Equals(sectorBBox.Rect)) //If we have full-sized tilemap selected...
			tilemapBBox.Rect = new RectangleF(0, 0, 0, 0);	//...we hide the border.		
	}

	public void OnTilemapChanged(Tilemap newTilemap)
	{
		currentTilemap = newTilemap;
		UpdateTilemapBox(currentTilemap);
		QueueDraw();
	}
}
