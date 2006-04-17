using DataStructures;
using System;
using System.Reflection;
using Gdk;

public class ObjectCreationEditor : IEditor
{
	private IEditorApplication application;
	private Sector sector;
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
		IGameObject gameObject = (IGameObject) CreateObject();
		if(gameObject is IObject) {
			IObject obj = (IObject) gameObject;
			RectangleF rect = obj.Area;
			rect.MoveTo(pos);
			obj.ChangeArea(rect);
		}
		sector.Add(gameObject);
		Redraw();
		
		// switch back to object edit mode when shift was not pressed
		if((Modifiers & ModifierType.ShiftMask) == 0) {
			ObjectsEditor editor = new ObjectsEditor(application.CurrentSector);
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
	
	private object CreateObject()
	{
		// create object
		ConstructorInfo Constructor = objectType.GetConstructor(Type.EmptyTypes);
		if(Constructor == null)
			throw new Exception("Type '" + objectType + "' has no public constructor without arguments");
		object Result = Constructor.Invoke(new object[] {});

		return Result;
	}	
}
