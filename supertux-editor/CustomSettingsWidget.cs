using System;
using Drawing;
using Gtk;
using Gdk;
using LispReader;

/// <summary>
/// Base class for custom settings widgets.
/// </summary>
public abstract class CustomSettingsWidget : ICustomSettingsWidget {
	public FieldOrProperty field;
	public FieldOrProperty Field {
		get {
			return field;
		}
		set {
			field = value;
		}
	}

	public object _object;
	public object Object {
		get {
			return _object;
		}
		set {
			_object = value;
		}
	}

	public abstract Widget Create();

}