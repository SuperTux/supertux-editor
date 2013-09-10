//  SuperTux Editor
//  Copyright (C) 2006 Matthias Braun <matze@braunis.de>
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.IO;
using System.Diagnostics;
using Glade;
using Drawing;
using LispReader;
using System.Collections.Generic;
using DataStructures;
using Undo;

public delegate void LevelChangedEventHandler(Level NewLevel);
public delegate void SectorChangedEventHandler(Level Level, Sector NewSector);
public delegate void TilemapChangedEventHandler(Tilemap Tilemap);

public class Application
{
	private class MruEntry {
		public Gtk.MenuItem MenuItem;
		public string FileName;
	}

	/// <summary>Original <see cref="MainWindow"/> title, read from .glade ressource</summary>
	private string MainWindowTitlePrefix;

	//allow incomming URIs (0 = no limitations, 0 = ID 0)
	public static Gtk.TargetEntry [] target_table = new Gtk.TargetEntry[] {
		new Gtk.TargetEntry("text/uri-list", 0, 0)
	};

	#region Glade
	[Glade.Widget]
	private Gtk.Window MainWindow = null;

	[Glade.Widget]
	private Gtk.Widget ToolSelectProps = null;

	private Gtk.Widget ToolTilesProps;
	private Gtk.Widget ToolObjectsProps;
	private Gtk.Widget ToolGObjectsProps;

	[Glade.Widget]
	private Gtk.Widget ToolBrushProps = null;

	[Glade.Widget] private Gtk.RadioToolButton ToolSelect  = null;
	[Glade.Widget] private Gtk.RadioToolButton ToolTiles   = null;
	[Glade.Widget] private Gtk.RadioToolButton ToolZoom    = null;
	[Glade.Widget] private Gtk.RadioToolButton ToolObjects = null;
	[Glade.Widget] private Gtk.RadioToolButton ToolBrush   = null;
	[Glade.Widget] private Gtk.RadioToolButton ToolFill    = null;
	[Glade.Widget] private Gtk.RadioToolButton ToolReplace = null;
	[Glade.Widget] private Gtk.RadioToolButton ToolPath    = null;

	[Glade.Widget]
	private Gtk.Statusbar sbMain = null;

	[Glade.Widget]
	private Gtk.MenuItem MenuItemUndo = null;

	[Glade.Widget]
	private Gtk.MenuItem MenuItemRedo = null;

	[Glade.Widget]
	private Gtk.MenuItem MenuItemToolPath = null;

	[Glade.Widget]
	private Gtk.ToolButton ToolButtonUndo = null;

	[Glade.Widget]
	private Gtk.ToolButton ToolButtonRedo = null;

	[Glade.Widget]
	private Gtk.ToolButton ToolButtonCamera = null;

	[Glade.Widget]
	private Gtk.Menu MenuItemLevel_menu = null; /* "Level" menu, contains MenuItemMruBegin */

	[Glade.Widget]
	private Gtk.MenuItem MenuItemMruBegin = null; /* RecentDocument list will start after this item */

	[Glade.Widget]
	private Gtk.Frame fGObjects = null;

	[Glade.Widget]
	private Gtk.Frame fToolsLeft = null;		//container for docking toolbox on left

	[Glade.Widget]
	private Gtk.Frame fToolsRight = null;		//container for docking toolbox on right

	[Glade.Widget]
	private Gtk.Alignment aTools = null;		//toolbox frame and it's wrappings

	#endregion Glade

	private TileListWidget tileList;
	private LayerListWidget layerList;
	private SectorSwitchNotebook sectorSwitchNotebook;
	private PropertiesView propertiesView;
	private TileSelection selection;
	private IPathObject iPathToEdit;

	private uint printStatusContextID;
	private uint printStatusMessageID;

	private Gtk.FileChooserDialog fileChooser;
	private List<MruEntry> MenuItemMruEntries = new List<MruEntry>(); /* list of MenuItem entries that constitute the RecentDocument list */

	private Tilemap tilemap;
	private Level level;
	private Sector sector;
	private LispSerializer serializer = new LispSerializer(typeof(Level));
	private string fileName;

	public event LevelChangedEventHandler LevelChanged;
	public event SectorChangedEventHandler SectorChanged;
	public event TilemapChangedEventHandler TilemapChanged;

	public SectorRenderer CurrentRenderer {
		get {
			return sectorSwitchNotebook.CurrentRenderer;
		}
	}

	public Sector CurrentSector {
		get {
			return sector;
		}
		set {
			//ignore when there is no change
			if (sector == value)
				return;
			sector = value;
			SectorChanged(level, value);
			if (CurrentRenderer != null) {
				// If there is no tool activated for this sector yet reset it to the Select tool.
				if (CurrentRenderer.Editor == null) {
					OnToolSelect(null, null);
					ToolSelect.Active = true;
				}
			}
		}
	}

	public Level CurrentLevel {
		get {
			return level;
		}
		set {
			//ignore when there is no change
			if (level == value)
				return;
			// Fix bug if loading worldmap while having a selection
			// in a "normal" level (or the other way round):
			if(level != null && !level.TilesetFile.Equals(value.TilesetFile)){
				selection.Resize(0, 0, 0);
				selection.FireChangedEvent();
			}
			level = value;
			LevelChanged(level);
			CurrentSector = level.Sectors[0];
			OnToolSelect(null, null);
			ToolSelect.Active = true;
		}
	}

	public Tilemap CurrentTilemap {
		get {
			return tilemap;
		}
		set {
			//ignore when there is no change
			if (tilemap == value)
				return;
			tilemap = value;
			TilemapChanged(value);
		}
	}


	public bool SnapToGrid {
		get {
			return snapToGrid;
		}
	}
	private bool snapToGrid = true;

	private static Application editorApplication;
	public static Application EditorApplication {
		get {
			return editorApplication;
		}
	}


	/// <summary>Write message on main windows's statusbar</summary>
	public void PrintStatus( string message )
	{
		sbMain.Remove( printStatusContextID, printStatusMessageID);
		sbMain.Push( printStatusContextID, message );
	}

	private Application(string[] args) {
		selection = new TileSelection();

		Glade.XML.CustomHandler = GladeCustomWidgetHandler;
		Glade.XML gxml = new Glade.XML("editor.glade", "MainWindow");
		gxml.Autoconnect(this);

		if (MainWindow == null)
			throw new Exception("Couldn't resolve all widgets");

		((GameObjectListWidget)ToolGObjectsProps).SetGtkFrame(fGObjects);

		Tileset.LoadEditorImages = true;

		// Initialize status bar for PrintStatus()
		printStatusContextID = sbMain.GetContextId("PrintStatus");
		printStatusMessageID = sbMain.Push(printStatusContextID, "Welcome to Supertux-Editor.");

		MainWindow.DeleteEvent += OnDelete;

		MainWindow.SetSizeRequest(900, 675);
		MainWindowTitlePrefix = MainWindow.Title;
		UpdateTitlebar();
		UpdateRecentDocuments();
		MainWindow.Icon = EditorStock.WindowIcon;
		//HACK: not a typo, EditorStock adds icons to the stock only when called 2x or more..
		MainWindow.Icon = EditorStock.WindowIcon;
		MainWindow.ShowAll();

		// Manually set icons for Tools, automatic stock initialization is broken on some systems
		ToolSelect.StockId = EditorStock.ToolSelect;
		ToolTiles.StockId = EditorStock.ToolTiles;
		ToolObjects.StockId = EditorStock.ToolObjects;
		ToolBrush.StockId = EditorStock.ToolBrush;
		ToolFill.StockId = EditorStock.ToolFill;
		ToolReplace.StockId = EditorStock.ToolReplace;
		ToolPath.StockId = EditorStock.ToolPath;
		ToolButtonCamera.StockId = EditorStock.Camera;

		// Hide some extra widgets (because MainWindow.ShowAll(); showed them all)
		fGObjects.Visible = false;
		if (Settings.Instance.ToolboxOnRight) 
		{
			aTools.Reparent(fToolsRight);
			fToolsLeft.Hide();
		} else {
			aTools.Reparent(fToolsLeft);
			fToolsRight.Hide();
		}


		// Tool "Select" is selected by default - call its event handler
		OnToolSelect(null, null);

		//Setup drag destination for "files"
		Gtk.Drag.DestSet(MainWindow, Gtk.DestDefaults.All, target_table,
				 Gdk.DragAction.Default |
				 Gdk.DragAction.Copy |
				 Gdk.DragAction.Move |
				 Gdk.DragAction.Link |
				 Gdk.DragAction.Private |
				 Gdk.DragAction.Ask);
		MainWindow.DragDataReceived += OnDragDataReceived;

		fileChooser = new Gtk.FileChooserDialog("Choose a Level", MainWindow, Gtk.FileChooserAction.Open, new object[] {});
		if (!Directory.Exists(Settings.Instance.LastDirectoryName)) {	//noexistent (or null) LastDirectoryName, resetting to default
			if( Settings.Instance.SupertuxData != null ) {
				Settings.Instance.LastDirectoryName = System.IO.Path.Combine(Settings.Instance.SupertuxData, "levels") + System.IO.Path.DirectorySeparatorChar;
			} else {
				Settings.Instance.LastDirectoryName = Environment.ExpandEnvironmentVariables("%HOME%");
			}
		}
		fileChooser.SetCurrentFolder(Settings.Instance.LastDirectoryName);
		fileChooser.AddButton(Gtk.Stock.Cancel, Gtk.ResponseType.Cancel);
		fileChooser.AddButton(Gtk.Stock.Ok, Gtk.ResponseType.Ok);
		fileChooser.DefaultResponse = Gtk.ResponseType.Ok;
		Gtk.FileFilter filter = new Gtk.FileFilter();
		filter.Name = "Supertux Levels and Worldmaps";
		filter.AddPattern("*.stl");
		filter.AddPattern("*.stwm");
		fileChooser.AddFilter( filter );
		Gtk.FileFilter levelfilter = new Gtk.FileFilter();
		levelfilter.Name = "Supertux Levels";
		levelfilter.AddPattern("*.stl");
		fileChooser.AddFilter( levelfilter );
		Gtk.FileFilter worldmapfilter = new Gtk.FileFilter();
		worldmapfilter.Name = "Supertux Worldmaps";
		worldmapfilter.AddPattern("*.stwm");
		fileChooser.AddFilter( worldmapfilter );
		Gtk.FileFilter brushfilter = new Gtk.FileFilter();
		brushfilter.Name = "Supertux-Editor Brushes";
		brushfilter.AddPattern("*.csv");
		fileChooser.AddFilter(brushfilter);
		Gtk.FileFilter all = new Gtk.FileFilter();
		all.Name = "All Files";
		all.AddPattern("*");
		fileChooser.AddFilter( all );
		if( Settings.Instance.SupertuxData != null ){
			try {
				fileChooser.AddShortcutFolder(System.IO.Path.Combine(Settings.Instance.SupertuxData, "levels"));
			} catch (Exception e) {
				LogManager.Log(LogLevel.Warning, "Couldn't add supertux level directory to File Chooser: " + e.Message);
			}
		}

		if (args.Length > 0) {
			Load(args[0]);
		}

		UndoManager.OnAddCommand += OnUndoManager;
		UndoManager.OnRedo += OnUndoManager;
		UndoManager.OnUndo += OnUndoManager;

		editorApplication = this;

		PrintStatus("Welcome to Supertux-Editor.");
	}

	private Gtk.Widget CreateTileList()
	{
		Gtk.VBox box = new Gtk.VBox();
		box.Homogeneous = false;

		Gtk.Adjustment vadjustment = new Gtk.Adjustment(0, 0, 100, 1, 10, 10);

		tileList = new TileListWidget(this, selection, vadjustment);
		TilegroupSelector selector = new TilegroupSelector(this, tileList);

		Gtk.HBox hbox = new Gtk.HBox(false, 0);

		Gtk.VScrollbar scrollbar = new Gtk.VScrollbar(vadjustment);

		hbox.PackStart(tileList, true, true, 0);
		hbox.PackEnd(scrollbar, false, true, 0);

		box.PackStart(selector, false, true, 0);
		box.PackStart(hbox, true, true, 0);

		return box;
	}

	private Gtk.Widget CreateObjectList()
	{
		Gtk.HBox hbox = new Gtk.HBox(false, 0);

		Gtk.Adjustment vadjustment = new Gtk.Adjustment(0, 0, 100, 1, 10, 10);
		Gtk.VScrollbar scrollbar = new Gtk.VScrollbar(vadjustment);

		hbox.PackStart(new ObjectListWidget(this, vadjustment), true, true, 0);
		hbox.PackEnd(scrollbar, false, true, 0);
		return hbox;
	}

	protected Gtk.Widget GladeCustomWidgetHandler(Glade.XML xml, string func_name, string name, string string1, string string2, int int1, int int2)
	{
		if(func_name == "TileList") {
			ToolTilesProps = CreateTileList();
			return ToolTilesProps;
		}
		if(func_name == "ObjectList") {
			ToolObjectsProps = CreateObjectList();
			return ToolObjectsProps;
		}
		if(func_name == "GObjectList") {
			ToolGObjectsProps = new GameObjectListWidget(this);
			return ToolGObjectsProps;
		}
		if(func_name == "SectorSwitchNotebook") {
			sectorSwitchNotebook = new SectorSwitchNotebook(this);
			sectorSwitchNotebook.ShowAll();
			return sectorSwitchNotebook;
		}
		if(func_name == "LayerList") {
			layerList = new LayerListWidget(this);
			return layerList;
		}
		if(func_name == "PropertiesView") {
			propertiesView = new PropertiesView(this);
			return propertiesView;
		}
		throw new Exception("No Custom Widget Handler named \""+func_name+"\" exists");
	}

	#region Tool Button Handlers

	/// <summary>Called when "Tool" menu is opened to update sensitivites</summary>
	public void OnMenuTool(object o, EventArgs args)
	{
		MenuItemToolPath.Sensitive = ToolPath.Sensitive;
	}

	protected void OnMenuToolSelect(object o, EventArgs args) {
		ToolSelect.Active = true;
	}

	protected void OnMenuToolTiles(object o, EventArgs args) {
		ToolTiles.Active = true;
	}

	protected void OnMenuToolObjects(object o, EventArgs args) {
		ToolObjects.Active = true;
	}

	protected void OnMenuToolBrush(object o, EventArgs args) {
		ToolBrush.Active = true;
	}

	protected void OnMenuToolFill(object o, EventArgs args) {
		ToolFill.Active = true;
	}

	protected void OnMenuToolReplace(object o, EventArgs args) {
		ToolReplace.Active = true;
	}

	protected void OnMenuToolPath(object o, EventArgs args) {
		ToolPath.Active = true;
	}

	protected void OnToolSelect(object o, EventArgs args) {
		if (ToolSelect.Active) {
			PrintStatus("Tool: Select");
			ToolSelectProps.Visible = true;
			ToolTilesProps.Visible = false;
			ToolObjectsProps.Visible = false;
			ToolBrushProps.Visible = false;
			SetTool(new ObjectSelectTool(this, CurrentSector));
		}
	}

	protected void OnToolZoom(object o, EventArgs args)
	{
		if (ToolZoom.Active) {
			PrintStatus("Tool: Zoom");
			ToolSelectProps.Visible  = false;
			ToolTilesProps.Visible   = false;
			ToolObjectsProps.Visible = false;
			ToolBrushProps.Visible   = false;
			if (level == null) return;
			SetTool(new ZoomTool(this));
		}
	}

	protected void OnToolTiles(object o, EventArgs args) {
		if (ToolTiles.Active) {
			PrintStatus("Tool: Tiles");
			ToolSelectProps.Visible = false;
			ToolTilesProps.Visible = true;
			ToolObjectsProps.Visible = false;
			ToolBrushProps.Visible = false;
			if (level == null) return;
			SetTool(new TilePaintTool(this, level.Tileset, selection));
		}
	}

	protected void OnToolObjects(object o, EventArgs args) {
		if (ToolObjects.Active) {
			PrintStatus("Tool: Objects");
			ToolSelectProps.Visible = false;
			ToolTilesProps.Visible = false;
			ToolObjectsProps.Visible = true;
			ToolBrushProps.Visible = false;
			SetTool(new ObjectSelectTool(this, CurrentSector));
		}
	}

	protected void OnToolBrush(object o, EventArgs args) {
		if (ToolBrush.Active) {
			PrintStatus("Tool: Brush");
			ToolSelectProps.Visible = false;
			ToolTilesProps.Visible = false;
			ToolObjectsProps.Visible = false;
			ToolBrushProps.Visible = true;
			SetTool(new ObjectSelectTool(this, CurrentSector));
		}
	}

	protected void OnToolFill(object o, EventArgs args) {
		if (ToolFill.Active) {
			PrintStatus("Tool: Fill");
			ToolSelectProps.Visible = false;
			ToolTilesProps.Visible = true;
			ToolObjectsProps.Visible = false;
			ToolBrushProps.Visible = false;
			if (level == null) return;
			SetTool(new TileFillTool(this, level.Tileset, selection));
		}
	}

	protected void OnToolReplace(object o, EventArgs args) {
		if (ToolReplace.Active) {
			PrintStatus("Tool: Replace");
			ToolSelectProps.Visible = false;
			ToolTilesProps.Visible = true;
			ToolObjectsProps.Visible = false;
			ToolBrushProps.Visible = false;
			if (level == null) return;
			SetTool(new TileReplaceTool(this, level.Tileset, selection));
		}
	}

	protected void OnToolPath(object o, EventArgs args) {
		if (!ToolPath.Active || iPathToEdit == null){
			ToolPath.Sensitive = false;
		} else {
			PrintStatus("Tool: Path editor");
			ToolSelectProps.Visible = false;
			ToolTilesProps.Visible = false;
			ToolObjectsProps.Visible = false;
			ToolBrushProps.Visible = false;
			if (level == null) return;
			if (iPathToEdit.Path == null) {					// Create new path if we have invalid one
				Path path = new Path();
				Path.Node pathNode = new Path.Node();
				if (iPathToEdit is IObject)				// Move it to object => preserve it's position (if applicable).
					pathNode.Pos = new Vector(((IObject)iPathToEdit).Area.Left, ((IObject)iPathToEdit).Area.Top);
				path.Nodes.Add(pathNode);
				iPathToEdit.Path = path;
			}
			SetTool(new PathTool(this, iPathToEdit.Path));
		}
	}

	#endregion Tool Button Handlers


	protected void OnHome(object o, EventArgs args) {
		if( sectorSwitchNotebook.CurrentRenderer != null ){
			sectorSwitchNotebook.CurrentRenderer.SetTranslation(new Vector(0, 0));
		}
	}

	protected void OnNormalSize(object o, EventArgs args) {
		if( sectorSwitchNotebook.CurrentRenderer != null ){
			sectorSwitchNotebook.CurrentRenderer.ZoomTo(1.0f/sectorSwitchNotebook.CurrentRenderer.GetZoom());
		}
	}

	protected void OnZoomIn(object o, EventArgs args) {
		if( sectorSwitchNotebook.CurrentRenderer != null ){
			//sectorSwitchNotebook.CurrentRenderer.ZoomIn();
			sectorSwitchNotebook.CurrentRenderer.ZoomTo((float) Math.Sqrt(2));
		}
	}

	protected void OnZoomOut(object o, EventArgs args) {
		if( sectorSwitchNotebook.CurrentRenderer != null ){
			//sectorSwitchNotebook.CurrentRenderer.ZoomOut();
			sectorSwitchNotebook.CurrentRenderer.ZoomTo(1/(float) Math.Sqrt(2));
		}
	}

	protected void OnZoomFit(object o, EventArgs args) {
		if( sectorSwitchNotebook.CurrentRenderer != null ) {
			sectorSwitchNotebook.CurrentRenderer.ZoomTo(new RectangleF(0, 0,
										   CurrentSector.Width*32, CurrentSector.Height*32));
		}
	}

	/// <summary>Create a new blank level</summary>
	protected void OnNew(object o, EventArgs args)
	{
		if (!ChangeConfirm("create a blank level"))
			return;

		try {
			UndoManager.Clear();
			Level level = LevelUtil.CreateLevel();
			CurrentLevel = level;
		} catch(Exception e) {
			ErrorDialog.Exception("Couldn't create new level", e);
		}
		fileName = null;
		UpdateUndoButtons();
		UpdateTitlebar();
		UndoManager.MarkAsSaved();
		ToolButtonCamera.Sensitive=true;
		EditProperties(CurrentLevel, "Level");
	}

	protected void OnOpen(object o, EventArgs e)
	{
		if (!ChangeConfirm("load a new level"))
			return;
		fileChooser.Title = "Choose a Level";
		fileChooser.Action = Gtk.FileChooserAction.Open;
		fileChooser.SetCurrentFolder(Settings.Instance.LastDirectoryName);
		fileChooser.Filter = fileChooser.Filters[0];
		int result = fileChooser.Run();
		fileChooser.Hide();
		if(result != (int) Gtk.ResponseType.Ok)
			return;

		Settings.Instance.LastDirectoryName = fileChooser.CurrentFolder;
		Settings.Instance.Save();
		Load(fileChooser.Filename);
		EditProperties(CurrentLevel, "Level");
	}

	private void Load(string fileName)
	{
		try {
			//undoSnapshots.Clear();
			UndoManager.Clear();
			Level newLevel = (Level) serializer.Read(fileName);
			if (newLevel.Version < 2) {
				ErrorDialog.ShowError("Couldn't load level: Old Level Format not supported",
				                      "Supertux-Editor does not support Supertux-0.1.x levels");
				return;
			}
			CurrentLevel = newLevel;
			this.fileName = fileName;
			Settings.Instance.addToRecentDocuments(fileName);
			Settings.Instance.Save();
			UpdateUndoButtons();
			UpdateTitlebar();
			UpdateRecentDocuments();
			UndoManager.MarkAsSaved();
			ToolButtonCamera.Sensitive=true;
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

		if(level == null)
			return;

		if(chooseName) {
			fileChooser.Title = "Select file to save Level";
			fileChooser.Action = Gtk.FileChooserAction.Save;
			fileChooser.SetCurrentFolder(Settings.Instance.LastDirectoryName);
			fileChooser.Filter = fileChooser.Filters[(level.isWorldmap)?2:1];
			int result = fileChooser.Run();
			fileChooser.Hide();
			if(result != (int) Gtk.ResponseType.Ok)
				return;
			Settings.Instance.LastDirectoryName = fileChooser.CurrentFolder;
			Settings.Instance.addToRecentDocuments(fileChooser.Filename);
			Settings.Instance.Save();
			UpdateRecentDocuments();
			fileName = fileChooser.Filename;
		}
		QACheck.ReplaceDeprecatedTiles(level);

		try {
			serializer.Write(fileName, level);
		} catch(Exception e) {
			ErrorDialog.Exception("Couldn't save level", e);
		}

		UndoManager.MarkAsSaved();
		UpdateTitlebar();
}

	protected void OnQuit(object o, EventArgs e)
	{
		Close();
	}

	protected void OnSnap(object o, EventArgs e)
	{
		snapToGrid = !snapToGrid;
		if( snapToGrid )
			PrintStatus("Snap objects to grid.");
		else
			PrintStatus("Snap to grid deactivated.");
	}

	protected void OnAbout(object o, EventArgs e)
	{
		string[] authors = new string[]{
			"Matthias \"MatzeB\" Braun",
			"",
			"Wolfgang Becker",
			"Christoph Sommer",
			"Arvid Norlander",
		};

		Gtk.AboutDialog dialog = new Gtk.AboutDialog();
		dialog.Icon = EditorStock.WindowIcon;
		dialog.ProgramName = "SuperTux Editor";
		dialog.Version = Constants.PACKAGE_VERSION;
		dialog.Comments = "A level and worldmap editor for SuperTux 0.3.x";
		dialog.Authors = authors;
		dialog.Copyright = "Copyright (c) 2006-2013 SuperTux Devel Team";
		dialog.License =
			"This program is free software; you can redistribute it and/or modify" + Environment.NewLine +
			"it under the terms of the GNU General Public License as published by" + Environment.NewLine +
			"the Free Software Foundation; either version 3 of the License, or" + Environment.NewLine +
			"(at your option) any later version." + Environment.NewLine +
			Environment.NewLine +
			"This program is distributed in the hope that it will be useful," + Environment.NewLine +
			"but WITHOUT ANY WARRANTY; without even the implied warranty of" + Environment.NewLine +
			"MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the" + Environment.NewLine +
			"GNU General Public License for more details." + Environment.NewLine +
			Environment.NewLine +
			"You should have received a copy of the GNU General Public License" + Environment.NewLine +
			"along with this program; if not, write to the Free Software Foundation, Inc.," + Environment.NewLine +
			"59 Temple Place, Suite 330, Boston, MA 02111-1307 USA" + Environment.NewLine;
		dialog.Website = "http://supertux.lethargik.org/";
		dialog.WebsiteLabel = "SuperTux on the Web";
		dialog.Run();
		dialog.Destroy();
	}

	/// <summary>Run the current version of the level in Supertux</summary>
	protected void OnPlay(object o, EventArgs args)
	{
		if(level == null)
			return;

		try {
			string tempName = System.IO.Path.GetTempPath();

			if(level.isWorldmap)
				tempName += "/supertux-editor.tmp.stwm";
			else
				tempName += "/supertux-editor.tmp.stl";

			serializer.Write(tempName, level);

			Process supertux_process = new Process();
			supertux_process.StartInfo.FileName = Settings.Instance.SupertuxExe;
			supertux_process.StartInfo.Arguments = "\"" + tempName + "\"";
String working_dir = System.IO.Path.GetDirectoryName(Settings.Instance.SupertuxExe);
			if (working_dir != null)
				supertux_process.StartInfo.WorkingDirectory = working_dir;

			/*
			 * Avoid problems with xdg-open being used wrongly and
			 * exceptions not being thrown. These problems happen
			 * when UseShellExecute = true.
			 */
			supertux_process.StartInfo.UseShellExecute = false;

			supertux_process.Start();
		} catch(System.ComponentModel.Win32Exception /*e*/) {
			ErrorDialog.ShowError("The SuperTux binary does not seem to exist." + Environment.NewLine +
			                      "Please set the correct location of it in the settings." + Environment.NewLine +
					      "(The current setting is `" + Settings.Instance.SupertuxExe + "')");
			
		} catch(Exception e) {
			ErrorDialog.Exception("Couldn't start supertux. You might try checking the configuration value for the path to the SuperTux binary.", e);
		}
	}

	protected void OnLevelProperties(object o, EventArgs args)
	{
		if(level == null)
			return;

		EditProperties(level, "Level");
	}

	protected void OnSettings(object o, EventArgs args)
	{
		new SettingsDialog(false);
	}

	protected void OnQACheck(object o, EventArgs args)
	{
		if(level == null)
			return;
		QACheck.CheckObjectDirections(level);
		foreach (Sector sector in level.Sectors)
			QACheck.CheckIds(this, sector, false);

		QACheck.CheckLicense(level);
	}

	protected void OnBrushLoad(object o, EventArgs args)
	{
		try {
			if (tilemap == null) {
				ErrorDialog.ShowError("Brush: No tilemap selected",
				                      "You have to select a tilemap before you load a brush");
				return;
			}

			fileChooser.Title = "Choose a Brush";
			fileChooser.Action = Gtk.FileChooserAction.Open;
			fileChooser.SetCurrentFolder(Settings.Instance.LastBrushDir);
			fileChooser.Filter = fileChooser.Filters[3];
			int result = fileChooser.Run();
			fileChooser.Hide();
			if(result != (int) Gtk.ResponseType.Ok)
				return;
			Settings.Instance.LastBrushDir = fileChooser.CurrentFolder;
			Settings.Instance.Save();
			string brushFile = fileChooser.Filename;

			TileBrushTool editor = new TileBrushTool(this, level.Tileset, brushFile);
			SetTool(editor);
		} catch(Exception e) {
			ErrorDialog.Exception(e);
		}
	}

	protected void OnBrushSaveAs(object o, EventArgs args)
	{
		try {
			ITool editor = sectorSwitchNotebook.CurrentRenderer.Editor;
			if(! (editor is TileBrushTool)) {
				ErrorDialog.ShowError("No brush editor active",
				                      "You need to open a brush before you can save changes to it");
				return;
			}
			TileBrushTool brushTool = (TileBrushTool) editor;

			fileChooser.Title = "Choose a Brush";
			fileChooser.Action = Gtk.FileChooserAction.Save;
			fileChooser.SetCurrentFolder(Settings.Instance.LastBrushDir);
			fileChooser.Filter = fileChooser.Filters[3];
			int result = fileChooser.Run();
			fileChooser.Hide();
			if(result != (int) Gtk.ResponseType.Ok)
				return;
			Settings.Instance.LastBrushDir = fileChooser.CurrentFolder;
			Settings.Instance.Save();
			string brushFile = fileChooser.Filename;

			brushTool.Brush.saveToFile(brushFile);
			/*
			try {
				LispSerializer serializer = new LispSerializer(typeof(Brush));
				serializer.Write(brushFile, brushEditor.Brush);
			} catch(Exception e) {
				ErrorDialog.Exception("Couldn't save brush", e);
			}
			*/

		} catch(Exception e) {
			ErrorDialog.Exception(e);
		}
	}

	private void OnDelete(object o, Gtk.DeleteEventArgs args)
	{
		Close();
		args.RetVal = true;
	}

	/// <summary>
	/// Ask if really continue if unsaved changes.
	/// </summary>
	/// <param name="act">What we would do ("quit", "close", "open another file" or such)</param>
	/// <returns>True if continue otherwise false</returns>
	private bool ChangeConfirm(string act) {
		if( UndoManager.IsDirty ) {
			Gtk.MessageDialog md = new Gtk.MessageDialog (MainWindow,
			                                      Gtk.DialogFlags.DestroyWithParent,
			                                      Gtk.MessageType.Warning,
			                                      Gtk.ButtonsType.None,
			                                      "Continue without saving changes?"+ Environment.NewLine + Environment.NewLine +"If you " + act + " without saving, changes since the last save will be discarded.");
			md.AddButton(Gtk.Stock.Cancel, Gtk.ResponseType.Cancel);
			md.AddButton("Save and close", Gtk.ResponseType.Accept);
			md.AddButton("Discard Changes", Gtk.ResponseType.Yes);

			Gtk.ResponseType result = (Gtk.ResponseType)md.Run ();
			md.Destroy();
			if (result == Gtk.ResponseType.Accept) {
				Save(false);
				return true;
			} else if (result != Gtk.ResponseType.Yes) {
				return false;
			}
		}
		return true;
	}

	private void Close()
	{
		//Really Quit?
		if (!ChangeConfirm("quit"))
			return;
		Settings.Instance.Save();
		MainWindow.Destroy();
		Gtk.Application.Quit();
	}

	public void SetTool(ITool editor)
	{
		if (sectorSwitchNotebook == null) return;
		if (sectorSwitchNotebook.CurrentRenderer == null) return;
		sectorSwitchNotebook.CurrentRenderer.Editor = editor;
		sectorSwitchNotebook.CurrentRenderer.QueueDraw();
	}

	public void SetToolSelect()
	{
		ToolSelect.Active = true;
		OnToolSelect(null,null);
	}

	public void SetToolTiles()
	{
		ToolTiles.Active = true;
		OnToolTiles(null,null);
	}

	public void SetToolZoom()
	{
		ToolZoom.Active = true;
		OnToolZoom(null,null);
	}

	public void SetToolObjects()
	{
		ToolObjects.Active = true;
		OnToolObjects(null,null);
	}

	public void SetToolBrush()
	{
		ToolBrush.Active = true;
		OnToolBrush(null,null);
	}

	public void SetToolFill()
	{
		ToolFill.Active = true;
		OnToolFill(null,null);
	}

	public void SetToolReplace()
	{
		ToolReplace.Active = true;
		OnToolReplace(null,null);
	}

	public void SetToolPath()
	{
		ToolPath.Active = true;
		OnToolPath(null,null);

	}

	public void EditProperties(object Object, string title)
	{
		propertiesView.SetObject(Object, title);

		if (Object is Path || Object is Path.Node)
			return;		//We want to keep currently edited IPathObject
		if (Object is IPathObject) {
			iPathToEdit = (IPathObject) Object;
			ToolPath.Sensitive = true;
		} else {
			iPathToEdit = null;
			ToolPath.Sensitive = false;
		}
	}

	public void UpdateUndoButtons()
	{
		ToolButtonUndo.Sensitive = (UndoManager.UndoCount > 0);
		ToolButtonRedo.Sensitive = (UndoManager.RedoCount > 0);
	}

	private void UpdateTitlebar() {
		string s = fileName != null ? fileName : "[No Name]";
		if (UndoManager.IsDirty) {
			MainWindow.Title = MainWindowTitlePrefix + " - " + s + "*";
		} else {
			MainWindow.Title = MainWindowTitlePrefix + " - " + s;
		}
	}

	private void UpdateRecentDocuments() {
		// remove all old MenuItems
		MenuItemMruEntries.ForEach(delegate(MruEntry o){ MenuItemLevel_menu.Remove(o.MenuItem); });
		MenuItemMruEntries.Clear();

		// find out where to insert the MenuItems
		int insertAt = 0;
		foreach (Gtk.Widget w in MenuItemLevel_menu.Children) {
			insertAt++;
			if (w == MenuItemMruBegin) break;
		}

		// add all new MenuItems
		Settings.Instance.RecentDocuments.ForEach(delegate(string fileName){
			string shortLabel = System.IO.Path.GetFileName(fileName);

			MruEntry e = new MruEntry();
			e.MenuItem = new Gtk.MenuItem(shortLabel);
			e.MenuItem.Activated += OnMruEntry;
			e.FileName = fileName;
			MenuItemMruEntries.Add(e);
			MenuItemLevel_menu.Insert(e.MenuItem, insertAt);
			e.MenuItem.Show();			//everything is hidden when created...
		});

	}

	/// <summary>
	/// Yes it is used for undo, redo and all of it, as we only need one.
	/// </summary>
	/// <param name="command"></param>
	private void OnUndoManager(Command command) {
		UpdateUndoButtons();
		UpdateTitlebar();
	}

	/// <summary>Revert level to last snapshot.</summary>
	public void Undo()
	{
		if (UndoManager.UndoCount < 1)
			return;
		string us = UndoManager.UndoTitle;
		UndoManager.Undo();
		PrintStatus("Undone: " + us );

		// Refresh View
		if ((sectorSwitchNotebook != null) && (sectorSwitchNotebook.CurrentRenderer != null)) sectorSwitchNotebook.CurrentRenderer.QueueDraw();
	}

	public void Redo() {
		if (UndoManager.RedoCount < 1)
			return;
		string us = UndoManager.RedoTitle;
		UndoManager.Redo();
		PrintStatus("Redone: " + us);

		// Refresh View
		if ((sectorSwitchNotebook != null) && (sectorSwitchNotebook.CurrentRenderer != null)) sectorSwitchNotebook.CurrentRenderer.QueueDraw();
	}

	public void OnUndo(object o, EventArgs args)
	{
		Undo();
	}

	public void OnRedo(object o, EventArgs args) {
		Redo();
	}

	public void EditCurrentCamera() {
		OnEditCamera(null, null);
	}

	public void DeleteCurrentPath() {
		SetToolSelect();
		Command command = new PropertyChangeCommand("Deleted path from " + iPathToEdit.GetType().ToString(),
							FieldOrProperty.Lookup(iPathToEdit.GetType().GetProperty("Path")), iPathToEdit, null);
		command.Do();
		UndoManager.AddCommand(command);
	}

	public void OnEditCamera(object o, EventArgs args) {
		Camera camera = null;
		foreach(IGameObject Object in CurrentSector.GetObjects()) {
			if (Object is Camera)
				camera = (Camera) Object;
		}
		if (camera == null) {
			camera = new Camera();
			CurrentSector.Add(camera, true);
		}
		EditProperties(camera, "Camera");
	}

	/// <summary>Called when "Edit" menu is opened</summary>
	public void OnMenuEdit(object o, EventArgs args)
	{
		string undoLabel = "Undo";
		MenuItemUndo.Sensitive = (UndoManager.UndoCount > 0);
		if (UndoManager.UndoCount > 0) undoLabel += ": " + UndoManager.UndoTitle;
		((Gtk.Label)MenuItemUndo.Child).Text = undoLabel;

		string redoLabel = "Redo";
		MenuItemRedo.Sensitive = (UndoManager.RedoCount > 0);
		if (UndoManager.RedoCount > 0) redoLabel += ": " + UndoManager.RedoTitle;
		((Gtk.Label)MenuItemRedo.Child).Text = redoLabel;
	}

	/// <summary>Called when an item of the RecentDocument MenuItems is chosen</summary>
	public void OnMruEntry(object o, EventArgs args)
	{

		// find out which MruEntry was chosen
		foreach (MruEntry mruEntry in MenuItemMruEntries) {
			if (mruEntry.MenuItem != o) continue;

			Load(mruEntry.FileName);
			break;
		}

	}

	private void OnDragDataReceived(object o, Gtk.DragDataReceivedArgs args)
	{
		string data = System.Text.Encoding.UTF8.GetString (args.SelectionData.Data);
		data += "\r\n";
		LogManager.Log(LogLevel.Debug, "Drag&Drop Uri-list received: {0}", data);

		int index = 0;		//index of first letter in filename
		string filename = "";

			// repeat until there's existing file that ends ".stl" or ".stwm"
		while (! (File.Exists(filename) && (filename.Substring(filename.Length - 4) == ".stl" || filename.Substring(filename.Length - 5) == ".stwm")))
		{
			index = data.IndexOf("file:", index);						//move pointer to next "file:"

			if (index == -1)
			{
				LogManager.Log(LogLevel.Debug, "		No more filenames... aborting");
				Gtk.Drag.Finish (args.Context, false, false, args.Time);
				return;
			}
			index += 5;									//move pointer after next "file:" to reach filename
			string uri = data.Substring(index, data.IndexOf('\n', index) - index -1 );
			filename = Uri.UnescapeDataString(uri);						//convert excape sequences ("%2b" = "+") to normal text
			LogManager.Log(LogLevel.Debug, "		Found filename: {0}", filename);
		}

		if (!ChangeConfirm("load a new level"))
		{
			Gtk.Drag.Finish (args.Context, false, false, args.Time);
			return;
		}

		Gtk.Drag.Finish (args.Context, true, false, args.Time);

		Load(filename);			//load the level
	}

	protected void OnToolboxSizeAllocated(object o, Gtk.SizeAllocatedArgs args)
	{
		Gtk.Frame fActive = (Gtk.Frame) o;
		Gtk.Frame fOther = (o == fToolsRight)?fToolsLeft:fToolsRight;
		Settings.Instance.ToolboxOnRight = (fActive == fToolsRight);		//recently touched frame becomes stored in preferences

		if (args.Allocation.Width < 5)
		{
			if (fOther.Visible)		//ignore, if the other frame is already visible;
				return;
			fActive.Show();
			fOther.Show();
		} else {
			if (!fOther.Visible)		//ignore, if the other frame is already hidden;
				return;
			fActive.WidthRequest=1;
			fActive.Show();
			aTools.Reparent(fActive);
			fOther.Hide();
		}
	}

	public static void Main(string[] args)
	{
		LispSerializer.SetupSerializers(typeof(Application).Assembly);
		
		Gtk.Application.Init();

		Application app = new Application(args);
#if !INSANEDEBUG
		try {
#endif
			Gtk.Application.Run();
#if !INSANEDEBUG
		} catch(Exception e) {
			if(app.level != null) {
				string filename = System.IO.Path.GetTempPath() + "/supertux-editor-emergency."+ ((app.level.isWorldmap)?"stwm":"stl");
				LogManager.Log(LogLevel.Fatal, "Unexpected Exception... Emergency save to '" + filename + "'");
				Console.Error.WriteLine(e.Message);
				app.serializer.Write(filename, app.level);
			}
			throw;
		}
#endif
		Settings.Instance.Save();

	}
}

/* EOF */
