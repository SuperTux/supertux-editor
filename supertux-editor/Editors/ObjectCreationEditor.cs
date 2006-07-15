using DataStructures;
using System;
using System.Reflection;
using Gdk;

public sealed class ObjectCreationEditor : ObjectEditorBase, IEditor
{
	private Type objectType;
	public event RedrawEventHandler Redraw;
	
	public ObjectCreationEditor(IEditorApplication application, Sector sector, Type objectType)
	{
		this.application = application;
		this.sector = sector;
		this.objectType = objectType;
	}
	
	public void Draw()
	{
		// TODO draw image of the object to create
	}
	
	public void OnMouseButtonPress(Vector pos, int button, ModifierType Modifiers)
	{
		application.TakeUndoSnapshot( "Created Object '" + objectType + "'" );
		IGameObject gameObject = CreateObjectAt(pos);
		
		// switch back to object edit mode when shift was not pressed
		if((Modifiers & ModifierType.ShiftMask) == 0) {
			ObjectsEditor editor = new ObjectsEditor(application, application.CurrentSector);
			if(gameObject is IObject) {
				editor.MakeActive((IObject) gameObject);
			}
			application.SetEditor(editor);
		}
	}

	public void OnMouseButtonRelease(Vector pos, int button, ModifierType Modifiers)
	{
	}

	public void OnMouseMotion(Vector pos, ModifierType Modifiers)
	{
	}

	private IGameObject CreateObjectAt(Vector pos)
	{
		if( application.SnapToGrid ){
			int snap = 32;
			pos = new Vector((float) ((int)pos.X / snap) * snap,
			                 (float) ((int)pos.Y / snap) * snap);
		}
		IGameObject gameObject = (IGameObject) CreateObject();
		if(gameObject is IObject) {
			IObject obj = (IObject) gameObject;
			RectangleF rect = obj.Area;
			rect.MoveTo(pos);
			obj.ChangeArea(rect);
		}
		if(gameObject is IPathObject) {
			Path path = ((IPathObject) gameObject).Path;
			if( path != null )
				path.Nodes[0].Pos = pos;
		}
		
		sector.Add(gameObject);
		Redraw();
		return gameObject;
	}
	
	private object CreateObject()
	{
		// create object
		ConstructorInfo Constructor = objectType.GetConstructor(Type.EmptyTypes);
		if(Constructor == null)
			throw new Exception("Type '" + objectType + "' has no public constructor without arguments");
		object Result = Constructor.Invoke(new object[] {});
		
		//some Objects need special treatment
		if( Result is Tilemap ){
			uint width = 0;
			uint height = 0;
			foreach(Tilemap tilemap in sector.GetObjects(typeof(Tilemap))) {
				if(tilemap.Width > width)
					width = tilemap.Width;
				if(tilemap.Height > height)
					height = tilemap.Height;
			}
			((Tilemap) Result).Resize( width, height, 0);
		}
		return Result;
	}
}
