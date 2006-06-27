using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace LispReader
{

	public abstract class FieldOrProperty
	{
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
					yield return new Field(field);
				}
				
				foreach(PropertyInfo property in type.GetProperties()) {
					yield return new Property(property);
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
				field.SetValue(Object, value);
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
				property.SetValue(Object, value, null);
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
