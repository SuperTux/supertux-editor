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
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using Lisp;
using LispReader;
using Undo;

public delegate void ObjectAddedHandler(Sector sector, IGameObject Object);
public delegate void ObjectRemovedHandler(Sector sector, IGameObject Object);
public delegate void SizeChangedHandler(Sector sector);

[LispRootAttribute("sector")]
public sealed class Sector : ICustomLispSerializer {
	public static bool SortObjectTags = true;

	[LispChild("name")]
	public string Name = String.Empty;

	[ChooseResourceSetting]
	[PropertyProperties(Tooltip = "Background music to use for the sector.")]
	[LispChild("music", Optional = true, Default = "")]
	public string Music = String.Empty;

	[PropertyProperties(Tooltip = "Gravity in sector, currently broken(?)")]
	[LispChild("gravity", Optional = true, Default = 10f)]
	public float Gravity = 10f;

	[LispChild("init-script", Optional = true, Default = "")]
	[EditScriptSetting]
	public string InitScript = String.Empty;

	//[ChooseColorSetting]
	//[LispChild("ambient-light", Optional = true, Default = new Drawing.Color( 1f, 1f, 1f ) )]
	[ChooseColorSetting]
	[LispChild("ambient-light", Optional = true, Default = "Color(1, 1, 1, 1)")]
	public Drawing.Color AmbientLight = new Drawing.Color( 1f, 1f, 1f );

	private List<IGameObject> GameObjects = new List<IGameObject> ();
	private int height;
	private int width;

	public event ObjectAddedHandler ObjectAdded;
	public event ObjectRemovedHandler ObjectRemoved;
	public event SizeChangedHandler SizeChanged;

	public int Height{
		get {
			return height;
		}
	}

	public int Width{
		get {
			return width;
		}
	}

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

	public void Add(IGameObject Object) {
		Add(Object, "unknown", false);
	}
	public void Add(IGameObject Object, string type) {
		Add(Object, type, false);
	}
	public void Add(IGameObject Object, bool NoUndo) {
		Add(Object, "unknown", NoUndo);
	}

	public void Add(IGameObject Object, string type, bool NoUndo)
	{
		Command command = null;
		Add(Object, type, NoUndo, ref command);
		if (!NoUndo)
			UndoManager.AddCommand(command);
	}

	//use this one if you want to handle resulting undo command yourself
	public void Add(IGameObject Object, string type, ref Command command)
	{
		Add(Object, type, false, ref command);
	}

	private void Add(IGameObject Object, string type, bool NoUndo, ref Command command)
	{
		if (!NoUndo) {
			command = new ObjectAddCommand(
				"Created Object '" + type + "'",
				Object,
				this);
		}
		GameObjects.Add(Object);
		try {
			if(ObjectAdded != null)
				ObjectAdded(this, Object);
		} catch(Exception e) {
			ErrorDialog.Exception(e);
		}
	}

	public void Remove(IGameObject Object) {
		Remove(Object, false);
	}

	public void Remove(IGameObject Object, bool NoUndo) {
		Command command = null;
		Remove(Object, NoUndo, ref command);
		if (!NoUndo)
			UndoManager.AddCommand(command);
	}

	//use this one if you want to handle resulting undo command yourself
	public void Remove(IGameObject Object, ref Command command) {
		Remove(Object, false, ref command);
	}

	private void Remove(IGameObject Object, bool NoUndo, ref Command command) {
		if (!NoUndo) {
			command = new ObjectRemoveCommand(
				"Delete Object " + Object,
				Object,
				this);
		}
		GameObjects.Remove(Object);
		try {
			if(ObjectRemoved != null)
				ObjectRemoved(this, Object);
		} catch(Exception e) {
			ErrorDialog.Exception(e);
		}
	}

	public bool Contains(IGameObject Object) {
		return GameObjects.Contains(Object);
	}

	public void EmitSizeChanged()
	{
		FinishRead();		//update Height / Width
		if(SizeChanged != null)
			SizeChanged(this);
	}

	public void CustomLispRead(Properties Props) {
		if (SortObjectTags)
		{
			foreach(Type type in this.GetType().Assembly.GetTypes()) {
				SupertuxObjectAttribute objectAttribute
					= (SupertuxObjectAttribute) Attribute.GetCustomAttribute(type, typeof(SupertuxObjectAttribute));
				if(objectAttribute == null)
					continue;

				LispSerializer serializer = new LispSerializer(type);
				foreach(List list in Props.GetList(objectAttribute.Name)) {
					try {
						IGameObject Object = (IGameObject) serializer.Read(list);
						GameObjects.Add(Object);
					} catch (System.NullReferenceException) {
						if (type == typeof(MusicObject) || type == typeof(AmbientLightObject)) {
							// ignore errors here due to the given fields being
							// turned from properties to objects
						} else {
							Console.WriteLine("Unexpected error while parsing object: {0} {1}", type, list);
							throw;
						}
					}
				}
			}
		}
		else
		{
			// FIXME: this is all just a hack to make the
			// editor not reorder the elements on load,
			// could be done nicer.
			foreach(List list in Props.GetList()) {
				foreach(Type type in this.GetType().Assembly.GetTypes()) {
					SupertuxObjectAttribute objectAttribute
						= (SupertuxObjectAttribute) Attribute.GetCustomAttribute(type, typeof(SupertuxObjectAttribute));
					if(objectAttribute == null)
						continue;

					if (objectAttribute.Name == (list[0] as Symbol).Name) {
						LispSerializer serializer = new LispSerializer(type);
						IGameObject Object = (IGameObject) serializer.Read(list);
						GameObjects.Add(Object);
						break;
					}
				}
			}
		}
	}

	public void CustomLispWrite(Writer Writer) {
		if (SortObjectTags)
		{
			var attributes = new List<Tuple<Type, SupertuxObjectAttribute>>();

			foreach(Type type in GetType().Assembly.GetTypes()) {
				var attr = (SupertuxObjectAttribute)Attribute.GetCustomAttribute(type, typeof(SupertuxObjectAttribute));
				if (attr != null) {
					attributes.Add(Tuple.Create(type, attr));
				}
			}

			attributes.Sort((lhs, rhs) => lhs.Item2.Name.CompareTo(rhs.Item2.Name));

			foreach(var attr in attributes) {
				LispSerializer serializer = new LispSerializer(attr.Item1);
				foreach(var obj in GetObjects(attr.Item1)) {
					serializer.Write(Writer, attr.Item2.Name, obj);
				}
			}
		}
		else
		{
			foreach(object obj in GetObjects()) {
				Type type = obj.GetType();

				SupertuxObjectAttribute objectAttribute = (SupertuxObjectAttribute)
					Attribute.GetCustomAttribute(type, typeof(SupertuxObjectAttribute));
				if(objectAttribute == null)
					continue;

				string name = objectAttribute.Name;
				LispSerializer serializer = new LispSerializer(type);
				serializer.Write(Writer, name, obj);
			}
		}
	}

	public void FinishRead() {
		width = 0;
		height = 0;
		foreach(Tilemap tmap in this.GetObjects(typeof(Tilemap))) {
			if(tmap.Width > width)
				width = tmap.Width;
			if(tmap.Height > height)
				height = tmap.Height;
			tmap.UpdatePos();
		}

		if (!String.IsNullOrEmpty(Music)) {
			GameObjects.Add(new MusicObject(Music));
			Music = String.Empty;
		}

		if (!(GameObjects.Exists(x => x is AmbientLightObject))) {
			GameObjects.Add(new AmbientLightObject(AmbientLight));
			AmbientLight = new Drawing.Color( 1f, 1f, 1f );
		}

		List<IGameObject> new_gameobjects = new List<IGameObject>();
		foreach(IGameObject obj in this.GetObjects()) {
			if (obj is IPathObject) {
				IPathObject path_obj = (IPathObject)obj;
				if (path_obj is PathGameObject) {
					// don't touch
				} else if (path_obj.Path != null) {
					PathGameObject new_path_gameobject = new PathGameObject();

					new_path_gameobject.Path = path_obj.Path;
					path_obj.Path = null;
					path_obj.PathRef = new_path_gameobject.EntityName;

					new_gameobjects.Add(new_path_gameobject);
				}
			}
		}
		GameObjects.AddRange(new_gameobjects);
	}
}

/* EOF */
