using System;
using System.Reflection;
using Gtk;
using LispReader;

public interface ICustomSettingsWidget
{
	FieldOrProperty Field {
		get;
		set;
	}
	object Object {
		get;
		set;
	}
	
	Widget Create();
}
