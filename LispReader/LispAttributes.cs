//  $Id$
using System;

namespace LispReader
{

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct,
	                AllowMultiple=false)]
	public sealed class LispRootAttribute : Attribute
	{
		public string Name;

		public LispRootAttribute(string name)
		{
			this.Name = name;
		}
	}

	/// <summary>
	///		Marks a class or struct as a serializer
	///		for <see cref="LispCustomSerializerAttribute.Type"/>
	/// </summary>
	/// <remarks>
	///		The class marked with this must implement
	///		<see cref="Lisp.ILispSerializer"/>.
	/// </remarks>
	/// <seealso cref="Lisp.ILispSerializer"/>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct,
	                AllowMultiple=false)]
	public sealed class LispCustomSerializerAttribute : Attribute
	{
		public Type Type;

		public LispCustomSerializerAttribute(Type type)
		{
			this.Type = type;
		}
	}

	/// <summary>
	///		Maps a field or property in a class to a lisp construct.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property,
	                AllowMultiple=false)]
	public sealed class LispChildAttribute : Attribute
	{
		public bool Translatable;
		public bool Optional;
		public object Default;
		public string Name;

		public LispChildAttribute(string Name) {
			this.Name = Name;
		}
	}

	// *NOTE*: This is guesswork, please confirm that it is correct.
	// Marks a field or property as being a list(?) that can contain
	// classes marked with LispRootAttribute? (See Level.Sectors and
	// Sector for example.)
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property,
	                AllowMultiple=true)]
	public sealed class LispChildsAttribute : Attribute
	{
		public string Name;
		public Type Type;
		private Type _listType;

		public Type ListType {
			set {
				_listType = value;
			}
			get {
				if(_listType == null)
					return Type;
				else
					return _listType;
			}
		}
	}

}
