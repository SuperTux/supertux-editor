//  $Id$
using System;
using System.Collections;
using System.Collections.Generic;
using Gtk;
using GLib;
using Drawing;

public class LayerListWidget : TreeView {
	private IEditorApplication application;
	private static object nullObject = new System.Object();
	private Tilemap currentTilemap;
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
	}

	public Tilemap CurrentTilemap {
		get {
			return currentTilemap;
		}
	}

	private void OnSectorChanged(Level level, Sector sector)
	{
		sector.ObjectAdded -= ObjectsChanged;
		sector.ObjectRemoved -= ObjectsChanged;

		this.sector = sector;

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

	private void UpdateList()
	{
		visibility.Clear();
		TreeStore store = new TreeStore(typeof(System.Object));
		foreach(Tilemap Tilemap in sector.GetObjects(typeof(Tilemap))) {
			store.AppendValues(Tilemap);
			visibility[Tilemap] = 1.0f;

			// if no tilemap is yet selected, select the first solid one
			if ((currentTilemap == null) && (Tilemap.Solid)) {
				currentTilemap = Tilemap;
				application.EditProperties(currentTilemap, "Tilemap (" + currentTilemap.ZPos + ")");
				application.ChangeCurrentTilemap(currentTilemap);
			}

		}
		store.AppendValues(nullObject);
		visibility[nullObject] = 1.0f;
		Model = store;

		// Visibly select current Tilemap
		if (currentTilemap != null) {
			TreePath path = TreePath.NewFirst();
			TreeIter iter;
			while (Model.GetIter(out iter, path)) {
				object obj = Model.GetValue(iter, 0);
				if(obj == currentTilemap) {
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
			TextRenderer.Text = "Objects";
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
			if(obj != currentTilemap) {
				currentTilemap = (Tilemap) obj;
				application.EditProperties(currentTilemap, "Tilemap (" + currentTilemap.ZPos + ")");
				application.ChangeCurrentTilemap(currentTilemap);
			}
		} else {
			currentTilemap = null;
	// TODO: clear properties window?
			application.ChangeCurrentTilemap(currentTilemap);
		}

		if((args.Event.Button == 3) && (obj is Tilemap)) {
			ShowPopupMenu();
		}
	}

	private void ShowPopupMenu()
	{
		Menu popupMenu = new Menu();

		MenuItem editPathItem = new MenuItem("Edit Path");
		editPathItem.Activated += OnEditPath;
		popupMenu.Append(editPathItem);

		MenuItem deletePathItem = new MenuItem("Delete Path");
		deletePathItem.Activated += OnDeletePath;
		popupMenu.Append(deletePathItem);

		MenuItem CheckIDsItem = new MenuItem("Check tile IDs");
		CheckIDsItem.Activated += OnCheckIDs;
		popupMenu.Append(CheckIDsItem);

		MenuItem deleteItem = new ImageMenuItem(Stock.Delete, null);
		deleteItem.Activated += OnDelete;
		popupMenu.Append(deleteItem);

		popupMenu.ShowAll();
		popupMenu.Popup();
	}

	private void OnEditPath(object o, EventArgs args)
	{
		IPathObject pathObject = (IPathObject) currentTilemap;
		if (pathObject.Path == null) {
			pathObject.Path = new Path();
			pathObject.Path.Nodes.Add(new Path.Node());
		}
		application.SetEditor(new PathEditor(application, pathObject.Path));
	}

	private void OnDeletePath(object o, EventArgs args)
	{
		IPathObject pathObject = (IPathObject) currentTilemap;
		if (pathObject.Path != null) {
			pathObject.Path = null;
			//application.SetEditor(null);
		}
	}

	private void OnDelete(object o, EventArgs args)
	{
		if(currentTilemap == null)
			return;

		// Don't remove last tilemap, that cause bugs.
		if (sector.GetObjects(typeof(Tilemap)).Count == 1)
			return;

		if(String.IsNullOrEmpty(currentTilemap.Name)){
			application.TakeUndoSnapshot("Delete Tilemap (" + currentTilemap.ZPos + ")");
		} else {
			application.TakeUndoSnapshot("Delete Tilemap " + currentTilemap.Name + " (" + currentTilemap.ZPos + ")");
		}
		sector.Remove(currentTilemap);
		currentTilemap = null;
		UpdateList();
	}

	private void OnCheckIDs(object o, EventArgs args) {
		if (currentTilemap == null)
			return;

		List<int> invalidtiles = QACheck.CheckIds(currentTilemap.Tiles, application.CurrentLevel.Tileset);
		MessageType msgtype;
		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		if (invalidtiles.Count == 0) {
			msgtype = MessageType.Info;
			sb.Append("All tile ids are valid");
		} else {
			msgtype = MessageType.Warning;
			sb.Append("This tilemap contains tiles with these non existant IDs:");
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
		if(currentTilemap != null) {
			float vis = visibility[currentTilemap];
			float newvis = 1.0f;
			if(vis == 1.0f) {
				newvis = 0.5f;
			} else if(vis == 0.5f) {
				newvis = 0.0f;
			} else {
				newvis = 1.0f;
			}

			application.CurrentRenderer.SetTilemapColor(currentTilemap,
			                                            new Color(1, 1, 1, newvis));
			visibility[currentTilemap] = newvis;
			QueueDraw();
		} else {
			float vis = visibility[nullObject];
			float newvis = 1.0f;
			if(vis == 1.0f) {
				newvis = 0.5f;
			} else if(vis == 0.5f) {
				newvis = 0.0f;
			} else {
				newvis = 1.0f;
			}

			application.CurrentRenderer.SetObjectsColor(new Color(1, 1, 1, newvis));
			visibility[nullObject] = newvis;
			QueueDraw();
		}
	}
}
