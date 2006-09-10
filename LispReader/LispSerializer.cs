using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using Lisp;

namespace LispReader
{

	/// <summary>This class allows serialization of .net Classes to/from lisp constructs.</summary>
	/// <remarks>
	/// <para>You just have to annotate the class to specify a mapping from fields/lists to
	/// lisp structures.</para>
	/// <para>The design is similar to <see cref="System.Xml.Serialization.XmlSerializer"/></para>
	/// </remarks>
	public class LispSerializer {
		private Type RootType;
		private static Dictionary<Type, ILispSerializer> typeSerializers
			= new Dictionary<Type, ILispSerializer>();
		
		static LispSerializer()
		{
			SetupSerializers(typeof(LispSerializer).Assembly);
		}
		
		public LispSerializer(Type RootType)
		{
			this.RootType = RootType;
		}

		public void Write(string FileName, object Object)
		{
			TextWriter Writer = new StreamWriter(FileName);
			try {
				Write(Writer, FileName, Object);
			} finally {
				Writer.Close();
			}
		}

		public void Write(TextWriter TextWriter, string Dest, object Object)
		{
			LispRootAttribute rootAttrib = (LispRootAttribute)
				Attribute.GetCustomAttribute(RootType, typeof(LispRootAttribute));
			if(rootAttrib == null)
				throw new LispException("Type needs to have LispRoot attribute");

			Writer Writer = new Writer(TextWriter);
			Write(Writer, rootAttrib.Name, Object);
		}

		public void Write(Writer Writer, string name, object Object)
		{
			WriteType(Writer, RootType, name, Object);
		}

		public object Read(string FileName)
		{
			TextReader Reader = new StreamReader(FileName);
			try {
				return Read(Reader, FileName);
			} finally {
				Reader.Close();
			}
		}

		public object Read(TextReader Reader, string Source)
		{
			LispRootAttribute RootAttrib = (LispRootAttribute)
				Attribute.GetCustomAttribute(RootType, typeof(LispRootAttribute));
			if(RootAttrib == null)
				throw new LispException("Type needs to have LispRoot attribute");

			Lexer Lexer = new Lexer(Reader);
			Parser Parser = new Parser(Lexer);

			List Root = Parser.Parse();
			Properties RootP = new Properties(Root);

			List List = null;
			if(!RootP.Get(RootAttrib.Name, ref List))
				throw new LispException("'" + Source + "' is not a " + RootAttrib.Name + " file");
			
			return ReadType(RootType, List);
		}

		public object Read(List List)
		{
			return ReadType(RootType, List);
		}

		private static object ReadType(Type type, List list)
		{
			ILispSerializer serializer = GetSerializer(type);
			if(serializer == null)
				serializer = CreateRootSerializer(type);
			
			return serializer.Read(list);
		}		

		private static void WriteType(Writer writer, Type type, string name, object Object)
		{
			ILispSerializer serializer = GetSerializer(type);
			if(serializer == null)
				serializer = CreateRootSerializer(type);
			
			serializer.Write(writer, name, Object);
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
		
		public static ILispSerializer GetSerializer(Type type)
		{
			ILispSerializer result;
			typeSerializers.TryGetValue(type, out result);
			return result;
		}
		
		public static void SetupSerializers(Assembly assembly)
		{
			foreach(Type type in assembly.GetTypes()) {
				ScanType(type);
			}
		}
		
		public static void ScanType(Type type)
		{
			foreach(Type nestedType in type.GetNestedTypes())
				ScanType(nestedType);
			
			LispCustomSerializerAttribute customSerializer =
			 	(LispCustomSerializerAttribute)
			 	Attribute.GetCustomAttribute(type, typeof(LispCustomSerializerAttribute));
			if(customSerializer != null) {
				object instance = CreateObject(type);
				typeSerializers.Add(customSerializer.Type, (ILispSerializer) instance);
				return;
			}
			
			LispRootAttribute rootAttrib = (LispRootAttribute)
				Attribute.GetCustomAttribute(type, typeof(LispRootAttribute));
			if(rootAttrib != null) {
				LispRootSerializer serializer = new LispRootSerializer(type);
				typeSerializers.Add(type, serializer);
				return;
			}
		}
		
		internal static ILispSerializer CreateRootSerializer(Type type)
		{
			LispRootSerializer serializer = new LispRootSerializer(type);
			typeSerializers.Add(type, serializer);
			
			return serializer;
		}
	}

}