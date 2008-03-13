//  $Id$
using System;
using Drawing;
using DataStructures;
using System.Collections;
using Gtk;

public sealed class SectorRenderer : RenderView
{
	private Hashtable colors = new Hashtable();
	//private ColorNode objectsColorNode;
	private RectangleF sectorBBox = new RectangleF();
	private RectangleF sectorFill = new RectangleF();
	private Level  level;
	private Sector sector;

	public SectorRenderer(Level level, Sector sector)
	{
		this.level  = level;
		this.sector = sector;

		/*
		foreach(Tilemap tilemap in sector.GetObjects(typeof(Tilemap))) {
			Node node = new TilemapNode(tilemap.Tiles, level.Tileset);
			ColorNode colorNode = new ColorNode(node, new Color(1f, 1f, 1f, 1f), true);
			layer.Add(tilemap.ZPos, colorNode);
			colors[tilemap] = colorNode;
		}

		*/

		OnSizeChanged(sector);

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
		/*
		ColorNode colorNode = (ColorNode) colors[tilemap];
		colorNode.Color = color;
		*/
		QueueDraw();
	}

	public void SetObjectsColor(Color color)
	{
		//objectsColorNode.Color = color;
		QueueDraw();
	}

	private void OnObjectAdded(Sector sector, IGameObject Object)
	{
		QueueDraw();
	}

	private void OnObjectRemoved(Sector sector, IGameObject Object)
	{
		QueueDraw();
	}

	private void OnDragMotion(object o, DragMotionArgs args)
	{
		LogManager.Log(LogLevel.Debug, "Motion: " + args.X + " - " + args.Y);
	}

	public override void DrawContents(DrawingContext context)
	{
		if(sector == null)
			return;

		sector.Draw(context);
		/* TODO: draw sector surroundings */
		/* TODO 
			// draw border around sector...
		sectorFill = new SceneGraph.Rectangle();
		sectorFill.Fill = true;
		ColorNode color = new ColorNode(sectorFill, new Drawing.Color(0.4f, 0.3f, 0.4f));
		layer.Add(-10000, color);

		sectorBBox = new SceneGraph.Rectangle();
		sectorBBox.Fill = false;
		color = new ColorNode(sectorBBox, new Drawing.Color(1, 0.3f, 1));
		layer.Add(1000, color);
	sectorBBox.Rect = new RectangleF(-1, -1,
		                                 width * tile_width + 1,
		                                 height * tile_height + 1);
		sectorFill.Rect = new RectangleF(-1, -1,
		                                 width * tile_width + 1,
		                                 height * tile_height + 1);
		*/
	}

	public void OnSizeChanged(Sector sector)
	{
		Tileset tileset     = level.Tileset;
		uint    tile_width  = tileset.TILE_WIDTH;
		uint    tile_height = tileset.TILE_HEIGHT;
		uint    width       = 0;
		uint    height      = 0;
		foreach(Tilemap tilemap in sector.GetObjects(typeof(Tilemap))) {
			if(tilemap.Tiles.Width > width)
				width = tilemap.Tiles.Width;
			if(tilemap.Tiles.Height > height)
				height = tilemap.Tiles.Height;
		}

		minx = -500;
		maxx = width * tile_width + 500;
		miny = -500;
		maxy = height * tile_height + 500;
	}
}
