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
using Gtk;

public class GameObjectListWidget : IconView
{
	const int COL_NAME = 0;
	const int COL_OBJECT = 1;

	private IGameObject currentObject;
	private IToolApplication application;
	private Sector sector;
	private Gtk.Frame myFrame;

	public GameObjectListWidget(IToolApplication application)
	{
		this.application = application;
		ButtonPressEvent += OnButtonPressed;

		TextColumn = COL_NAME;

		application.SectorChanged += OnSectorChanged;
	}

	public void SetGtkFrame(Gtk.Frame myFrame)
	{
		this.myFrame = myFrame;
	}

	private void OnSectorChanged(Level level, Sector sector)
	{
		sector.ObjectAdded -= ObjectsChanged;
		sector.ObjectRemoved -= ObjectsChanged;

		this.sector = sector;

		sector.ObjectAdded += ObjectsChanged;
		sector.ObjectRemoved += ObjectsChanged;
		UpdateList();
	}

	private void ObjectsChanged(Sector sector, IGameObject Object)
	{
		if((Object is IObject) || (Object is ILayer))
			return;

		UpdateList();
	}

	private void UpdateList()
	{
		bool found = false;
		ListStore store = new ListStore(typeof(string), typeof(System.Object));
		foreach(IGameObject Object in sector.GetObjects()) {
			if (Object is ILayer)	//skip items moved into LayerListWidget
				continue;
			if (Object is Camera)	//skip Camera, it has Toolbar button and item in sectorSwitchNotebook context menu
				continue;
			if(! (Object is IObject)) {
				store.AppendValues(Object.GetType().Name, Object);
				found = true;
			}
		}
		Model = store;
		if (myFrame != null) myFrame.Visible = found;
	}

	[GLib.ConnectBefore]
		private void OnButtonPressed(object o, ButtonPressEventArgs args)
		{
			TreePath path = GetPathAtPos((int) args.Event.X, (int) args.Event.Y);
			if (path == null) return;

			TreeIter iter;
			if(!Model.GetIter(out iter, path))
				return;

			currentObject = (IGameObject) Model.GetValue(iter, COL_OBJECT);
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

		MenuItem deletePathItem = new MenuItem("Delete Path");
		deletePathItem.Activated += OnDeletePath;
		deletePathItem.Sensitive = currentObject is IPathObject;
		popupMenu.Append(deletePathItem);

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
		IPathObject pathObject = (IPathObject)currentObject;
		if (pathObject.Path == null) {
			pathObject.Path = new Path();
			pathObject.Path.Nodes.Add(new Path.Node());
		}
		application.SetToolPath();
	}

	private void OnDeletePath(object o, EventArgs args) {
		if (!(currentObject is IPathObject))
			return;
		IPathObject pathObject = (IPathObject)currentObject;
		if (pathObject.Path != null) {
			pathObject.Path = null;
			//application.SetEditor(null);
		}
	}

}

/* EOF */
