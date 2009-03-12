//  $Id$
using System;
using System.Collections.Generic;
using OpenGl;
using DataStructures;
using SceneGraph;
using Gtk;
using Gdk;
using Undo;

public sealed class ObjectsEditor : ObjectEditorBase, IEditor, IDisposable
{
	private sealed class ControlPoint : IObject, Node
	{
		public enum AttachPoint {
			TOP = 1,
			BOTTOM = 2,
			LEFT = 4,
			RIGHT = 8
		};

		private RectangleF area;
		private AttachPoint attachPoint;
		public IObject Object;

		private static float DISTANCE = 12;
		private static float SIZE = 16;

		public ControlPoint(IObject Object, AttachPoint attachPoint)
		{
			this.Object = Object;
			this.attachPoint = attachPoint;
		}

		public void Draw(Gdk.Rectangle cliprect)
		{
			UpdatePosition();
			gl.Color4f(0, 0, 1, 0.7f);
			gl.Disable(gl.TEXTURE_2D);

			gl.Begin(gl.QUADS);
			gl.Vertex2f(Area.Left, Area.Top);
			gl.Vertex2f(Area.Right, Area.Top);
			gl.Vertex2f(Area.Right, Area.Bottom);
			gl.Vertex2f(Area.Left, Area.Bottom);
			gl.End();

			gl.Enable(gl.TEXTURE_2D);
			gl.Color4f(1, 1, 1, 1);
		}

		public void UpdatePosition()
		{
			Vector pos;
			if((attachPoint & AttachPoint.TOP) != 0) {
				pos.Y = Object.Area.Top - DISTANCE;
			} else if((attachPoint & AttachPoint.BOTTOM) != 0) {
				pos.Y = Object.Area.Bottom + DISTANCE;
			} else {
				pos.Y = (Object.Area.Top + Object.Area.Bottom) / 2f;
			}
			if((attachPoint & AttachPoint.LEFT) != 0) {
				pos.X = Object.Area.Left - DISTANCE;
			} else if((attachPoint & AttachPoint.RIGHT) != 0) {
				pos.X = Object.Area.Right + DISTANCE;
			} else {
				pos.X = (Object.Area.Left + Object.Area.Right) / 2f;
			}
			area = new RectangleF(pos.X - SIZE/2f, pos.Y - SIZE/2f, SIZE, SIZE);
		}

		public void ChangeArea(RectangleF Area)
		{
			float adjust = SIZE/2f + DISTANCE;

			RectangleF newArea = Object.Area;
			if((attachPoint & AttachPoint.TOP) != 0) {
				newArea.Top = Area.Top + adjust;
			} else if((attachPoint & AttachPoint.BOTTOM) != 0) {
				newArea.Bottom = Area.Bottom - adjust;
			}
			if((attachPoint & AttachPoint.LEFT) != 0) {
				newArea.Left = Area.Left + adjust;
			} else if((attachPoint & AttachPoint.RIGHT) != 0) {
				newArea.Right = Area.Right - adjust;
			}
			Object.ChangeArea(newArea);
			area = Area;
		}

		public bool Resizable {
			get {
				return false;
			}
		}

		public RectangleF Area {
			get {
				return area;
			}
		}

		public Node GetSceneGraphNode() {
			return this;
		}
	}

	private sealed class ObjectAreaChangeCommand : Command {
		/// <summary>
		/// The object this action was on
		/// </summary>
		private IObject changedObject;
		/// <summary>
		/// Old area
		/// </summary>
		private RectangleF originalArea;
		/// <summary>
		/// New area
		/// </summary>
		private RectangleF newArea;

		public override void Do() {
			changedObject.ChangeArea(newArea);
		}

		public override void Undo() {
			changedObject.ChangeArea(originalArea);
		}

		public ObjectAreaChangeCommand(string title,
		                               RectangleF originalArea, RectangleF newArea,
		                               IObject changedObject)
			: base(title) {
			this.changedObject = changedObject;
			this.originalArea = originalArea;
			this.newArea = newArea;
		}
	}

	private IObject activeObject;
	private Vector pressPoint;
	private RectangleF originalArea;
	private bool dragging;
	// Used to make sure we just do undo snapshot when moving.
	//private bool moveStarted;
	private List<ControlPoint> controlPoints = new List<ControlPoint>();

	public event RedrawEventHandler Redraw;

	public ObjectsEditor(IEditorApplication application, Sector sector)
	{
		this.application = application;
		this.sector = sector;
		if (sector != null) sector.ObjectRemoved += OnObjectRemoved;
	}

	public void Dispose()
	{
		if (sector != null) sector.ObjectRemoved -= OnObjectRemoved;
	}

	public void Draw(Gdk.Rectangle cliprect)
	{
		if(activeObject != null) {
			IObject obj = activeObject;
			if(obj is ControlPoint)
				obj = ((ControlPoint) obj).Object;

			gl.Color4f(1, 0, 0, 0.7f);
			obj.GetSceneGraphNode().Draw(cliprect);
			gl.Color4f(1, 1, 1, 1);
		}
		foreach(ControlPoint point in controlPoints) {
			point.Draw(cliprect);
		}
	}

	public void OnMouseButtonPress(Vector mousePos, int button, ModifierType Modifiers)
	{
		if(button == 1 || button == 3) {
			if (activeObject == null || !activeObject.Area.Contains(mousePos)) {
				MakeActive(FindNext(mousePos));
			}

			if(activeObject != null && button == 1) {
				pressPoint = mousePos;
				originalArea = activeObject.Area;
				dragging = true;
			}

			Redraw();
		}
		if(button == 3) {
			PopupMenu(button);
		}
	}

	public void OnMouseButtonRelease(Vector mousePos, int button, ModifierType Modifiers)
	{
		if(dragging) {
			dragging = false;

			if (mousePos != pressPoint) {
				//moveStarted = false;
				ObjectAreaChangeCommand command = new ObjectAreaChangeCommand(
					"Moved Object " + activeObject,
					originalArea,
					getNewPosition(mousePos, SnapValue(Modifiers)),
					activeObject);
				UndoManager.AddCommand(command);
				moveObject(mousePos, SnapValue(Modifiers));
			} else {
				MakeActive(FindNext(mousePos));
				Redraw();
			}
		}
	}

	public void OnMouseMotion(Vector mousePos, ModifierType Modifiers)
	{
		if(dragging) {
			//if (!moveStarted) {
			//	application.TakeUndoSnapshot("Moved Object " + activeObject);
			//	moveStarted = true;
			//}
			moveObject(mousePos, SnapValue(Modifiers));
		}
	}

	private void moveObject(Vector mousePos, int snap)
	{
		RectangleF newArea = getNewPosition(mousePos, snap);
		activeObject.ChangeArea(newArea);
		Redraw();
	}

	private IObject FindNext(Vector pos)
	{
		foreach(ControlPoint point in controlPoints) {
			if(point.Area.Contains(pos)) {
				return point;
			}
		}

		IObject firstObject = null;
		bool foundLastActive = false;
		// Cycle through the objects which share a point
		foreach(IObject Object in sector.GetObjects(typeof(IObject))) {
			if(Object.Area.Contains(pos)) {
				if(firstObject == null)
					firstObject = Object;

				if(Object == activeObject) {
					foundLastActive = true;
					continue;
				}
				if(foundLastActive) {
					return Object;
				}
			}
		}

		if(firstObject != null)
			return firstObject;

		return null;
	}

	public void MakeActive(IObject Object)
	{
		if (activeObject != Object) {		//ignore MakeActive(activeObject)
			activeObject = Object;

			if(! (activeObject is ControlPoint)) {
				if(activeObject != null)
					application.EditProperties(activeObject, activeObject.GetType().Name);
				controlPoints.Clear();
			}

			if(activeObject != null && activeObject.Resizable) {
				controlPoints.Add(new ControlPoint(activeObject,
					                           ControlPoint.AttachPoint.TOP | ControlPoint.AttachPoint.LEFT));
				controlPoints.Add(new ControlPoint(activeObject,
					                           ControlPoint.AttachPoint.TOP));
				controlPoints.Add(new ControlPoint(activeObject,
					                           ControlPoint.AttachPoint.TOP | ControlPoint.AttachPoint.RIGHT));
				controlPoints.Add(new ControlPoint(activeObject,
					                           ControlPoint.AttachPoint.LEFT));
				controlPoints.Add(new ControlPoint(activeObject,
					                           ControlPoint.AttachPoint.RIGHT));
				controlPoints.Add(new ControlPoint(activeObject,
					                           ControlPoint.AttachPoint.BOTTOM | ControlPoint.AttachPoint.LEFT));
				controlPoints.Add(new ControlPoint(activeObject,
					                           ControlPoint.AttachPoint.BOTTOM));
				controlPoints.Add(new ControlPoint(activeObject,
					                           ControlPoint.AttachPoint.BOTTOM | ControlPoint.AttachPoint.RIGHT));
			}
			application.PrintStatus("ObjectsEditor:MakeActive(" + activeObject + ")");
		}
	}

	private void OnObjectRemoved(Sector sector, IGameObject Object) {
		if (activeObject==Object) {
			activeObject = null;
			Redraw();
		}
	}

	private void PopupMenu(int button)
	{
		if(! (activeObject is IGameObject))
			return;

		Menu popupMenu = new Menu();

		MenuItem cloneItem = new MenuItem("Clone");
		cloneItem.Activated += OnClone;
		cloneItem.Sensitive = activeObject is ICloneable;
		popupMenu.Append(cloneItem);

		if(activeObject is IPathObject) {
			IPathObject pathObject = (IPathObject) activeObject;

			MenuItem editPathItem = new MenuItem("Edit Path");
			editPathItem.Activated += OnEditPath;
			popupMenu.Append(editPathItem);

			if (pathObject.PathRemovable) {
				MenuItem deletePathItem = new MenuItem("Delete Path");
				deletePathItem.Sensitive = pathObject.Path != null;
				deletePathItem.Activated += OnDeletePath;
				popupMenu.Append(deletePathItem);
			}
		}

		MenuItem deleteItem = new ImageMenuItem(Stock.Delete, null);
		deleteItem.Activated += OnDelete;
		popupMenu.Append(deleteItem);

		popupMenu.ShowAll();
		popupMenu.Popup();
	}

	private void OnClone(object o, EventArgs args)
	{
		if(activeObject == null)
			return;

		try {
			object newObject = ((ICloneable) activeObject).Clone();
			IGameObject gameObject = (IGameObject) newObject;
			sector.Add(gameObject, gameObject.GetType().Name + " (clone)");
		} catch(Exception e) {
			ErrorDialog.Exception(e);
		}
	}

	private void OnEditPath(object o, EventArgs args)
	{
		IPathObject pathObject = (IPathObject) activeObject;
		if (pathObject.Path == null) {
			// We need to get area before or it may have changed due to adding
			// the path.
			RectangleF area = activeObject.Area;
			pathObject.Path = new Path();
			pathObject.Path.Nodes.Add(new Path.Node());
			// Move path to object.
			pathObject.Path.Move(new Vector(area.Left, area.Top));
		}
		application.SetToolPath();
	}

	private void OnDeletePath(object o, EventArgs args)
	{
		IPathObject pathObject = (IPathObject) activeObject;
		if (pathObject.Path != null) {
			Command command = new PropertyChangeCommand("Removed path from " + activeObject,
				LispReader.FieldOrProperty.Lookup(typeof(IPathObject).GetProperty("Path")),
				activeObject,
				null);
			command.Do();
			UndoManager.AddCommand(command);
		}
	}

	private void OnDelete(object o, EventArgs args)
	{
		if(activeObject == null)
			return;
		sector.Remove((IGameObject) activeObject);
	}

	private RectangleF getNewPosition(Vector mousePos, int snap) {
		Vector spos = new Vector(originalArea.Left, originalArea.Top);
		spos += mousePos - pressPoint;
		if (snap > 0) {
			// TODO: Get this right for area objects, they currently snap to the
			//       handle instead of the actual object...
			spos = new Vector((float) ((int) spos.X / snap) * snap,
			                  (float) ((int) spos.Y / snap) * snap);
		}

		return new RectangleF(spos.X, spos.Y,
		                      originalArea.Width,
		                      originalArea.Height);
	}
}
