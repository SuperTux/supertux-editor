//  $Id$
using System;
using SceneGraph;
using Drawing;
using DataStructures;
using System.Collections;
using Gtk;
using LispReader;

public sealed class SectorRenderer : RenderView
{
	private Hashtable colors = new Hashtable();
	private ColorNode objectsColorNode;
	private ColorNode backgroundColorNode;
	private NodeWithChilds objectsNode;
	private NodeWithChilds backgroundNode;
	private SceneGraph.Rectangle sectorBBox;
	private SceneGraph.Rectangle sectorFill;
	private IEditorApplication application;
	private Level level;
	private Sector sector;

	public SectorRenderer(IEditorApplication application, Level level, Sector sector)
	{
		this.application = application;
		this.level = level;
		this.sector = sector;
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
		color = new ColorNode(new TilemapBorder(application), new Drawing.Color(1, 1, 0));
		layer.Add(1001, color);

		OnSizeChanged(sector);

		this.SceneGraphRoot = layer;

		sector.ObjectAdded += OnObjectAdded;
		sector.ObjectRemoved += OnObjectRemoved;
		sector.SizeChanged += OnSizeChanged;
		application.TilemapChanged += OnTilemapChanged;
		FieldOrProperty.Lookup(typeof(Tilemap).GetProperty("Layer")).Changed += OnTilemapLayerModified;
	}

	public override void Dispose()
	{
		sector.ObjectAdded -= OnObjectAdded;
		sector.ObjectRemoved -= OnObjectRemoved;
		sector.SizeChanged -= OnSizeChanged;
		application.TilemapChanged -= OnTilemapChanged;		
		FieldOrProperty.Lookup(typeof(Tilemap).GetProperty("Layer")).Changed -= OnTilemapLayerModified;
	}

	public Color GetTilemapColor(Tilemap tilemap)
	{
		return ((ColorNode) colors[tilemap]).Color;
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

	public Color GetBackgroundColor()
	{
		return backgroundColorNode.Color;
	}

	public void SetBackgroundColor(Color color)
	{
		backgroundColorNode.Color = color;
		QueueDraw();
	}

	public Color GetObjectsColor()
	{
		return objectsColorNode.Color;
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
			layer.Add(tilemap.Layer, colorNode);
			LogManager.Log(LogLevel.Debug, "Adding tilemap color: {0}", Object.GetHashCode());
			colors[tilemap] = colorNode;
		}

		if (Object is Background) {
			Background background = (Background) Object;

			Node mynode = background.GetSceneGraphNode();
			if(mynode != null) {
				backgroundNode.AddChild(mynode);
			}
		}
	}

	private void OnObjectRemoved(Sector sector, IGameObject Object)
	{
		//handle tilemaps
		if( Object is Tilemap ){
			Layer layer = (Layer) SceneGraphRoot;
			Tilemap tm = (Tilemap) Object;
			layer.Remove(tm.Layer, (ColorNode) colors[tm]);
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

	/// <summary> Moves tilemap from layer to layer when ZPos is changed. </summary>
	private void OnTilemapLayerModified(object Object, FieldOrProperty field, object oldValue)
	{
		if (colors.ContainsKey(Object)){	//is is our tilemap => our sector?
			Layer layer = (Layer) SceneGraphRoot;
			Tilemap tm = (Tilemap) Object;
			ColorNode color = (ColorNode) colors[tm];
			int oldLayer = (int) oldValue;

			layer.Remove(oldLayer, color);
			layer.Add(tm.Layer, color);

			QueueDraw();
		}
	}

	public void OnSizeChanged(Sector sector)
	{
		sectorBBox.Rect = new RectangleF(-1, -1,
		                                 sector.Width * Tileset.TILE_WIDTH + 1,
		                                 sector.Height * Tileset.TILE_HEIGHT + 1);
		sectorFill.Rect = new RectangleF(-1, -1,
		                                 sector.Width * Tileset.TILE_WIDTH + 1,
		                                 sector.Height * Tileset.TILE_HEIGHT + 1);

		minx = -500;
		maxx = sector.Width * Tileset.TILE_WIDTH + 500;
		miny = -500;
		maxy = sector.Height * Tileset.TILE_HEIGHT + 500;
	}

	public void OnTilemapChanged(Tilemap newTilemap)
	{
		QueueDraw();
	}
}
