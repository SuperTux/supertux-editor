//  $Id$
using System;
using System.Collections.Generic;
using Gtk;
using Undo;

public class SectorSwitchNotebook : Notebook
{
	private Level level;
	private Sector sector;
	private IEditorApplication application;

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

	private void ClearTabList()
	{
		foreach(Widget widget in this) {
			Remove(widget);
		}
	}

	private void CreateTabList()
	{
		foreach(Sector sector in level.Sectors) {
			SectorRenderer Renderer = new SectorRenderer(application, level, sector);
			ScrollBarRenderView scrollbarview = new ScrollBarRenderView(Renderer);
			scrollbarview.ShowAll();
			AppendPage(scrollbarview, new Label(sector.Name));
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
		//HACK: Don't remove first sector if we got two sector.
		//      It will cause weird bugs elsewhere.
		if ((level.Sectors.IndexOf(sector) == 0) && (level.Sectors.Count == 2)){
			application.PrintStatus("Bug: Removing first sector does not work if there are exactly two sectors.");
			return;
		}

		application.PrintStatus("Sector '"+ sector.Name + "' removed.");
		SectorRemoveCommand command = new SectorRemoveCommand(
			"Removed sector",
			sector,
			level);
		command.OnSectorAddRemove += OnSectorUpdate;
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

	/// <summary>
	/// Used from sector commands that add/remove sectors to force an update of our
	/// tablist and any other stuff we have to do.
	/// </summary>
	private void OnSectorUpdate() {
		ClearTabList();
		CreateTabList();
	}

	private void OnCreateNew(object o, EventArgs args)
	{
		try {
			Sector sector = LevelUtil.CreateSector("NewSector");
			SectorAddCommand command = new SectorAddCommand(
				"Added sector",
				sector,
				level);
			command.OnSectorAddRemove += OnSectorUpdate;
			command.Do();
			UndoManager.AddCommand(command);
			OnSectorChanged(level, sector);
		} catch(Exception e) {
			ErrorDialog.Exception("Couldn't create new sector", e);
		}
	}
}
