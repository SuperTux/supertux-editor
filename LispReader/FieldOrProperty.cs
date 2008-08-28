//  $Id$
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace LispReader
{
	public delegate void PropertyChangedHandler(object Object, FieldOrProperty field);

	/// <summary>
	/// Oreginal base class (MemberInfo) can't Get/SetValue and this class allows it.
	/// One code can work with FieldInfo or PropertyInfo as if there was no difference between them. + can raise event when Changed
	/// It's a kind of hack, however, because we can't "rebase" a class. 
	///</summary>
	public abstract class FieldOrProperty
	{
		private static Dictionary<FieldInfo, FieldOrProperty> fields = new Dictionary<FieldInfo, FieldOrProperty>();
		private static Dictionary<PropertyInfo, FieldOrProperty> properties = new Dictionary<PropertyInfo, FieldOrProperty>();

		private class FieldOrPropertyLister : IEnumerable<FieldOrProperty>
		{
			private Type type;

			public FieldOrPropertyLister(Type type)
			{
				this.type = type;
			}

			private IEnumerator<FieldOrProperty> GetEnumerator()
			{
				foreach(FieldInfo field in type.GetFields()) {
					yield return Lookup(field);
				}

				foreach(PropertyInfo property in type.GetProperties()) {
					yield return Lookup(property);
				}
			}

			IEnumerator<FieldOrProperty> IEnumerable<FieldOrProperty>.GetEnumerator()
			{
				return GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}

		public event PropertyChangedHandler Changed;

		public static IEnumerable<FieldOrProperty> GetFieldsAndProperties(Type type)
		{
			return new FieldOrPropertyLister(type);
		}

		public abstract string Name {
			get;
		}
		public abstract Type Type {
			get;
		}
		public abstract void SetValue(object Object, object Value);
		public abstract object GetValue(object Object);
		public abstract object GetCustomAttribute(Type attributeType);
		public abstract object[] GetCustomAttributes(Type attributeType);

		public static FieldOrProperty Lookup(FieldInfo field) {
			if (!fields.ContainsKey(field))
				fields.Add(field, new Field(field));
			return fields[field];
		}

		public static FieldOrProperty Lookup(PropertyInfo property) {
			if (!properties.ContainsKey(property))
				properties.Add(property, new Property(property));
			return properties[property];
		}

		protected void FireChanged(object Object, FieldOrProperty field){
			if (Changed != null)
				Changed(Object, field);
		}

		/// <summary> Code uses this to notify editors when only part of object changes (but it's adress not). </summary>
		public void FireChanged(object Object) {
			FireChanged(Object, this);
		}

		private class Field : FieldOrProperty{
			private FieldInfo field;

			public Field(FieldInfo field)
			{
				this.field = field;
			}

			public override string Name {
				get {
					return field.Name;
				}
			}

			public override Type Type {
				get {
					return field.FieldType;
				}
			}

			public override void SetValue(object Object, object value)
			{
				object oldValue = field.GetValue(Object);
				field.SetValue(Object, value);
				if (oldValue != value)
					FireChanged(Object, this);
			}

			public override object GetValue(object Object)
			{
				return field.GetValue(Object);
			}

			public override object GetCustomAttribute(Type attributeType)
			{
				return Attribute.GetCustomAttribute(field, attributeType);
			}

			public override object[] GetCustomAttributes(Type attributeType)
			{
				return Attribute.GetCustomAttributes(field, attributeType);
			}
		}

		private class Property : FieldOrProperty{
			private PropertyInfo property;

			public Property(PropertyInfo property)
			{
				this.property = property;
			}

			public override string Name {
				get {
					return property.Name;
				}
			}

			public override Type Type {
				get {
					return property.PropertyType;
				}
			}

			public override void SetValue(object Object, object value)
			{
				object oldValue = property.GetValue(Object, null);
				property.SetValue(Object, value, null);
				if (oldValue != value)
					FireChanged(Object, this);
			}

			public override object GetValue(object Object)
			{
				return property.GetValue(Object, null);
			}

			public override object GetCustomAttribute(Type attributeType)
			{
				return Attribute.GetCustomAttribute(property, attributeType);
			}

			public override object[] GetCustomAttributes(Type attributeType)
			{
				return Attribute.GetCustomAttributes(property, attributeType);
			}
		}

	}

}
