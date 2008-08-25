//  $Id$
using System;
using System.Collections;
using System.Collections.Generic;
using Gtk;
using GLib;
using Drawing;
using LispReader;
using Undo;

public class LayerListWidget : TreeView {
	private IEditorApplication application;
	private static object separatorObject = new System.Object();
	private static object badguysObject = new System.Object();
	private static object backgroundObject = new System.Object();
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

	public LayerListWidget(IEditorApplication application)
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
	}

	private void OnSectorChanged(Level level, Sector sector)
	{
		sector.ObjectAdded -= ObjectsChanged;
		sector.ObjectRemoved -= ObjectsChanged;

		this.sector = sector;

		//Find and select first solid tilemap (and discard tilemap from level opened before).
		application.CurrentTilemap = null;

		sector.ObjectAdded += ObjectsChanged;
		sector.ObjectAdded += ObjectsChanged;
		UpdateList();
	}

	private void ObjectsChanged(Sector sector, IGameObject Object)
	{
		if(! (Object is Tilemap))
			return;

		UpdateList();
	}

	/// <summary>Called when a new level is loaded</summary>
	private void OnLevelChanged(Level level)
	{
		//Find and select first solid tilemap (and discard tilemap from level opened before).
		application.CurrentTilemap = null;
	}

	/// <summary> Compare ZPos Values of Tilemaps, non Tilemap layers last </summary>
    public int compareZPos(TreeModel model, TreeIter tia, TreeIter tib){
		object objA = model.GetValue (tia, 0);
		object objB = model.GetValue (tib, 0);
		int a = int.MaxValue; 
		int b =	int.MaxValue;
		if(objA is Tilemap) {
			Tilemap Tilemap = (Tilemap) objA;
			a = Tilemap.ZPos;
		}	
		if(objB is Tilemap) {
			Tilemap Tilemap = (Tilemap) objB;
			b = Tilemap.ZPos;
		}		
		return a - b; 
     }	
	
	private void UpdateList()
	{
		visibility.Clear();
		TreeStore store = new TreeStore(typeof(System.Object));
		foreach(Tilemap Tilemap in sector.GetObjects(typeof(Tilemap))) {
			store.AppendValues(Tilemap);
			visibility[Tilemap] = application.CurrentRenderer.GetTilemapColor(Tilemap).Alpha;

			// if no tilemap is yet selected, select the first solid one
			if ((application.CurrentTilemap == null) && (Tilemap.Solid)) {
				application.CurrentTilemap = Tilemap;
				application.EditProperties(application.CurrentTilemap, "Tilemap (" + application.CurrentTilemap.ZPos + ")");
			}

		}
		store.SetSortFunc( 0, compareZPos );
		store.SetSortColumnId( 0, SortType.Ascending );
		store.AppendValues(separatorObject);
		visibility[separatorObject] = 0;

		store.AppendValues(backgroundObject);
		visibility[backgroundObject] = application.CurrentRenderer.GetBackgroundColor().Alpha;

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
		if(o is Tilemap) {
			Tilemap Tilemap = (Tilemap) o;
			if (Tilemap.Name.Length == 0) {
				TextRenderer.Text = "Tilemap (" + Tilemap.ZPos + ")";
			} else {
				TextRenderer.Text = Tilemap.Name + " (" + Tilemap.ZPos + ")";
			}
		} else {
			if (o == badguysObject)
				TextRenderer.Text = "Objects";
			else
				TextRenderer.Text = "Background image";
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
			if(obj != application.CurrentTilemap) {
				application.CurrentTilemap = (Tilemap) obj;
				application.EditProperties(application.CurrentTilemap, "Tilemap (" + application.CurrentTilemap.ZPos + ")");
			}
		} else {
			if (obj == separatorObject)
				return;
			application.CurrentTilemap = null;
		}

		if((args.Event.Button == 3) && (obj is Tilemap)) {
			ShowPopupMenu();
		}
	}

	private void ShowPopupMenu()
	{
		Menu popupMenu = new Menu();

		MenuItem addItem = new ImageMenuItem(Stock.Add, null);
		addItem.Activated += OnAdd;
		popupMenu.Append(addItem);

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

		MenuItem deleteItem = new ImageMenuItem(Stock.Delete, null);
		deleteItem.Sensitive = sector.GetObjects(typeof(Tilemap)).Count > 1;
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
		IPathObject pathObject = (IPathObject) application.CurrentTilemap;
		if (pathObject.Path == null) {
			pathObject.Path = new Path();
			pathObject.Path.Nodes.Add(new Path.Node());
		}
		application.SetToolPath(pathObject.Path);
	}

	private void OnDeletePath(object o, EventArgs args)
	{
		IPathObject pathObject = (IPathObject) application.CurrentTilemap;
		if (pathObject.Path != null) {
			Command command = new PropertyChangeCommand("Removed path of Tilemap " + application.CurrentTilemap.Name + " (" + application.CurrentTilemap.ZPos + ")",
				new FieldOrProperty.Property(typeof(Tilemap).GetProperty("Path")),
				application.CurrentTilemap,
				null);
			command.Do();
			UndoManager.AddCommand(command);
		}
	}

	private void OnDelete(object o, EventArgs args)
	{
		if(application.CurrentTilemap == null)
			return;

		// Don't remove last tilemap, that cause bugs.
		if (sector.GetObjects(typeof(Tilemap)).Count == 1)
			return;

		sector.Remove(application.CurrentTilemap);
		application.CurrentTilemap = null;
		UpdateList();
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

			float vis = visibility[obj];
			float newvis = 1.0f;
			if(vis == 1.0f) {
				newvis = 0.5f;
			} else if(vis == 0.5f) {
				newvis = 0.0f;
			} else {
				newvis = 1.0f;
			}

			if (obj is Tilemap)
				application.CurrentRenderer.SetTilemapColor((Tilemap)obj,
			                                            new Color(1, 1, 1, newvis));
			if (obj == badguysObject)
				application.CurrentRenderer.SetObjectsColor(new Color(1, 1, 1, newvis));

			if (obj == backgroundObject) 
				application.CurrentRenderer.SetBackgroundColor(new Drawing.Color(1, 1, 1, newvis));

			visibility[obj] = newvis;
			QueueDraw();
		}
	}
}
