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

	public abstract Widget Create(object caller);

	protected void CreateToolTip(object caller, Widget widget) {
		// Create a tooltip if we can.
		PropertyPropertiesAttribute propertyProperties = (PropertyPropertiesAttribute)
			field.GetCustomAttribute(typeof(PropertyPropertiesAttribute));
		if ((propertyProperties != null) && (caller.GetType() == typeof(PropertiesView))) {
			PropertiesView propview = (PropertiesView)caller;
			propview.tooltips.SetTip(widget, propertyProperties.Tooltip, propertyProperties.Tooltip);
		}
	}
}