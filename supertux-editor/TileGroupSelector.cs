using System;
using Gtk;

public class TilegroupSelector : ComboBox
{
	private Level level;
	private TileListWidget tileList;
	
	public TilegroupSelector(IEditorApplication application, TileListWidget tileList)
	{
		this.tileList = tileList;
		application.LevelChanged += OnLevelChanged;
		
		CellRendererText renderer = new CellRendererText(); 
		PackStart(renderer, false);
		SetCellDataFunc(renderer, TextDataFunc);		            
		
		Changed += OnTileGroupChoosen;
	}
	
	private void OnLevelChanged(Level level)
	{
		if(this.level != null)
			this.level.TilesetChanged -= OnTilesetChanged;
		if(level != null)
			level.TilesetChanged += OnTilesetChanged;
		this.level = level;
		
		OnTilesetChanged(level);
	}
	
	private void OnTilesetChanged(Level level)
	{
		Tileset tileset = level.Tileset;
		
		TreeStore store = new TreeStore(typeof(Tilegroup));
		foreach(Tilegroup group in tileset.Tilegroups.Values) {
			store.AppendValues(group);
		}
		Model = store;
	}
	
	private void TextDataFunc(CellLayout cell_layout, CellRenderer renderer, TreeModel model, TreeIter iter)
	{
		CellRendererText textRenderer = (CellRendererText) renderer;
		Tilegroup group = (Tilegroup) Model.GetValue(iter, 0);
		
		textRenderer.Text = group.Name;
	}	
	
	private void OnTileGroupChoosen(object o, EventArgs args)
	{
		TreeIter iter;

    	if (!GetActiveIter (out iter))
    		return;
		
		Tilegroup group = (Tilegroup) Model.GetValue(iter, 0);
		tileList.ChangeTilegroup(group);
	}
}
