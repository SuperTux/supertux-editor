using System;
using Gtk;

public class GameObjectListWidget : TreeView
{
	private IGameObject currentObject;
	
	public GameObjectListWidget(IEditorApplication application)
	{
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
	
	private void OnSectorChanged(Level level, Sector newSector)
	{
		TreeStore store = new TreeStore(typeof(System.Object));			
		foreach(IGameObject Object in newSector.GameObjects) {
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
    	
    	if(args.Event.Button == 3) {
    		ShowPopupMenu();
    	}
    }

	private void ShowPopupMenu()
	{
		Menu popupMenu = new Menu();

		ImageMenuItem propertiesItem = new ImageMenuItem(Stock.Properties, null);
		propertiesItem.Activated += OnProperties;
		popupMenu.Add(propertiesItem);
		
		popupMenu.ShowAll();
		popupMenu.Popup();
	}
	
	private void OnProperties(object o, EventArgs args)
	{
		new SettingsWindow(currentObject.GetType().Name, currentObject);
	}
}
