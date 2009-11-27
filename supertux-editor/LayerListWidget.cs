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
using System.Collections;
using System.Collections.Generic;
using Gtk;
using GLib;
using Drawing;
using LispReader;
using Undo;

public class LayerListWidget : TreeView {
	private Application application;
	private static object separatorObject = new System.Object();
	private static object badguysObject = new System.Object();
	private Sector sector;
	private Dictionary<object, float> visibility = new Dictionary<object, float>();

	private class VisibilityRenderer : CellRendererPixbuf
	{
		public VisibilityRenderer()
		{
			Mode = CellRendererMode.Editable;
		}

		public override CellEditable StartEditing(Gdk.Event evnt,
		                                          Widget widget,
		                                          string path,
		                                          Gdk.Rectangle background_area,
		                                          Gdk.Rectangle cell_area,
		                                          CellRendererState flags) {
			if(VisibilityChanged != null)
				VisibilityChanged(this, null);
			return null;
		}

		public delegate void VisibilityChangedHandler(object o, EventArgs args);

		public event VisibilityChangedHandler VisibilityChanged;
	}

	public LayerListWidget(Application application)
	{
		this.application = application;
		RowSeparatorFunc = OurRowSeparatorFunc;
		ButtonPressEvent += OnButtonPressed;

		VisibilityRenderer visibilityRenderer = new VisibilityRenderer();
		visibilityRenderer.VisibilityChanged += OnVisibilityChange;
		TreeViewColumn visibilityColumn = new TreeViewColumn("Visibility",
		                                                     visibilityRenderer);
		visibilityColumn.SetCellDataFunc(visibilityRenderer, (TreeCellDataFunc)VisibilityDataFunc);
		AppendColumn(visibilityColumn);

		CellRendererText TextRenderer = new CellRendererText();
		TreeViewColumn TypeColumn = new TreeViewColumn();
		TypeColumn.PackStart(TextRenderer, true);
		TypeColumn.SetCellDataFunc(TextRenderer, (TreeCellDataFunc)TextDataFunc);
		TypeColumn.Title = "Type";
		AppendColumn(TypeColumn);

		HeadersVisible = false;

		application.SectorChanged += OnSectorChanged;
		application.TilemapChanged += OnTilemapChanged;
		application.LevelChanged += OnLevelChanged;

		FieldOrProperty.AnyFieldChanged += OnFieldModified;
	}

	private void OnFieldModified(object Object, FieldOrProperty field, object oldValue)
	{
		if (! (Object is ILayer)) //ignore changes on other objects
			return;
		if (! (field.Name=="Layer" || field.Name=="Name")) //ignore changes on other fields
			return;

		//TODO: Is that sorting execute-once or what? (found no other working way)
		TreeStore store = (TreeStore) Model;
		if (store != null)
			store.SetSortFunc( 0, compareLayer );
		QueueDraw();
	}

	private void OnSectorChanged(Level level, Sector sector)
	{
		sector.ObjectAdded -= ObjectsChanged;
		sector.ObjectRemoved -= ObjectsChanged;

		this.sector = sector;

		//Find and select first solid tilemap (and discard tilemap from level opened before).
		application.CurrentTilemap = null;

		sector.ObjectAdded += ObjectsChanged;
		sector.ObjectRemoved += ObjectsChanged;
		UpdateList();
	}

	private void ObjectsChanged(Sector sector, IGameObject Object)
	{
		if(! (Object is ILayer))
			return;

		UpdateList();
	}

	/// <summary>Called when a new level is loaded</summary>
	private void OnLevelChanged(Level level)
	{
		//Find and select first solid tilemap (and discard tilemap from level opened before).
		application.CurrentTilemap = null;
	}


	/// <summary> Get ZPos Value for object, non-tilemaps last </summary>
	private int getZPos (object o) {
		if(o is ILayer) {
			ILayer ILayer = (ILayer) o;
			return ILayer.Layer;
		}
		return int.MaxValue;
	}

	/// <summary> Compare ZPos Values and return which comes first </summary>
	public int compareLayer(TreeModel model, TreeIter tia, TreeIter tib){
		object objA = model.GetValue (tia, 0);
		object objB = model.GetValue (tib, 0);
		int a = getZPos(objA);
		int b = getZPos(objB);
		return a.CompareTo(b);
     }

	private void UpdateList()
	{
		visibility.Clear();
		TreeStore store = new TreeStore(typeof(System.Object));
		foreach(ILayer ILayer in sector.GetObjects(typeof(ILayer))) {
			store.AppendValues(ILayer);
			if (ILayer is IDrawableLayer) {
				visibility[ILayer] = application.CurrentRenderer.GetILayerColor(ILayer).Alpha;

				if (ILayer is Tilemap) {
					Tilemap Tilemap = (Tilemap) ILayer;
					// if no tilemap is yet selected, select the first solid one
					if ((application.CurrentTilemap == null) && (Tilemap.Solid)) {
						application.CurrentTilemap = Tilemap;
						string name = (String.IsNullOrEmpty(Tilemap.Name))?"":" \"" +Tilemap.Name + "\"";
						application.EditProperties(application.CurrentTilemap, "Tilemap" + name + " (" + application.CurrentTilemap.Layer + ")");
					}
				}
			}
		}
		store.SetSortFunc( 0, compareLayer );
		store.SetSortColumnId( 0, SortType.Ascending );
		store.AppendValues(separatorObject);
		visibility[separatorObject] = 0;

		store.AppendValues(badguysObject);
		visibility[badguysObject] = application.CurrentRenderer.GetObjectsColor().Alpha;
		Model = store;

		// Visibly select current Tilemap
		if (application.CurrentTilemap != null) {
			TreePath path = TreePath.NewFirst();
			TreeIter iter;
			while (Model.GetIter(out iter, path)) {
				object obj = Model.GetValue(iter, 0);
				if(obj == application.CurrentTilemap) {
					HasFocus = true;
					ActivateRow(path, GetColumn(0));
					SetCursor(path, GetColumn(0), false);
				}
				path.Next();
			}
		}

	}

	private void OnTilemapChanged(Tilemap Tilemap)
	{
		LogManager.Log(LogLevel.Debug, "LayerListWidget.cs OnTilemapChanged: Not implemented");
	}

	private bool OurRowSeparatorFunc (TreeModel model, TreeIter iter)
	{
		object o = model.GetValue(iter, 0);
		return (o == separatorObject);
	}

	private void VisibilityDataFunc(TreeViewColumn Column, CellRenderer Renderer,
	                                TreeModel Model, TreeIter Iter)
	{
		CellRendererPixbuf PixbufRenderer = (CellRendererPixbuf) Renderer;

		object o = Model.GetValue(Iter, 0);

		if (o is ILayer && !(o is IDrawableLayer)) {	//no visibility for objects that we can't currently display
			PixbufRenderer.StockId = null;
			return;
		}

		float vis = visibility[o];
		if(vis <= 0) {
			PixbufRenderer.StockId = null;
		} else if(vis <= 0.5f) {
			PixbufRenderer.StockId = EditorStock.EyeHalf;
		} else {
			PixbufRenderer.StockId = EditorStock.Eye;
		}
	}

	private void TextDataFunc(TreeViewColumn Column, CellRenderer Renderer,
	                          TreeModel Model, TreeIter Iter)
	{
		object o = Model.GetValue(Iter, 0);

		CellRendererText TextRenderer = (CellRendererText) Renderer;
		if(o is ILayer) {
			ILayer ILayer = (ILayer) o;
			if (ILayer.Name.Length == 0) {
				TextRenderer.Text = ILayer.GetType().Name + " (" + ILayer.Layer + ")";
			} else {
				TextRenderer.Text = ILayer.Name + " (" + ILayer.Layer + ")";
			}
		} else {
			if (o == badguysObject)
				TextRenderer.Text = "Objects";
			else
				TextRenderer.Text = "UNKNOWN";
		}
	}

	[GLib.ConnectBefore]
	private void OnButtonPressed(object o, ButtonPressEventArgs args)
	{
		TreePath path;
		if(!GetPathAtPos((int) args.Event.X, (int) args.Event.Y, out path))
			return;

		TreeIter iter;
		if(!Model.GetIter(out iter, path))
			return;

		object obj = Model.GetValue(iter, 0);
		if(obj is Tilemap) {
			if (visibility[obj]>0) {		//set null tilemap when selected one is invisible
				application.CurrentTilemap = (Tilemap) obj;
			} else {
				application.CurrentTilemap = null;
			}
		} else {
			if (obj == separatorObject)
				return;
			application.CurrentTilemap = null;
		}

		if (obj is ILayer) {
			ILayer ILayer = (ILayer) obj;
			if (ILayer != null) {		//open it's properties if any
				string name = (String.IsNullOrEmpty(ILayer.Name))?"":" \"" +ILayer.Name + "\"";
				application.EditProperties(ILayer, ILayer.GetType().Name + name + " (" + ILayer.Layer.ToString() + ")");
			}
		}

		if ((args.Event.Button == 3) && (obj is ILayer)) {
			ShowPopupMenu(obj as ILayer);
		}
	}

	private void ShowPopupMenu(ILayer layer)
	{
		Menu popupMenu = new Menu();

		MenuItem addItem = new ImageMenuItem(Stock.Add, null);
		addItem.Activated += OnAdd;
		popupMenu.Append(addItem);

		if (layer is Tilemap) {
			MenuItem resizeItem = new MenuItem("Resize");
			resizeItem.Activated += OnResize;
			popupMenu.Append(resizeItem);

			MenuItem editPathItem = new MenuItem("Edit Path");
			editPathItem.Activated += OnEditPath;
			popupMenu.Append(editPathItem);

			MenuItem deletePathItem = new MenuItem("Delete Path");
			deletePathItem.Sensitive = application.CurrentTilemap.Path != null;
			deletePathItem.Activated += OnDeletePath;
			popupMenu.Append(deletePathItem);

			MenuItem CheckIDsItem = new MenuItem("Check tile IDs");
			CheckIDsItem.Activated += OnCheckIDs;
			popupMenu.Append(CheckIDsItem);
		}

		MenuItem deleteItem = new ImageMenuItem(Stock.Delete, null);
		if (layer is Tilemap) {
			deleteItem.Sensitive = sector.GetObjects(typeof(Tilemap)).Count > 1;
		}
		deleteItem.Activated += OnDelete;
		popupMenu.Append(deleteItem);

		popupMenu.ShowAll();
		popupMenu.Popup();
	}

	private void OnAdd(object o, EventArgs args)
	{
		Tilemap tilemap = new Tilemap();
		tilemap.Resize( sector.Width, sector.Height, 0);
		sector.Add(tilemap, "Tilemap");
	}

	private void OnResize(object o, EventArgs args)
	{
		try {
			new ResizeDialog(sector, application.CurrentTilemap);
		} catch(Exception e) {
			ErrorDialog.Exception(e);
		}
	}

	private void OnEditPath(object o, EventArgs args)
	{
		application.SetToolPath();					//iPathToEdit is set when calling "EditProperties()" and already contains active Tilemap
	}

	private void OnDeletePath(object o, EventArgs args)
	{
		IPathObject pathObject = (IPathObject) application.CurrentTilemap;
		if (pathObject.Path != null) {
			Command command = new PropertyChangeCommand("Removed path of Tilemap " + application.CurrentTilemap.Name + " (" + application.CurrentTilemap.Layer + ")",
				FieldOrProperty.Lookup(typeof(Tilemap).GetProperty("Path")),
				application.CurrentTilemap,
				null);
			command.Do();
			UndoManager.AddCommand(command);
		}
	}

	private void OnDelete(object o, EventArgs args)
	{
		TreeIter treeIter;
		TreeModel treeModel;
		if (Selection.GetSelected(out treeModel, out treeIter))
		{	//we have selected row
			IGameObject obj = (IGameObject) treeModel.GetValue(treeIter, 0);
			if (obj == null)
				return;

			// Don't remove last tilemap, that cause bugs.
			if (obj is Tilemap && sector.GetObjects(typeof(Tilemap)).Count == 1)
				return;

			sector.Remove(obj);
			if (obj is Tilemap) application.CurrentTilemap = null;
			UpdateList();
		}
	}

	private void OnCheckIDs(object o, EventArgs args) {
		if (application.CurrentTilemap == null)
			return;

		List<int> invalidtiles = QACheck.CheckIds(application.CurrentTilemap, application.CurrentLevel.Tileset);
		MessageType msgtype;
		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		if (invalidtiles.Count == 0) {
			msgtype = MessageType.Info;
			sb.Append("All tile ids are valid");
		} else {
			msgtype = MessageType.Warning;
			sb.Append("This tilemap contains tiles with these nonexistent IDs:");
			foreach (int id in invalidtiles) {
				sb.Append(" " + id.ToString());
			}
		}
		MessageDialog md = new MessageDialog(null, DialogFlags.DestroyWithParent,
		                                     msgtype, ButtonsType.Close, sb.ToString());
		md.Run();
		md.Destroy();
	}

	private void OnVisibilityChange(object o, EventArgs args)
	{
		TreeIter treeIter;
		TreeModel treeModel;
		if (Selection.GetSelected(out treeModel, out treeIter))
		{	//we have selected row
			object obj = treeModel.GetValue(treeIter, 0);

			if (obj is ILayer && !(obj is IDrawableLayer))	//skip it, if we can't currently display that object
				return;

			float vis = visibility[obj];
			float newvis = 1.0f;
			if(vis == 1.0f) {
				newvis = 0.5f;
			} else if(vis == 0.5f) {
				newvis = 0.0f;
			} else {
				newvis = 1.0f;
			}

			if (obj is ILayer)
				application.CurrentRenderer.SetILayerColor((ILayer)obj,
			                                            new Color(1, 1, 1, newvis));
			if (obj == badguysObject)
				application.CurrentRenderer.SetObjectsColor(new Color(1, 1, 1, newvis));

			visibility[obj] = newvis;

			if (obj is Tilemap) {				//Selecting and deselecting for invisible layers
				if ( (application.CurrentTilemap == obj) && newvis == 0)	//deselect active tilemap that is made invisible
					application.CurrentTilemap = null;
				if ( (application.CurrentTilemap == null) && newvis != 0)	//select un-invisibled tilemap if we have no active one
					application.CurrentTilemap = (Tilemap) obj;
			}

			QueueDraw();
		}
	}
}

/* EOF */
