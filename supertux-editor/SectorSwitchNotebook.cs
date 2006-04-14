using System;
using Gtk;

public class SectorSwitchNotebook : Notebook
{
	private Level Level;
	private Sector Sector;

	public delegate void SectorChangedEventHandler(Sector NewSector);
	public event SectorChangedEventHandler SectorChanged;

	public SectorRenderer CurrentRenderer {
		get {
			return (SectorRenderer) CurrentPageWidget;
		}
	}

	public SectorSwitchNotebook(IEditorApplication Application)
	{
		SwitchPage += OnSwitchPage;
		ButtonPressEvent += OnButtonPress;
		Application.LevelChanged += OnLevelChanged;
		Application.SectorChanged += OnSectorChanged;
	}

	private void OnLevelChanged(Level NewLevel)
	{
		ClearTabList();
		this.Level = NewLevel;
		CreateTabList();
	}

	private void OnSectorChanged(Level Level, Sector NewSector)
	{
		int num = Level.Sectors.IndexOf(NewSector);
		if(num < 0)
			return;
		Sector = NewSector;		
		
		if(num != CurrentPage) {
			CurrentPage = num;
		}
	}

	private void ClearTabList()
	{
		foreach(Widget Widget in this) {
			Remove(Widget);
		}
	}

	private void CreateTabList()
	{
		foreach(Sector Sector in Level.Sectors) {
			SectorRenderer Renderer = new SectorRenderer(Level, Sector);
			Renderer.ShowAll();
			AppendPage(Renderer, new Label(Sector.Name));
		}
		
		if(this.Sector == null && Level.Sectors.Count > 0)
			this.Sector = Level.Sectors[0];
	}

	private void OnSwitchPage(object o, SwitchPageArgs args)
	{
		Sector NewSector = Level.Sectors[(int) args.PageNum];
		SectorChanged(NewSector);
	}
	
	private void OnButtonPress(object o, ButtonPressEventArgs args)
	{
		if(args.Event.Button == 3) {
			popupMenu();	
		}
	}
	
	private void popupMenu()
	{
		Menu popupMenu = new Menu();
		
		foreach(Sector sector in Level.Sectors) {
			MenuItem item = new MenuItem(sector.Name);
			item.Name = sector.Name;
			item.Activated += OnSectorItemActivated;
			popupMenu.Add(item);
		}
		popupMenu.Add(new SeparatorMenuItem());
		
		MenuItem propertiesItem = new ImageMenuItem(Stock.Properties, null);
		propertiesItem.Activated += OnPropertiesActivated;
		popupMenu.Add(propertiesItem);
		
		MenuItem resizeItem = new MenuItem("Resize");
		resizeItem.Activated += OnResizeActivated;
		popupMenu.Add(resizeItem);
		
		MenuItem createNewItem = new ImageMenuItem(Stock.New, null);
		createNewItem.Activated += OnCreateNew;
		popupMenu.Add(createNewItem);
		
		MenuItem deleteItem = new ImageMenuItem(Stock.Delete, null);
		deleteItem.Activated += OnDeleteActivated;
		popupMenu.Add(deleteItem);
		
		popupMenu.ShowAll();
		popupMenu.Popup();
	}
	
	private void OnSectorItemActivated(object o, EventArgs args)
	{
		MenuItem item = (MenuItem) o;
		foreach(Sector sector in Level.Sectors) {
			if(sector.Name == item.Name) {
				SectorChanged(sector);
				CurrentRenderer.GrabFocus();
				return;
			}
		}
		
		Console.WriteLine("Sector '" + item.Name + "' not found?!?");
	}
	
	private void OnPropertiesActivated(object o, EventArgs args)
	{
		new SettingsWindow("Sector Properties", Sector);
		ClearTabList();
		CreateTabList();
		OnSectorChanged(Level, Sector);
	}
	
	private void OnDeleteActivated(object o, EventArgs args)
	{
		Level.Sectors.Remove(Sector);
		ClearTabList();
		CreateTabList();
	}
	
	private void OnResizeActivated(object o, EventArgs args)
	{
		try {
			new ResizeDialog(Sector);
		} catch(Exception e) {
			ErrorDialog.Exception(e);
		}
	}
	
	private void OnCreateNew(object o, EventArgs args)
	{
		try {
			Sector sector = LevelUtil.CreateSector("NewSector");
		
			Level.Sectors.Add(sector);
			ClearTabList();
			CreateTabList();
			OnSectorChanged(Level, sector);
		} catch(Exception e) {
			ErrorDialog.Exception("Couldn't create new sector", e);
		}
	}
}

