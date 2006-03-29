using System;
using System.Reflection;
using Gtk;

public interface ICustomSettingsWidget
{
	FieldInfo Field {
		get;
		set;
	}
	object Object {
		get;
		set;
	}
	
	Widget Create();
}
