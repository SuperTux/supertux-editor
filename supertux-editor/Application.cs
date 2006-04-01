using System;
using System.IO;
using System.Diagnostics;
using Gtk;
using Glade;
using Gdl;
using Sdl;
using Drawing;
using LispReader;

public class Application : IEditorApplication {
	[Glade.Widget]
	private Gtk.Window MainWindow;

	private TileListWidget tileList;
	private LayerListWidget layerList;
	private SectorSwitchNotebook sectorSwitchNotebook;
	private Selection selection;

	private FileChooserDialog FileChooser;
	private Dock dock;
	private DockLayout layout;

	private Level level;
	private Sector sector;
	private	LispSerializer serializer = new LispSerializer(typeof(Level));
	private string fileName;
	private string layoutFile;

	public event LevelChangedEventHandler LevelChanged;
	public event SectorChangedEventHandler SectorChanged;
	public event TilemapChangedEventHandler TilemapChanged;
	
	public SectorRenderer CurrentRenderer {
		get {
			return sectorSwitchNotebook.CurrentRenderer;
		}
	}

	private Application(string[] args) {
		layoutFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		layoutFile += "/" + Constants.PACKAGE_NAME + "/layout.xml";
		
		Glade.XML.CustomHandler = GladeCustomWidgetHandler;
		Glade.XML gxml = new Glade.XML("editor.glade", "MainWindow");
		gxml.Autoconnect(this);
	
		if(MainWindow == null)
			throw new Exception("Couldn't resolve all widgets");

		Tileset.LoadEditorImages = true;

		selection = new Selection();
		
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
		sectorSwitchNotebook = new SectorSwitchNotebook(this);
		sectorSwitchNotebook.SectorChanged += ChangeCurrentSector;
		sectorSwitchNotebook.ShowAll();
		DockItem mainDock = new DockItem("MainDock", "MainDock",
		                                 DockItemBehavior.NoGrip);
		mainDock.Add(sectorSwitchNotebook);
		dock.AddItem(mainDock, DockPlacement.Center);
				
		Widget tileListWidget = CreateTileList();
		DockItem tileListDock = new DockItem("TileList", "Tiles",
											 DockItemBehavior.NeverFloating);
		tileListDock.Add(tileListWidget);
		tileListDock.DockTo(mainDock, DockPlacement.Left);
		
		ScrolledWindow scrolledWindow = new ScrolledWindow();
		layerList = new LayerListWidget(this);
		scrolledWindow.Add(layerList);
		scrolledWindow.VscrollbarPolicy = PolicyType.Never;
		DockItem layerListDock = new DockItem("LayerList", "Layers",
		                                      DockItemBehavior.NeverFloating);
		layerListDock.Add(scrolledWindow);
		layerListDock.DockTo(mainDock, DockPlacement.Bottom);		
		
		ObjectListWidget objectList = new ObjectListWidget();
		DockItem objectListDock = new DockItem("ObjectList", "Objects",
		                                       DockItemBehavior.NeverFloating);
		objectListDock.Add(objectList);
		objectListDock.DockTo(tileListDock, DockPlacement.Center);
		
		scrolledWindow = new ScrolledWindow();
		GameObjectListWidget gObjectList = new GameObjectListWidget(this);
		scrolledWindow.Add(gObjectList);
		scrolledWindow.VscrollbarPolicy = PolicyType.Never;
		DockItem gObjectListDock = new DockItem("GObjectList", "GObjects",
		                                        Gtk.Stock.Info,
		                                        DockItemBehavior.NeverFloating);
		gObjectListDock.Add(scrolledWindow);
		gObjectListDock.DockTo(layerListDock, DockPlacement.Center);
		
		layout.LoadFromFile(layoutFile);
		layout.LoadLayout("__default__");
	}
	
	private Widget CreateTileList()
	{
		VBox box = new VBox();
		box.Homogeneous = false;
		
		ComboBoxEntry combo = new ComboBoxEntry(new string[] { "test", "1", "zwo" });
		combo.Changed += OnTileGroupChoosen;
		box.PackStart(combo, false, true, 0);
				
		tileList = new TileListWidget(this, selection);
		box.PackStart(tileList, true, true, 0);
		
		return box;
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
	
	private void OnTileGroupChoosen(object o, EventArgs args)
	{
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

	private void Load(string fileName)
	{
		try {
			Level newLevel = (Level) serializer.Read(fileName);
			if(newLevel.Version < 2)
				throw new Exception("Old Level Format not supported");
			ChangeCurrentLevel(newLevel);
			this.fileName = fileName;
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

	protected void Save(bool chooseName)
	{
		if(fileName == null)
			chooseName = true;

		if(chooseName) {
			FileChooser.SetCurrentFolder(Settings.Instance.LastDirectoryName);
			FileChooser.Action = FileChooserAction.Save;
			int result = FileChooser.Run();
			FileChooser.Hide();
			if(result != (int) ResponseType.Ok)
				return;
			Settings.Instance.LastDirectoryName = FileChooser.CurrentFolder;
			Settings.Instance.Save();
			fileName = FileChooser.Filename;
		}
		
		try {
			serializer.Write(fileName, level);
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
		if(level == null)
			return;
		
		try {
			string tempName = System.IO.Path.GetTempPath() + "/supertux-editor.tmp.stl";
			serializer.Write(tempName, level);
			Process.Start(Settings.Instance.SupertuxExe, tempName);
		} catch(Exception e) {
			ErrorDialog.Exception("Couldn't start supertux", e);
		}
	}
	
	protected void OnLevelProperties(object o, EventArgs args)
	{
		if(level == null)
			return;
		
		new SettingsWindow("Level Properties", level);	
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
		layout.SaveToFile(layoutFile);
		MainWindow.Destroy();
		Gtk.Application.Quit();
	}

	public void ChangeCurrentLevel(Level newLevel)
	{
		level = newLevel;
		LevelChanged(level);
		ChangeCurrentSector(level.Sectors[0]);
	}

	public void ChangeCurrentSector(Sector newSector)
	{
		this.sector = newSector;
		SectorChanged(level, newSector);
	}

	public void ChangeCurrentTilemap(Tilemap tilemap)
	{
		TilemapChanged(tilemap);
		TilemapEditor editor = new TilemapEditor(tilemap, level.Tileset,
		                                         selection);
		sectorSwitchNotebook.CurrentRenderer.Editor = editor;
	}

	public void SetObjectsEditMode()
	{
		ObjectsEditor editor = new ObjectsEditor(sector);
		sectorSwitchNotebook.CurrentRenderer.Editor = editor;
	}

	public static void Main(string[] args)
	{
		LispSerializer.SetupSerializers(typeof(Application).Assembly);
		InitSdl();
		
		Gtk.Application.Init();

		Application app = new Application(args);
		try {
			Gtk.Application.Run();
		} catch(Exception e) {
			if(app.level != null) {
				Console.Error.WriteLine("Unxpected Exception... Emergency save to '/tmp/supertux-editor-emergency.stl'");
				app.serializer.Write(System.IO.Path.GetTempPath() + "/supertux-editor-emergency.stl", app.level);
			}
			throw e;
		}
		
		Settings.Instance.Save();

		SDL.Quit();
	}
}
