using System;
using System.Collections.Generic;
using OpenGl;
using DataStructures;
using SceneGraph;
using Gtk;
using Gdk;

public class ObjectsEditor : IEditor
{
	private class PathNode : IObject, Node
	{
		private Path.PathNode node;
		
		public PathNode(Path.PathNode node) {
			this.node = node;
		}
		
		public bool Resizable {
			get {
				return false;
			}
		}
		
		public void ChangeArea(RectangleF Area)
		{
			node.X = Area.Left + 10;
			node.Y = Area.Top + 10;
		}
		
		public RectangleF Area {
			get {
				return new RectangleF(node.X - 10, node.Y - 10,
				                      20, 20);
			}
		}
		
		public void Draw()
		{
			gl.Color4f(0, 0, 1, 0.7f);
			gl.Disable(gl.TEXTURE_2D);
			
			float left = node.X - 10;
			float right = node.X + 10;
			float top = node.Y - 10;
			float bottom = node.Y + 10;
			
			gl.Begin(gl.QUADS);
			gl.Vertex2f(left, top);
			gl.Vertex2f(right, top);
			gl.Vertex2f(right, bottom);
			gl.Vertex2f(left, bottom);
			gl.End();
			
			gl.Enable(gl.TEXTURE_2D);
			gl.Color4f(1, 1, 1, 1);			
		}
		
		public Node GetSceneGraphNode() {
			return this;
		}
	}
	
	private class ControlPoint : IObject, Node
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
		
		public void Draw()
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
	
	private Sector sector;
	private IObject activeObject;
	private Vector pressPoint;
	private RectangleF originalArea;
	private bool dragging;
	private List<ControlPoint> controlPoints = new List<ControlPoint>();

	public event RedrawEventHandler Redraw;
	
	public ObjectsEditor(Sector sector)
	{
		this.sector = sector;
	}
	
	public void Draw()
	{
		if(activeObject != null) {
			IObject obj = activeObject;
			if(obj is ControlPoint)
				obj = ((ControlPoint) obj).Object;

			gl.Color4f(1, 0, 0, 0.7f);
			obj.GetSceneGraphNode().Draw();
			gl.Color4f(1, 1, 1, 1);
		}
		foreach(ControlPoint point in controlPoints) {
			point.Draw();
		}
	}

	public void OnMouseButtonPress(Vector pos, int button, ModifierType Modifiers)
	{
		if(button == 1 || button == 3) {
			if(activeObject == null || !activeObject.Area.Contains(pos)) {
				MakeActive(FindNext(pos));
			}
					
			if(activeObject != null && button == 1) {
				pressPoint = pos;
				originalArea = activeObject.Area;
				dragging = true;
			}
			
			Redraw();
		}
		if(button == 3) {
			PopupMenu(button);		
		}
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
		activeObject = Object;
		
		if(! (activeObject is ControlPoint))
			controlPoints.Clear();
		
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
	}
	
	private void PopupMenu(int button)
	{
		if(! (activeObject is IGameObject))
			return;
		
		Menu popupMenu = new Menu();
		
		MenuItem propertiesItem = new ImageMenuItem(Stock.Properties, null);
		propertiesItem.Activated += OnProperties;
		popupMenu.Append(propertiesItem);
		
		MenuItem cloneItem = new MenuItem("Clone");
		cloneItem.Activated += OnClone;
		cloneItem.Sensitive = activeObject is ICloneable;
		popupMenu.Append(cloneItem);
		
		MenuItem deleteItem = new ImageMenuItem(Stock.Delete, null);
		deleteItem.Activated += OnDelete;
		popupMenu.Append(deleteItem);
		
		popupMenu.ShowAll();
		popupMenu.Popup(); 
	}
	
	private void OnProperties(object o, EventArgs args)
	{
		if(activeObject == null)
			return;
		new SettingsWindow(activeObject.GetType().Name + " Object Properties", activeObject);
	}
	
	private void OnClone(object o, EventArgs args)
	{
		if(activeObject == null)
			return;
		
		try {
			object newObject = ((ICloneable) activeObject).Clone();
			IGameObject gameObject = (IGameObject) newObject;
			sector.Add(gameObject);
		} catch(Exception e) {
			ErrorDialog.Exception(e);
		}
	}
	
	private void OnDelete(object o, EventArgs args)
	{
		if(activeObject == null)
			return;
		sector.Remove((IGameObject) activeObject);
		activeObject = null;
	}

	public void OnMouseButtonRelease(Vector pos, int button, ModifierType Modifiers)
	{
		if(dragging) {
			dragging = false;
			
			if(pos != pressPoint) {
				moveObject(pos, (Modifiers & ModifierType.ShiftMask) != 0);
			} else {
				MakeActive(FindNext(pos));
				Redraw();
			}
		}
	}

	public void OnMouseMotion(Vector pos, ModifierType Modifiers)
	{
		if(dragging) {
			moveObject(pos, (Modifiers & ModifierType.ShiftMask) != 0);
		}
	}
	
	private void moveObject(Vector mousePos, bool snap)
	{
		Vector spos = new Vector(originalArea.Left, originalArea.Top);
		spos += mousePos - pressPoint;
		if(snap) {
			spos = new Vector((float) ((int)spos.X / 32) * 32,
		                      (float) ((int)spos.Y / 32) * 32);	
		}
		
		RectangleF newArea = new RectangleF(spos.X, spos.Y,
		                                       originalArea.Width,
		                                       originalArea.Height);
		activeObject.ChangeArea(newArea);
		Redraw();		
	}
}

