//  SuperTux Editor
//  Copyright (C) 2006 Matthias Braun <matze@braunis.de>
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

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
	private NodeWithChilds objectsNode;
	private SceneGraph.Rectangle sectorBBox;
	private SceneGraph.Rectangle sectorFill;
	private Application application;
	private Level level;
	private Sector sector;

	public SectorRenderer(Application application, Level level, Sector sector)
	{
		this.application = application;
		this.level = level;
		this.sector = sector;
		Layer layer = new Layer();

		foreach(IDrawableLayer IDrawableLayer in sector.GetObjects(typeof(IDrawableLayer))) {
			Node node = IDrawableLayer.GetSceneGraphNode();
			if (IDrawableLayer is Tilemap)	//Special handling for tilemaps
				node = new TilemapNode((Tilemap) IDrawableLayer, level.Tileset);
			ColorNode colorNode = new ColorNode(node, new Color(1f, 1f, 1f, 1f), true);
			layer.Add(IDrawableLayer.Layer, colorNode);
			colors[IDrawableLayer] = colorNode;
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
		//TODO: It should be possible to iterate over all (currently present?) types that implements ILayer.. How?
		FieldOrProperty.AnyFieldChanged += OnFieldChanged;
	}

	public override void Dispose()
	{
		sector.ObjectAdded -= OnObjectAdded;
		sector.ObjectRemoved -= OnObjectRemoved;
		sector.SizeChanged -= OnSizeChanged;
		application.TilemapChanged -= OnTilemapChanged;
		//TODO: It should be possible to iterate over all (currently present?) types that implements ILayer.. How?
		FieldOrProperty.AnyFieldChanged += OnFieldChanged;
	}

	public Color GetILayerColor(ILayer ILayer)
	{
		return ((ColorNode) colors[ILayer]).Color;
	}

	/// <summary>
	///		Change color of any ILayer . Useful to hide any ILayer objects (they are not drawn, because ColorNodes are set do not-draw-when-transparent)
	/// </summary>
	/// <remarks>
	///		Used to hide ILayer in <see cref="LayerListWidget.OnVisibilityChange"/>.
	/// </remarks>
	/// <param name="ILayer">The ILayer to change color of.</param>
	/// <param name="color">The new color.</param>
	public void SetILayerColor(ILayer ILayer, Color color)
	{
		LogManager.Log(LogLevel.Debug, "Set color of ILayer {0}", ILayer.GetHashCode());
		ColorNode colorNode = (ColorNode) colors[ILayer];
		colorNode.Color = color;
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

		if (Object is IDrawableLayer) {
			IDrawableLayer IDrawableLayer = (IDrawableLayer) Object;

			Node mynode = IDrawableLayer.GetSceneGraphNode();
			if(mynode != null) {
				ColorNode colorNode = new ColorNode(mynode, new Color(1f, 1f, 1f, 1f));
				layer.Add(IDrawableLayer.Layer, colorNode);
				LogManager.Log(LogLevel.Debug, "Adding ILayer color: {0}", Object.GetHashCode());
				colors[IDrawableLayer] = colorNode;
			}
		}
	}

	private void OnObjectRemoved(Sector sector, IGameObject Object)
	{
		//handle drawable ILayers
		if( Object is IDrawableLayer || Object is Tilemap ){
			Layer layer = (Layer) SceneGraphRoot;
			ILayer ILayer = (ILayer) Object;
			layer.Remove(ILayer.Layer, (ColorNode) colors[ILayer]);
			colors.Remove(ILayer);
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

	/// <summary> Moves any ILayer from layer to layer when ZPos is changed. </summary>
	private void OnFieldChanged(object Object, FieldOrProperty field, object oldValue)
	{
		if (! (Object is IGameObject && sector.Contains((IGameObject)Object)))	//return, if it's not (GameObject in our sector)
			return;

		if (Object is ILayer && field.Name == "Layer") { //filter for ILayer.Layer
			Layer layer = (Layer) SceneGraphRoot;
			ILayer ILayer = (ILayer) Object;
			ColorNode color = (ColorNode) colors[ILayer];
			int oldLayer = (int) oldValue;

			layer.Remove(oldLayer, color);
			layer.Add(ILayer.Layer, color);

			QueueDraw();
		}

		PropertyPropertiesAttribute propertyProperties = (PropertyPropertiesAttribute)
				field.GetCustomAttribute(typeof(PropertyPropertiesAttribute));
		if (propertyProperties != null && propertyProperties.RedrawOnChange)	//Every property that affects appearance is marked using this attribute
			QueueDraw();
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

/* EOF */
