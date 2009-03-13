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
	private Vector mousePoint;	//used when drawing selections
	private RectangleF originalArea;
	private bool dragging;
	private bool selecting;
	private List<ControlPoint> controlPoints = new List<ControlPoint>();
	private List<IObject> selectedObjects = new List<IObject>();

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
		if (selecting && (pressPoint - mousePoint).Norm() >= 10) {
			gl.Color4f(0, 0.7f, 1, 0.7f);
			gl.Disable(gl.TEXTURE_2D);

			gl.Begin(gl.QUADS);
			gl.Vertex2f(pressPoint.X, pressPoint.Y);
			gl.Vertex2f(mousePoint.X, pressPoint.Y);
			gl.Vertex2f(mousePoint.X, mousePoint.Y);
			gl.Vertex2f(pressPoint.X, mousePoint.Y);
			gl.End();

			gl.Enable(gl.TEXTURE_2D);
			gl.Color4f(1, 1, 1, 1);
		} else {
			foreach(IObject selectedObject in selectedObjects) {
				IObject obj = selectedObject;
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
	}

	public void OnMouseButtonPress(Vector mousePos, int button, ModifierType Modifiers)
	{
		if (button == 1) {
			if (selecting) {
				selecting = false;
			} else {
				if (activeObject == null || !activeObject.Area.Contains(mousePos)) {
					MakeActive(FindNext(mousePos));
				}

				if(activeObject != null && button == 1) {
					pressPoint = mousePos;
					originalArea = activeObject.Area;
					dragging = true;
				}
			}

			Redraw();
		}
		if (button == 3) {
			if (dragging) {
				activeObject.ChangeArea(originalArea);
				dragging = false;
			} else {
				pressPoint = mousePos;
				mousePoint = mousePos;
				selecting = true;
			}

			Redraw();
		}
	}

	public void OnMouseButtonRelease(Vector mousePos, int button, ModifierType Modifiers)
	{
		if (button == 1 && dragging) {
			dragging = false;

			if (mousePos != pressPoint) {
				RectangleF newArea = getNewPosition(mousePos, SnapValue(Modifiers));
				if (originalArea != newArea) {
					ObjectAreaChangeCommand command = new ObjectAreaChangeCommand(
						"Moved Object " + activeObject,
						originalArea,
						newArea,
						activeObject);
					UndoManager.AddCommand(command);
					moveObject(mousePos, SnapValue(Modifiers));
				}
			} else {
				MakeActive(FindNext(mousePos));
				Redraw();
			}
		}
		if (button == 3 && selecting) {
			selecting = false;
			if ((pressPoint - mousePos).Norm() < 10) {	//for moves within 10px circle around pressPoint show popup menu
				bool hit = false;
				foreach (IObject selectedObject in selectedObjects) {
					hit |= selectedObject.Area.Contains(mousePos);
				}				
				if (!hit) {
					MakeActive(FindNext(mousePos));
					Redraw();
				}
				if (hit || activeObject != null)	//Show popup menu when clicked on object (from selection or new one)
					PopupMenu(button);
			} else {
				activeObject = null;
				selectedObjects.Clear();
				controlPoints.Clear();

				RectangleF selectedArea = new RectangleF(pressPoint, mousePos);
				foreach(IObject Object in sector.GetObjects(typeof(IObject))) {
					if (selectedArea.Contains(Object.Area)) {
						selectedObjects.Add(Object);
					}
				}
				if (selectedObjects.Count == 1)		//show properties
					application.EditProperties(selectedObjects[0], selectedObjects[0].GetType().Name);
				else
					application.EditProperties(selectedObjects, "Group of " + selectedObjects.Count.ToString() + " objects");
				Redraw();
			}
		}
	}

	public void OnMouseMotion(Vector mousePos, ModifierType Modifiers)
	{
		if(dragging) {
			moveObject(mousePos, SnapValue(Modifiers));
		}
		if (selecting) {
			mousePoint = mousePos;
			Redraw();
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
			selectedObjects.Clear();
			selectedObjects.Add(Object);

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
		if (Object == null)
			selectedObjects.Clear();	//Ensure empty list when making active null object (temporary and intended fix)
	}

	private void OnObjectRemoved(Sector sector, IGameObject Object) {
		if (activeObject==Object) {
			activeObject = null;
		}
		if (selectedObjects.Contains(Object as IObject)) {
			selectedObjects.Remove(Object as IObject);
			Redraw();
		}
	}

	private void PopupMenu(int button)
	{
		if(selectedObjects.Count == 0)
			return;

		if(activeObject is ControlPoint)
			return;

		bool groupCloneable = true;	//foreach cycle to get required statistics
		foreach (IObject selectedObject in selectedObjects) {
			groupCloneable &= selectedObject is ICloneable;
		}

		Menu popupMenu = new Menu();

		MenuItem cloneItem = new MenuItem("Clone");
		cloneItem.Activated += OnClone;
		cloneItem.Sensitive = groupCloneable;
		popupMenu.Append(cloneItem);

		if(selectedObjects.Count == 1 && selectedObjects[0] is IPathObject) {
			IPathObject pathObject = (IPathObject) selectedObjects[0];

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
		List<IObject> Objects = new List<IObject>(selectedObjects);
		foreach (IGameObject selectedObject in Objects)
			try {
				object newObject = ((ICloneable) selectedObject).Clone();
				IGameObject gameObject = (IGameObject) newObject;
				sector.Add(gameObject, gameObject.GetType().Name + " (clone)");
			} catch(Exception e) {
				ErrorDialog.Exception(e);
		}
	}

	private void OnEditPath(object o, EventArgs args)
	{
		IPathObject pathObject = (IPathObject) selectedObjects[0];
		if (pathObject.Path == null) {
			// We need to get area before or it may have changed due to adding
			// the path.
			RectangleF area = selectedObjects[0].Area;
			pathObject.Path = new Path();
			pathObject.Path.Nodes.Add(new Path.Node());
			// Move path to object.
			pathObject.Path.Move(new Vector(area.Left, area.Top));
		}
		application.SetToolPath();
	}

	private void OnDeletePath(object o, EventArgs args)
	{
		IPathObject pathObject = (IPathObject) selectedObjects[0];
		if (pathObject.Path != null) {
			Command command = new PropertyChangeCommand("Removed path from " + selectedObjects[0],
				LispReader.FieldOrProperty.Lookup(typeof(IPathObject).GetProperty("Path")),
				selectedObjects[0],
				null);
			command.Do();
			UndoManager.AddCommand(command);
		}
	}

	private void OnDelete(object o, EventArgs args)
	{
		List<IObject> Objects = new List<IObject>(selectedObjects);
		foreach (IGameObject selectedObject in Objects)
			sector.Remove(selectedObject);
	}

	private RectangleF getNewPosition(Vector mousePos, int snap) {
		Vector spos = new Vector(originalArea.Left, originalArea.Top);
		spos += mousePos - pressPoint;
		if (snap > 0) {
			spos.X += snap / 2;	//PressPoint would be cutting edge instead of "center" without this correction
			spos.Y += snap / 2;
			// TODO: Get this right for area objects, they currently snap to the
			//       handle instead of the actual object...
			spos = new Vector((float) ((int) spos.X / snap) * snap - ((spos.X<0)?snap:0),
			                  (float) ((int) spos.Y / snap) * snap - ((spos.Y<0)?snap:0));
		}

		return new RectangleF(spos.X, spos.Y,
		                      originalArea.Width,
		                      originalArea.Height);
	}
}
