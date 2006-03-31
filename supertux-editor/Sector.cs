using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using Lisp;

[LispRootAttribute("sector")]
public class Sector : ICustomLispSerializer {
	[LispChild("name")]
	public string Name = "";
	[ChooseResourceSetting]
	[LispChild("music")]
	public string Music = "";
	[LispChild("gravity")]
	public float Gravity = 10.0f;
	[LispChild("init-script")]
	public string InitScript = "";

	public List<IGameObject> GameObjects = new List<IGameObject> ();

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
			SupertuxObjectAttribute objectAttribute
			= (SupertuxObjectAttribute) Attribute.GetCustomAttribute(type, typeof(SupertuxObjectAttribute));
			if(objectAttribute == null)
				continue;
			
			LispSerializer serializer = new LispSerializer(type);
			foreach(object Object in GetObjects(type)) {
				Writer.StartList(objectAttribute.Name);
				serializer.Write(Writer, Object);
				Writer.EndList(objectAttribute.Name);
			}
		}
		
	}
	
	public void FinishRead() {
	}
}

