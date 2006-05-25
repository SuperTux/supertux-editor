using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using Lisp;
using LispReader;

public delegate void ObjectAddedHandler(Sector sector, IGameObject Object);
public delegate void ObjectRemovedHandler(Sector sector, IGameObject Object);
public delegate void SizeChangedHandler(Sector sector);

[LispRootAttribute("sector")]
public class Sector : ICustomLispSerializer {
	[LispChild("name")]
	public string Name = "";
	[ChooseResourceSetting]
	[LispChild("music", Optional = true, Default = "")]
	public string Music = "";
	[LispChild("gravity", Optional = true, Default = 10f)]
	public float Gravity = 10f;
	[LispChild("init-script", Optional = true, Default = "")]
	[EditScriptSetting]	
	public string InitScript = "";

	private List<IGameObject> GameObjects = new List<IGameObject> ();
	
	public event ObjectAddedHandler ObjectAdded;
	public event ObjectRemovedHandler ObjectRemoved;
	public event SizeChangedHandler SizeChanged;

	private class DynamicList : IEnumerable {
		public Sector Sector;
		public Type ObjectsType;

		public IEnumerator GetEnumerator() {
			foreach(IGameObject obj in Sector.GameObjects) {
				if(ObjectsType.IsInstanceOfType(obj))
					yield return obj;
			}
		}
	}
	public IEnumerable GetObjects(Type ObjectsType) {
		DynamicList Result = new DynamicList();
		Result.Sector = this;
		Result.ObjectsType = ObjectsType;
		return Result;
	}
	public IEnumerable<IGameObject> GetObjects() {
		return GameObjects;
	}
	
	public void Add(IGameObject Object)
	{
		GameObjects.Add(Object);
		try {
			if(ObjectAdded != null)
				ObjectAdded(this, Object);
		} catch(Exception e) {
			ErrorDialog.Exception(e);
		}
	}
	
	public void Remove(IGameObject Object)
	{
		GameObjects.Remove(Object);
		try {
			if(ObjectRemoved != null)
				ObjectRemoved(this, Object);
		} catch(Exception e) {
			ErrorDialog.Exception(e);
		}
	}
	
	public void EmitSizeChanged()
	{
		if(SizeChanged != null)
			SizeChanged(this);
	}
	
	public void CustomLispRead(Properties props) {
		foreach(Type type in this.GetType().Assembly.GetTypes()) {
			SupertuxObjectAttribute objectAttribute
			= (SupertuxObjectAttribute) Attribute.GetCustomAttribute(type, typeof(SupertuxObjectAttribute));
			if(objectAttribute == null)
				continue;
			
			LispSerializer serializer = new LispSerializer(type);
			foreach(List list in props.GetList(objectAttribute.Name)) {
				IGameObject Object = (IGameObject) serializer.Read(list);
				GameObjects.Add(Object);
			}
		}
	}
	
	public void CustomLispWrite(Writer Writer) {
		foreach(Type type in this.GetType().Assembly.GetTypes()) {
			SupertuxObjectAttribute objectAttribute = (SupertuxObjectAttribute)
				Attribute.GetCustomAttribute(type, typeof(SupertuxObjectAttribute));
			if(objectAttribute == null)
				continue;
			
			string name = objectAttribute.Name;
			LispSerializer serializer = new LispSerializer(type);
			foreach(object Object in GetObjects(type)) {
				serializer.Write(Writer, name, Object);
			}
		}
	}
	
	public void FinishRead() {
	}
}

