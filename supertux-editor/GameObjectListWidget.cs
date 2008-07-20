//  $Id$
using System;
using Gtk;

public class GameObjectListWidget : IconView
{
	const int COL_NAME = 0;
	const int COL_OBJECT = 1;

	private IGameObject currentObject;
	private IEditorApplication application;
	private Sector sector;

	public GameObjectListWidget(IEditorApplication application)
	{
		this.application = application;
		ButtonPressEvent += OnButtonPressed;

		TextColumn = COL_NAME;

		application.SectorChanged += OnSectorChanged;
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
		if((Object is IObject) || (Object is Tilemap))
			return;

		UpdateList();
	}

	private void UpdateList()
	{
		ListStore store = new ListStore(typeof(string), typeof(System.Object));
		foreach(IGameObject Object in sector.GetObjects()) {
			if(! (Object is IObject) && !(Object is Tilemap))
				store.AppendValues(Object.GetType().Name, Object);
		}
		Model = store;
	}

	[GLib.ConnectBefore]
		private void OnButtonPressed(object o, ButtonPressEventArgs args)
		{
			TreePath path = GetPathAtPos((int) args.Event.X, (int) args.Event.Y);
			if (path == null) return;

			TreeIter iter;
			if(!Model.GetIter(out iter, path))
				return;

			currentObject = (IGameObject) Model.GetValue(iter, COL_OBJECT);
			application.EditProperties(currentObject, currentObject.GetType().Name);

			if(args.Event.Button == 3) {
				ShowPopupMenu();
			}
		}

	private void ShowPopupMenu()
	{
		Menu popupMenu = new Menu();

		MenuItem editPathItem = new MenuItem("Edit Path");
		editPathItem.Activated += OnEditPath;
		editPathItem.Sensitive = currentObject is IPathObject;
		popupMenu.Append(editPathItem);

		MenuItem deletePathItem = new MenuItem("Delete Path");
		deletePathItem.Activated += OnDeletePath;
		deletePathItem.Sensitive = currentObject is IPathObject;
		popupMenu.Append(deletePathItem);

		MenuItem deleteItem = new ImageMenuItem(Stock.Delete, null);
		deleteItem.Activated += OnDelete;
		popupMenu.Append(deleteItem);

		popupMenu.ShowAll();
		popupMenu.Popup();
	}

	private void OnDelete(object o, EventArgs args)
	{
		if(currentObject == null)
			return;

		sector.Remove(currentObject);
		UpdateList();
	}

	private void OnEditPath(object o, EventArgs args)
	{
		if(! (currentObject is IPathObject))
			return;
		IPathObject pathObject = (IPathObject)currentObject;
		if (pathObject.Path == null) {
			pathObject.Path = new Path();
			pathObject.Path.Nodes.Add(new Path.Node());
		}
		application.SetEditor(new PathEditor(application, pathObject.Path));
	}

	private void OnDeletePath(object o, EventArgs args) {
		if (!(currentObject is IPathObject))
			return;
		IPathObject pathObject = (IPathObject)currentObject;
		if (pathObject.Path != null) {
			pathObject.Path = null;
			//application.SetEditor(null);
		}
	}

}
