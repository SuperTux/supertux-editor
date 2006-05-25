using System;
using Gtk;

public class GameObjectListWidget : TreeView
{
	private IGameObject currentObject;
	private IEditorApplication application;
	private Sector sector;
	
	public GameObjectListWidget(IEditorApplication application)
	{
		this.application = application;
		ButtonPressEvent += OnButtonPressed;
		
		CellRendererText TextRenderer = new CellRendererText();
		TreeViewColumn TypeColumn = new TreeViewColumn();
		TypeColumn.PackStart(TextRenderer, true);
		TypeColumn.SetCellDataFunc(TextRenderer, TextDataFunc);
		TypeColumn.Title = "Type";
		AppendColumn(TypeColumn);
	
		HeadersVisible = false;
		
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
		TreeStore store = new TreeStore(typeof(System.Object));			
		foreach(IGameObject Object in sector.GetObjects()) {
			if(! (Object is IObject) && !(Object is Tilemap))
				store.AppendValues(Object);
		}
		Model = store;	
	}	
	
	private void TextDataFunc(TreeViewColumn Column, CellRenderer Renderer,
			             	  TreeModel Model, TreeIter Iter)
	{
		object o = Model.GetValue(Iter, 0);
	
		CellRendererText TextRenderer = (CellRendererText) Renderer;
		TextRenderer.Text = o.GetType().Name;
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
    	
    	currentObject = (IGameObject) Model.GetValue(iter, 0);
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
		
		IPathObject pathObject = (IPathObject) currentObject;
		application.SetEditor(new PathEditor(application, pathObject.Path));
	}
		
}
