//  $Id$
using System;
using System.Collections.Generic;
using Gtk;
using Undo;
using LispReader;

public class SectorSwitchNotebook : Notebook
{
	private Level level;
	private Sector sector;
	private IEditorApplication application;
	private Dictionary<object, Widget> widgets = new Dictionary<object, Widget>();	//keep widgets in dictionary for easy updates

	public delegate void SectorChangedEventHandler(Sector newSector);
	public event SectorChangedEventHandler SectorChanged;

	public SectorRenderer CurrentRenderer {
		get {
			try {
				ScrollBarRenderView scrollview = (ScrollBarRenderView) CurrentPageWidget;
				if(scrollview == null)
					return null;

				return (SectorRenderer) scrollview.Renderer;
			} catch(Exception e) {
				Console.WriteLine("Except: " + e.Message);
				return null;
			}

		}
	}

	public SectorSwitchNotebook(IEditorApplication application)
	{
		this.application = application;
		SwitchPage += OnSwitchPage;
		ButtonPressEvent += OnButtonPress;
		application.LevelChanged += OnLevelChanged;
		application.SectorChanged += OnSectorChanged;

		FieldOrProperty.Lookup(typeof(Sector).GetField("Name")).Changed += OnSectorRenamed;
	}

	private void OnLevelChanged(Level newLevel)
	{
		ClearTabList();
		this.level = newLevel;
		CreateTabList();
	}

	private void OnSectorChanged(Level level, Sector newSector)
	{
		int num = level.Sectors.IndexOf(newSector);
		if(num < 0)
			return;
		this.sector = newSector;

		if(num != CurrentPage) {
			CurrentPage = num;
		}
	}

	/// <summary> Called when Name changes on any sector. </summary>
	private void OnSectorRenamed(object Object, FieldOrProperty field, object oldValue)
	{
		if (widgets.ContainsKey(Object))
			SetTabLabelText (widgets[Object], (string) field.GetValue(Object));
	}

	private void ClearTabList()
	{
		while (NPages > 0) {
			ScrollBarRenderView scrlview = (ScrollBarRenderView) GetNthPage(-1);
			IDisposable disposable = (IDisposable) scrlview.Renderer;
			disposable.Dispose();	//Let the render unregister its event handlers
			RemovePage(-1);	//Remove last page
		}
		widgets.Clear();
	}

	private void CreateTabList()
	{
		foreach(Sector sector in level.Sectors) {
			OnSectorAdd(sector);
		}

		if(this.sector == null && level.Sectors.Count > 0)
			this.sector = level.Sectors[0];
	}

	private void OnSwitchPage(object o, SwitchPageArgs args)
	{
		Sector NewSector = level.Sectors[(int) args.PageNum];
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

		foreach(Sector sector in level.Sectors) {
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

		MenuItem CheckIDsItem = new MenuItem("Check all tilemaps for bad tile IDs");
		CheckIDsItem.Activated += OnCheckIDs;
		popupMenu.Append(CheckIDsItem);

		popupMenu.ShowAll();
		popupMenu.Popup();
	}

	private void OnSectorItemActivated(object o, EventArgs args)
	{
		MenuItem item = (MenuItem) o;
		foreach(Sector sector in level.Sectors) {
			if(sector.Name == item.Name) {
				CurrentPage = (PageNum(widgets[sector]));	//switch to selected page
				SectorChanged(sector);
				return;
			}
		}

		LogManager.Log(LogLevel.Error, "Sector '" + item.Name + "' not found?!?");
	}

	private void OnPropertiesActivated(object o, EventArgs args)
	{
		application.EditProperties(sector, "Sector");
	}

	private void OnDeleteActivated(object o, EventArgs args)
	{
		// Don't remove sector if it is the only one.
		if (level.Sectors.Count == 1){
			application.PrintStatus("A level has to have at least one sector.");
			return;
		}

		application.PrintStatus("Sector '"+ sector.Name + "' removed.");
		SectorRemoveCommand command = new SectorRemoveCommand(
			"Removed sector",
			sector,
			level);
		command.OnSectorAdd += OnSectorAdd;
		command.OnSectorRemove += OnSectorRemove;
		command.Do();
		UndoManager.AddCommand(command);
	}

	private void OnResizeActivated(object o, EventArgs args)
	{
		try {
			new ResizeDialog(sector);
		} catch(Exception e) {
			ErrorDialog.Exception(e);
		}
	}

	protected void OnCheckIDs(object o, EventArgs args) {
		QACheck.CheckIds(application, sector, true);
	}

	/// <summary> Used from Sector Add/Remove commands to notify us about change. </summary>
	private void OnSectorAdd(Sector sector) {
		SectorRenderer Renderer = new SectorRenderer(application, level, sector);
		ScrollBarRenderView scrollbarview = new ScrollBarRenderView(Renderer);
		scrollbarview.ShowAll();
		AppendPage(scrollbarview, new Label(sector.Name));
		widgets.Add(sector, scrollbarview);

	}

	/// <summary> Used from Sector Add/Remove commands to notify us about change. </summary>
	private void OnSectorRemove(Sector sector) {
		if (widgets.ContainsKey(sector)) {
			ScrollBarRenderView scrollbarview = (ScrollBarRenderView) widgets[sector];
			RemovePage(PageNum(scrollbarview));
			scrollbarview.Renderer.Dispose();
			widgets.Remove(sector);
		} else {
			ErrorDialog.ShowError("Removed sector \"" + sector.Name + "\" was not found in the level");
		}
	}

	private void OnCreateNew(object o, EventArgs args)
	{
		try {
			Sector sector = LevelUtil.CreateSector("NewSector");
			SectorAddCommand command = new SectorAddCommand(
				"Added sector",
				sector,
				level);
			command.OnSectorAdd += OnSectorAdd;
			command.OnSectorRemove += OnSectorRemove;
			command.Do();
			UndoManager.AddCommand(command);
			OnSectorChanged(level, sector);
			OnPropertiesActivated(null, null);
		} catch(Exception e) {
			ErrorDialog.Exception("Couldn't create new sector", e);
		}
	}
}
