using System;
using System.Collections;
using System.Reflection;
using System.IO;
using Lisp;

/**
 * This class allows serialization of .net Classes to/from lisp constructs.
 * You just have to annotate the class to specify a mapping from fields/lists to
 * lisp structures.
 *
 * The design is similar to System.Xml.Serialization.XmlSerializer
 */
public class LispSerializer {
	private Type RootType;
	
	public LispSerializer(Type RootType) {
		this.RootType = RootType;
	}

	public void Write(string FileName, object Object) {
		TextWriter Writer = new StreamWriter(FileName);
		Write(Writer, FileName, Object);
		Writer.Close();
	}

	public void Write(TextWriter TextWriter, string Dest, object Object) {
		Writer Writer = new Writer(TextWriter);
		
		LispRootAttribute RootAttrib = (LispRootAttribute)
			Attribute.GetCustomAttribute(RootType, typeof(LispRootAttribute));
		if(RootAttrib == null)
			throw new Exception("Type needs to have LispRoot attribute");


		Writer.StartList(RootAttrib.Name);
		Write(Writer, Object);
		Writer.EndList(RootAttrib.Name);
	}

	public void Write(Writer Writer, object Object) {
		WriteType(Writer, RootType, Object);
	}

	public object Read(string FileName) {
		TextReader Reader = new StreamReader(FileName);
		return Read(Reader, FileName);
	}

	public object Read(TextReader Reader, string Source) {
		LispRootAttribute RootAttrib = (LispRootAttribute)
			Attribute.GetCustomAttribute(RootType, typeof(LispRootAttribute));
		if(RootAttrib == null)
			throw new Exception("Type needs to have LispRoot attribute");

		Lexer Lexer = new Lexer(Reader);
		Parser Parser = new Parser(Lexer);

		List Root = Parser.Parse();
		Properties RootP = new Properties(Root);

		List List = null;
		if(!RootP.Get(RootAttrib.Name, ref List))
			throw new Exception("'" + Source + "' is not a " + RootAttrib.Name + " file");
		
		return ReadType(RootType, List);
	}

	public object Read(List List) {
		return ReadType(RootType, List);
	}

	private object ReadType(Type type, List List) {
		object Result = CreateObject(type);

		Properties Props = new Properties(List);
		
		// iterate over all fields and properties
		FieldInfo[] fields = type.GetFields();		
		foreach(FieldInfo field in fields) {
			LispChildAttribute ChildAttrib = (LispChildAttribute)
				Attribute.GetCustomAttribute(field, typeof(LispChildAttribute));
			if(ChildAttrib != null) {
				string Name = ChildAttrib.Name;
				if(field.FieldType == typeof(int)) {
					int val = 0;
					if(!Props.Get(Name, ref val))
						Console.WriteLine("Field '" + Name + "' not in lisp");
					else
						field.SetValue(Result, val);
				} else if(field.FieldType == typeof(string)) {
					string val = null;
					if(!Props.Get(Name, ref val))
						Console.WriteLine("Field '" + Name + "' not in lisp");
					else
						field.SetValue(Result, val);
				} else if(field.FieldType == typeof(float)) {
					float val = 0;
					if(!Props.Get(Name, ref val))
						Console.WriteLine("Field '" + Name + "' not in lisp");
					else
						field.SetValue(Result, val);
				} else if(field.FieldType == typeof(bool)) {
					bool val = false;
					if(!Props.Get(Name, ref val))
						Console.WriteLine("Field '" + Name + "' not in lisp");
					else
						field.SetValue(Result, val);
				} else {
					throw new Exception("Type " + field.FieldType + " not supported for LispChild yet");
				}
			}
			
			foreach(LispChildsAttribute ChildsAttrib in
					Attribute.GetCustomAttributes(field, typeof(LispChildsAttribute))) {
				if(ChildsAttrib != null) {
					object list = field.GetValue(Result);
					Type ListType = field.FieldType;
					MethodInfo AddMethod = ListType.GetMethod(
							"Add", new Type[] { ChildsAttrib.ListType }, null);
					if(AddMethod == null)
						throw new Exception("No Add method found for field " + field.Name);

					foreach(List ChildList in Props.GetList(ChildsAttrib.Name)) {
						object child = ReadType(ChildsAttrib.Type, ChildList);
						AddMethod.Invoke(list, new object[] { child } );
					}
				}
			}			
		}
		
		PropertyInfo[] properties = type.GetProperties();
		foreach(PropertyInfo property in properties) {
			if(!property.CanWrite)
				continue;
			
			LispChildAttribute ChildAttrib = (LispChildAttribute)
				Attribute.GetCustomAttribute(property, typeof(LispChildAttribute));
			if(ChildAttrib != null) {
				string Name = ChildAttrib.Name;
				Type ptype = property.PropertyType;
				if(ptype == typeof(int)) {
					int val = 0;
					if(!Props.Get(Name, ref val)) {
						Console.WriteLine("Field '" + Name + "' not in lisp");
					} else {
						property.SetValue(Result, val, null);
					}
				} else if(ptype == typeof(string)) {
					string val = null;
					if(!Props.Get(Name, ref val)) {
						Console.WriteLine("Field '" + Name + "' not in lisp");
					} else {
						property.SetValue(Result, val, null);
					}
				} else if(ptype == typeof(float)) {
					float val = 0;
					if(!Props.Get(Name, ref val)) {
						Console.WriteLine("Field '" + Name + "' not in lisp");
					} else {
						property.SetValue(Result, val, null);
					}
				} else if(ptype == typeof(bool)) {
					bool val = false;
					if(!Props.Get(Name, ref val)) {
						Console.WriteLine("Field '" + Name + "' not in lisp");
					} else {
						property.SetValue(Result, val, null);
					}
					break;
				} else {
					throw new Exception("Type " + ptype + " not supported for LispChild yet");
				}
			}			
		}

		if(Result is ICustomLispSerializer) {
			ICustomLispSerializer Custom = (ICustomLispSerializer) Result;
			Custom.CustomLispRead(Props);
			Custom.FinishRead();
		}

		return Result;
	}

	private void WriteType(Writer Writer, Type Type, object Object) {
		FieldInfo[] fields = Type.GetFields();
		
		foreach(FieldInfo field in fields) {
			LispChildAttribute ChildAttrib = (LispChildAttribute)
				Attribute.GetCustomAttribute(field, typeof(LispChildAttribute));
			if(ChildAttrib != null) {
				object Value = field.GetValue(Object);
				if(Value != null) {
					if(ChildAttrib.Translatable) {
						Writer.WriteTranslatable(ChildAttrib.Name, Value.ToString());
					} else {
						Writer.Write(ChildAttrib.Name, Value);
					}
				} else {
					Console.WriteLine("Warning: Field '" + field.Name + "' is null");
				}
			}

			foreach(LispChildsAttribute ChildsAttrib in
					Attribute.GetCustomAttributes(field,
												  typeof(LispChildsAttribute))) {
				if(ChildsAttrib != null) {
					object list = field.GetValue(Object);
					if(! (list is IEnumerable))
						throw new Exception("Field '" + field.Name + "' is not IEnumerable");
					
					IEnumerable enumerable = (IEnumerable) list;

					foreach(object ChildObject in enumerable) {
						if(ChildsAttrib.Type.IsAssignableFrom(
									ChildObject.GetType())) {
							Writer.StartList(ChildsAttrib.Name);
							WriteType(Writer, ChildsAttrib.Type, ChildObject);
							Writer.EndList(ChildsAttrib.Name);
						}
					}
				}
			}
		}
			
		if(Object is ICustomLispSerializer) {
			ICustomLispSerializer Custom = (ICustomLispSerializer) Object;
			Custom.CustomLispWrite(Writer);
		}
	}

	private object CreateObject(Type Type) {
		// create object
		ConstructorInfo Constructor = Type.GetConstructor(Type.EmptyTypes);
		if(Constructor == null)
			throw new Exception("Type '" + Type + "' has no public constructor without arguments");
		object Result = Constructor.Invoke(new object[] {});

		return Result;
	}
}

