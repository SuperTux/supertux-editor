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
public sealed class Sector : ICustomLispSerializer {
	[LispChild("name")]
	public string Name = "";
	[ChooseResourceSetting]
	[PropertyProperties(Tooltip = "Background music to use for the sector.")]
	[LispChild("music", Optional = true, Default = "")]
	public string Music = "";
	[LispChild("gravity", Optional = true, Default = 10f)]
	public float Gravity = 10f;
	[LispChild("init-script", Optional = true, Default = "")]
	[EditScriptSetting]
	public string InitScript = "";

	//[ChooseColorSetting]
	//[LispChild("ambient-light", Optional = true, Default = new Drawing.Color( 1f, 1f, 1f ) )]
	[ChooseColorSetting]
	[LispChild("ambient-light", Optional = true )]
	public Drawing.Color AmbientLight = new Drawing.Color( 1f, 1f, 1f );

	private List<IGameObject> GameObjects = new List<IGameObject> ();

	public event ObjectAddedHandler ObjectAdded;
	public event ObjectRemovedHandler ObjectRemoved;
	public event SizeChangedHandler SizeChanged;

	private class DynamicList : IEnumerable, ICollection {
		public Sector Sector;
		public Type ObjectsType;

		public IEnumerator GetEnumerator() {
			foreach(IGameObject obj in Sector.GameObjects) {
				if(ObjectsType.IsInstanceOfType(obj))
					yield return obj;
			}
		}

		// Just needed to implement the ICollection.
		bool ICollection.IsSynchronized {
			get {
				return false;
			}
		}

		// Just needed to implement the ICollection.
		object ICollection.SyncRoot {
			get {
				return this;
			}
		}

		// Untested, just needed to implement the ICollection.
		void ICollection.CopyTo(Array array, int index) {
			if (array == null)
				throw new ArgumentNullException("array");
			if (index < 0)
				throw new ArgumentOutOfRangeException("index");
			if (array.Rank != 1)
				throw new ArgumentException("array", "Array rank must be 1");
			if (index >= array.Length)
				throw new ArgumentException("index", "index is greater than the length of array");
			if (array is IGameObject[]) {
				if (array.Length - index < this.Count)
					throw new ArgumentException("array", "Array is too short.");
				int i = index;
				IGameObject[] GameObjectArray = (IGameObject[])array;
				foreach (IGameObject obj in Sector.GameObjects) {
					if (ObjectsType.IsInstanceOfType(obj)) {
						GameObjectArray[i] = obj;
						i++;
					}
				}
			} else {
				throw new ArgumentException("array", "Unsupported type");
			}
		}

		public int Count {
			get {
				int i = 0;
				foreach(IGameObject obj in Sector.GameObjects) {
					if (ObjectsType.IsInstanceOfType(obj))
						i++;
				}
				return i;
			}
		}


	}

	public ICollection GetObjects(Type ObjectsType) {
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

