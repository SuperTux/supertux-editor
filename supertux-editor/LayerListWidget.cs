using System;
using System.Collections.Generic;
using Gtk;
using GLib;
using Drawing;

public class LayerListWidget : TreeView {
	private IEditorApplication Application;
	private static object NullObject = new System.Object();
	private Tilemap currentTilemap;
	private Dictionary<object, float> visibility = new Dictionary<object, float>();
	
	private class VisibilityRenderer : CellRendererPixbuf
	{
		public VisibilityRenderer()
		{
			Mode = CellRendererMode.Editable;
		}
				
		public override CellEditable StartEditing(Gdk.Event evnt, Widget widget,
		                                            string path,
		                                            Gdk.Rectangle background_area,
		                                            Gdk.Rectangle cell_area,
		                                            CellRendererState flags) {
			if(VisibilityChanged != null)
				VisibilityChanged(this, null);                                            		
			return base.StartEditing(evnt, widget, path, background_area, cell_area, flags);
		}
		                                            
		public delegate void VisibilityChangedHandler(object o, EventArgs args);
		
		public event VisibilityChangedHandler VisibilityChanged;
	}
	
	public LayerListWidget(IEditorApplication Application)
	{
		this.Application = Application;
		ButtonPressEvent += OnButtonPressed;
		
		VisibilityRenderer visibilityRenderer = new VisibilityRenderer();
		visibilityRenderer.VisibilityChanged += OnVisibilityChange;
		TreeViewColumn visibilityColumn = new TreeViewColumn("Visibility",
		                                                       visibilityRenderer);
		visibilityColumn.SetCellDataFunc(visibilityRenderer, VisibilityDataFunc);
		AppendColumn(visibilityColumn);
		
		CellRendererText TextRenderer = new CellRendererText();
		TreeViewColumn TypeColumn = new TreeViewColumn();
		TypeColumn.PackStart(TextRenderer, true);
		TypeColumn.SetCellDataFunc(TextRenderer, TextDataFunc);
		TypeColumn.Title = "Type";
		AppendColumn(TypeColumn);
		
		HeadersVisible = false;

		Application.SectorChanged += OnSectorChanged;
		Application.TilemapChanged += OnTilemapChanged;
	}

	private void OnSectorChanged(Level Level, Sector Sector)
	{
		visibility.Clear();
		TreeStore store = new TreeStore(typeof(System.Object));
		foreach(Tilemap Tilemap in Sector.GetObjects(typeof(Tilemap))) {
			store.AppendValues(Tilemap);
			visibility[Tilemap] = 1.0f;
		}
		store.AppendValues(NullObject);
		visibility[NullObject] = 1.0f;
		Model = store;
	}

	private void OnTilemapChanged(Tilemap Tilemap)
	{
		Console.WriteLine("Not implemented");
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
			TextRenderer.Text = Tilemap.LayerName;
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
    			Application.ChangeCurrentTilemap(currentTilemap);
    		}
    	} else {
    		Application.SetObjectsEditMode();
    		currentTilemap = null;
    	}
    	
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
		if(currentTilemap == null)
			return;
		
		new SettingsWindow("Tilemap Properties", currentTilemap);
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
			
			Application.CurrentRenderer.SetTilemapColor(currentTilemap,
		                                                new Color(1, 1, 1, newvis));
			visibility[currentTilemap] = newvis;
			QueueDraw();
		} else {
			float vis = visibility[NullObject];
			float newvis = 1.0f;
			if(vis == 1.0f) {
				newvis = 0.5f;
			} else if(vis == 0.5f) {
				newvis = 0.0f;
			} else {
				newvis = 1.0f;
			}
			
			Application.CurrentRenderer.SetObjectsColor(new Color(1, 1, 1, newvis));
			visibility[NullObject] = newvis;
			QueueDraw();
		}
	}
}

