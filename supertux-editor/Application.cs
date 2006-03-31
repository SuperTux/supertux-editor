using System;
using System.IO;
using System.Diagnostics;
using Gtk;
using Glade;
using Gdl;
using Sdl;
using Drawing;

public class Application : IEditorApplication {
	[Glade.Widget]
	private Gtk.Window MainWindow;

	private TileListWidget TileList;
	private LayerListWidget LayerList;
	private SectorSwitchNotebook SectorSwitchNotebook;
	private Selection Selection;

	private FileChooserDialog FileChooser;
	private Dock dock;
	private DockLayout layout;

	private Level Level;
	private Sector Sector;
	private	LispSerializer Serializer = new LispSerializer(typeof(Level));
	private string FileName;
	private string LayoutFile;

	public event LevelChangedEventHandler LevelChanged;
	public event SectorChangedEventHandler SectorChanged;
	public event TilemapChangedEventHandler TilemapChanged;
	
	public SectorRenderer CurrentRenderer {
		get {
			return SectorSwitchNotebook.CurrentRenderer;
		}
	}

	private Application(string[] args) {
		LayoutFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		LayoutFile += "/" + Constants.PACKAGE_NAME + "/layout.xml";
		
		Glade.XML.CustomHandler = GladeCustomWidgetHandler;
		Glade.XML gxml = new Glade.XML("editor.glade", null);
		gxml.Autoconnect(this);
	
		if(MainWindow == null)
			throw new Exception("Couldn't resolve all widgets");

		Tileset.LoadEditorImages = true;

		Selection = new Selection();
		
		SetupDock();
		
		MainWindow.DeleteEvent += OnDelete;

		MainWindow.SetSizeRequest(900, 675);
		MainWindow.ShowAll();
		
		FileChooser = new FileChooserDialog("Choose a Level", MainWindow, FileChooserAction.Open, new object[] {});
		if(Settings.Instance.LastDirectoryName != null)
			FileChooser.SetCurrentFolder(Settings.Instance.LastDirectoryName);
		FileChooser.AddButton(Gtk.Stock.Cancel, Gtk.ResponseType.Cancel);
		FileChooser.AddButton(Gtk.Stock.Open, Gtk.ResponseType.Ok);
		FileChooser.DefaultResponse = Gtk.ResponseType.Ok;

		if(args.Length > 0) {
			Load(args[0]);
		}
	}
	
	private void SetupDock()
	{
		SectorSwitchNotebook = new SectorSwitchNotebook(this);
		SectorSwitchNotebook.SectorChanged += ChangeCurrentSector;
		SectorSwitchNotebook.ShowAll();
		DockItem mainDock = new DockItem("MainDock", "MainDock",
		                                 DockItemBehavior.NoGrip);
		mainDock.Add(SectorSwitchNotebook);
		dock.AddItem(mainDock, DockPlacement.Center);
				
		TileList = new TileListWidget(this, Selection);
		DockItem tileListDock = new DockItem("TileList", "Tiles",
											 DockItemBehavior.NeverFloating);
		tileListDock.Add(TileList);
		tileListDock.DockTo(mainDock, DockPlacement.Left);
		
		ScrolledWindow scrolledWindow = new ScrolledWindow();
		LayerList = new LayerListWidget(this);
		scrolledWindow.Add(LayerList);
		scrolledWindow.VscrollbarPolicy = PolicyType.Never;
		DockItem layerListDock = new DockItem("LayerList", "Layers",
		                                      DockItemBehavior.NeverFloating);
		layerListDock.Add(scrolledWindow);
		layerListDock.DockTo(mainDock, DockPlacement.Bottom);		
		
		ObjectListWidget ObjectList = new ObjectListWidget();
		DockItem objectListDock = new DockItem("ObjectList", "Objects",
		                                       DockItemBehavior.NeverFloating);
		objectListDock.Add(ObjectList);
		objectListDock.DockTo(tileListDock, DockPlacement.Center);
		
		scrolledWindow = new ScrolledWindow();
		GameObjectListWidget objectList = new GameObjectListWidget(this);
		scrolledWindow.Add(objectList);
		scrolledWindow.VscrollbarPolicy = PolicyType.Never;
		DockItem gObjectListDock = new DockItem("GObjectList", "GObjects",
		                                        Gtk.Stock.Info,
		                                        DockItemBehavior.NeverFloating);
		gObjectListDock.Add(scrolledWindow);
		gObjectListDock.DockTo(layerListDock, DockPlacement.Center);
		
		layout.LoadFromFile(LayoutFile);
		layout.LoadLayout("__default__");
	}
	
	protected Widget GladeCustomWidgetHandler(Glade.XML xml, string func_name, string name, string string1, string string2, int int1, int int2)
	{
		if(func_name == "CreateDockWidget") {
			dock = new Dock ();		
			layout = new DockLayout (dock);
			DockBar dockbar = new DockBar (dock);
		
			Box box = new HBox (false, 5);
			box.PackStart (dockbar, false, false, 0);
			box.PackEnd (dock, true, true, 0);
			
			box.ShowAll();
			return box;
		}
		
		return null;
	}

	private static void InitSdl()
	{
		if(SDL.Init(SDL.INIT_EVERYTHING | SDL.INIT_NOPARACHUTE) < 0) {
	 		throw new Exception("Couldn't initialize SDL: " + SDL.GetError());
		}
	}

	protected void OnNew(object o, EventArgs args)
	{
		try {
			Level level = LevelUtil.CreateLevel();
			ChangeCurrentLevel(level);
		} catch(Exception e) {
			ErrorDialog.Exception("Couldn't create new level", e);
		}
	}

	protected void OnOpen(object o, EventArgs e)
	{
		FileChooser.SetCurrentFolder(Settings.Instance.LastDirectoryName);
		FileChooser.Action = FileChooserAction.Open;
		int result = FileChooser.Run();
		FileChooser.Hide();
		if(result != (int) ResponseType.Ok)
			return;
	
		Settings.Instance.LastDirectoryName = FileChooser.CurrentFolder;
		Settings.Instance.Save();
		Load(FileChooser.Filename);
	}

	private void Load(string Filename)
	{
		try {
			Level NewLevel = (Level) Serializer.Read(Filename);
			if(NewLevel.Version < 2)
				throw new Exception("Old Level Format not supported");
			ChangeCurrentLevel(NewLevel);
			this.FileName = Filename;
		} catch(Exception e) {
			ErrorDialog.Exception("Error loading level", e);
		}
	}

	protected void OnSave(object o, EventArgs e)
	{
		Save(false);
	}

	protected void OnSaveAs(object o, EventArgs e)
	{
		Save(true);
	}

	protected void Save(bool ChooseName)
	{
		if(FileName == null)
			ChooseName = true;

		if(ChooseName) {
			FileChooser.SetCurrentFolder(Settings.Instance.LastDirectoryName);
			FileChooser.Action = FileChooserAction.Save;
			int result = FileChooser.Run();
			FileChooser.Hide();
			if(result != (int) ResponseType.Ok)
				return;
			Settings.Instance.LastDirectoryName = FileChooser.CurrentFolder;
			Settings.Instance.Save();
			FileName = FileChooser.Filename;
		}
		
		try {
			Serializer.Write(FileName, Level);
		} catch(Exception e) {
			ErrorDialog.Exception("Couldn't save level", e);
		}
	}

	protected void OnQuit(object o, EventArgs e)
	{
		Close();
	}

	protected void OnAbout(object o, EventArgs e)
	{
	}
	
	protected void OnPlay(object o, EventArgs args)
	{
		if(Level == null)
			return;
		
		try {
			string TempName = System.IO.Path.GetTempPath() + "/supertux-editor.tmp.stl";
			Serializer.Write(TempName, Level);
			Process.Start(Settings.Instance.SupertuxExe, TempName);
		} catch(Exception e) {
			ErrorDialog.Exception("Couldn't start supertux", e);
		}
	}
	
	protected void OnLevelProperties(object o, EventArgs args)
	{
		if(Level == null)
			return;
		
		new SettingsWindow("Level Properties", Level);	
	}
	
	private void OnDelete(object o, DeleteEventArgs args)
	{
		// TODO ask for save?
		Close();
		args.RetVal = true;
	}
	
	private void Close()
	{
		Settings.Instance.Save();
		layout.SaveToFile(LayoutFile);
		MainWindow.Destroy();
		Gtk.Application.Quit();
	}

	public void ChangeCurrentLevel(Level NewLevel)
	{
		Level = NewLevel;
		LevelChanged(Level);
		ChangeCurrentSector(Level.Sectors[0]);
	}

	public void ChangeCurrentSector(Sector NewSector)
	{
		this.Sector = NewSector;
		SectorChanged(Level, NewSector);
	}

	public void ChangeCurrentTilemap(Tilemap Tilemap)
	{
		TilemapChanged(Tilemap);
		TilemapEditor Editor = new TilemapEditor(Tilemap, Level.Tileset,
		                                         Selection);
		SectorSwitchNotebook.CurrentRenderer.Editor = Editor;
	}

	public void SetObjectsEditMode()
	{
		ObjectsEditor Editor = new ObjectsEditor(Sector);
		SectorSwitchNotebook.CurrentRenderer.Editor = Editor;
	}

	public static void Main(string[] args)
	{
		InitSdl();
		
		Gtk.Application.Init();

		Application app = new Application(args);
		try {
			Gtk.Application.Run();
		} catch(Exception e) {
			if(app.Level != null) {
				Console.Error.WriteLine("Unxpected Exception... Emergency save to '/tmp/supertux-editor-emergency.stl'");
				app.Serializer.Write(System.IO.Path.GetTempPath() + "/supertux-editor-emergency.stl", app.Level);
			}
			throw e;
		}
		
		Settings.Instance.Save();

		SDL.Quit();
	}
}
