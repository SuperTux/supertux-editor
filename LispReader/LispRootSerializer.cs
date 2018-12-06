//  $Id$
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Lisp;

namespace LispReader
{

	/// <summary>Serializes and Deserializes an object based on reflection.</summary>
	/// <remarks>If someone is looking for a challenge: Speed could be improved heavily
	/// by creating CIL at runtime...</remarks>
	public class LispRootSerializer : ILispSerializer
	{
		private Type type;

		public LispRootSerializer(Type type)
		{
			this.type = type;
		}

		private void CheckRequired(LispChildAttribute ChildAttrib)
		{
			if(!ChildAttrib.Optional)
				LogManager.Log(LogLevel.Debug, "Required field '" + type.Name + "." + ChildAttrib.Name + "' was not declared in loded file.");
		}

		public object Read(List list)
		{
			object result = CreateObject(type);

			Properties props = new Properties(list);
			// iterate over all fields and properties
			foreach(FieldOrProperty field in FieldOrProperty.GetFieldsAndProperties(type)) {
				LispChildAttribute ChildAttrib = (LispChildAttribute)
					field.GetCustomAttribute(typeof(LispChildAttribute));
				if(ChildAttrib != null) {
					string Name = ChildAttrib.Name;
					string altName = ChildAttrib.AlternativeName;
					if(field.Type == typeof(int)) {
						int val = 0;
						if(!(props.Get(Name, ref val) || (altName != null && props.Get(altName, ref val)))) {
							CheckRequired(ChildAttrib);
						} else {
							field.SetValue(result, val);
						}
					} else if(field.Type == typeof(string)) {
						string val = null;
						if(!props.Get(Name, ref val)) {
							CheckRequired(ChildAttrib);
						} else {
							field.SetValue(result, val);
						}
					} else if(field.Type == typeof(float)) {
						float val = 0;
						if(!(props.Get(Name, ref val) || (altName != null && props.Get(altName, ref val)))) {
							CheckRequired(ChildAttrib);
						} else {
							field.SetValue(result, val);
						}
					} else if (field.Type.IsEnum) {
						Enum val = null;
						if (!props.Get(Name, ref val, field.Type)) {
							CheckRequired(ChildAttrib);
						} else {
							field.SetValue(result, val);
						}
					} else if(field.Type == typeof(bool)) {
						bool val = false;
						if(!props.Get(Name, ref val)) {
							CheckRequired(ChildAttrib);
						} else {
							field.SetValue(result, val);
						}
					} else if(field.Type == typeof(List<int>)) {
						List<int> val = new List<int>();
						if(!props.GetIntList(Name, val)) {
							CheckRequired(ChildAttrib);
						} else {
							field.SetValue(result, val);
						}
					} else if(field.Type == typeof(List<string>)) {
						List<string> val = new List<string>();
						if(!props.GetStringList(Name, val)) {
							CheckRequired(ChildAttrib);
						} else {
							field.SetValue(result, val);
						}
					} else if(field.Type == typeof(Lisp.List)) {
						Lisp.List val;
						if(!props.GetLispList(Name, out val)) {
							CheckRequired(ChildAttrib);
						} else {
							field.SetValue(result, val);
						}
					} else {
						ILispSerializer serializer = LispSerializer.GetSerializer(field.Type);
						if(serializer == null)
							throw new LispException("Type " + field.Type + " not supported for serialization");

						List val = null;
						if(!props.Get(Name, ref val)) {
							CheckRequired(ChildAttrib);
						} else {
							try {
								object oval = serializer.Read(val);
								field.SetValue(result, oval);
							} catch(LispException) {
								if (Name == "ambient-light" || Name == "music") {
									// ignore errors of these for backward compatibilty
								} else {
									throw;
								}
							}
						}
					}
				}

				foreach(LispChildsAttribute childsAttrib in
						field.GetCustomAttributes(typeof(LispChildsAttribute))) {
					object objectList = field.GetValue(result);
					Type ListType = field.Type;
					MethodInfo AddMethod = ListType.GetMethod(
							"Add", new Type[] { childsAttrib.ListType }, null);
					if(AddMethod == null)
						throw new LispException("No Add method found for field " + field.Name);

					ILispSerializer serializer = LispSerializer.GetSerializer(childsAttrib.Type);
					if(serializer == null)
						serializer = LispSerializer.CreateRootSerializer(childsAttrib.Type);

					foreach(List childList in props.GetList(childsAttrib.Name)) {
						object child = serializer.Read(childList);
						AddMethod.Invoke(objectList, new object[] { child } );
					}
				}
			}

			if(result is ICustomLispSerializer) {
				ICustomLispSerializer custom = (ICustomLispSerializer) result;
				custom.CustomLispRead(props);
				custom.FinishRead();
			}

			return result;
		}

		public void Write(Writer writer, string name, object Object)
		{
			writer.StartList(name);

			foreach(FieldOrProperty field in FieldOrProperty.GetFieldsAndProperties(type)) {
				LispChildAttribute ChildAttrib = (LispChildAttribute)
					field.GetCustomAttribute(typeof(LispChildAttribute));
				if(ChildAttrib != null && !ChildAttrib.Transient) {
					object Value = field.GetValue(Object);
					if(Value != null) {
						if(ChildAttrib.Translatable) {
							if(!ChildAttrib.Optional || !Value.Equals(ChildAttrib.Default))
								writer.WriteTranslatable(ChildAttrib.Name, Value.ToString());
						} else {
							Type childType = field.Type;

							ILispSerializer serializer = LispSerializer.GetSerializer(childType);
							if(serializer != null) {
								if(!ChildAttrib.Optional || !Value.Equals(ChildAttrib.Default)) {
									serializer.Write(writer, ChildAttrib.Name, Value);
								}
							} else {
								if(ChildAttrib.Optional && childType.IsEnum) {
									// If it is an enum we need to convert ChildAttrib.Default
									// to an enum as ChildAttrib.Default is an Int32 by some (unknown) reason.
									Enum Defval = (Enum)Enum.ToObject(childType, ChildAttrib.Default);
									if (!Value.Equals(Defval))
										writer.Write(ChildAttrib.Name, Value);
								} else if(!ChildAttrib.Optional || !Value.Equals(ChildAttrib.Default)) {
									writer.Write(ChildAttrib.Name, Value);
								}
							}
						}
					} else {
						LogManager.Log(LogLevel.Debug, "Field '{0}' of type '{1}' is null", field.Name, name);
					}
				}

				foreach(LispChildsAttribute childsAttrib in
						field.GetCustomAttributes(typeof(LispChildsAttribute))) {
					if(childsAttrib != null) {
						object list = field.GetValue(Object);
						if(! (list is IEnumerable))
							throw new LispException("Field '" + field.Name + "' is not IEnumerable");

						ILispSerializer serializer = LispSerializer.GetSerializer(childsAttrib.Type);
						if(serializer == null)
							serializer = LispSerializer.CreateRootSerializer(childsAttrib.Type);

						IEnumerable enumerable = (IEnumerable) list;
						foreach(object childObject in enumerable) {
							if(childsAttrib.Type.IsAssignableFrom(childObject.GetType())) {
								serializer.Write(writer, childsAttrib.Name, childObject);
							}
						}
					}
				}
			}

			if(Object is ICustomLispSerializer) {
				ICustomLispSerializer custom = (ICustomLispSerializer) Object;
				custom.CustomLispWrite(writer);
			}

			writer.EndList(name);
		}

		private static object CreateObject(Type Type)
		{
			// create object
			ConstructorInfo Constructor = Type.GetConstructor(Type.EmptyTypes);
			if(Constructor == null)
				throw new LispException("Type '" + Type + "' has no public constructor without arguments");
			object Result = Constructor.Invoke(new object[] {});

			return Result;
		}
	}

}
