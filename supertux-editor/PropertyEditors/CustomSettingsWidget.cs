//  $Id$
//
//  Copyright (C) 2007 Arvid Norlander <anmaster AT berlios DOT de>
//
//  This program is free software; you can redistribute it and/or
//  modify it under the terms of the GNU General Public License
//  as published by the Free Software Foundation; either version 2
//  of the License, or (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA
//  02111-1307, USA.
using System;
using Drawing;
using Gtk;
using Gdk;
using LispReader;

/// <summary>
/// Base class for custom settings widgets.
/// </summary>
public abstract class CustomSettingsWidget : ICustomSettingsWidget, IDisposable {
	private FieldOrProperty field;
	public FieldOrProperty Field {
		get {
			return field;
		}
		set {
			if (field != null)
				field.Changed -= OnAnyFieldChanged;
			field = value;
			field.Changed += OnAnyFieldChanged;
		}
	}

	private object _object;
	public object Object {
		get {
			return _object;
		}
		set {
			_object = value;
		}
	}

	public abstract Widget Create(object caller);
	public Widget Create(object caller, object Object, FieldOrProperty field){
		this.Field = field;
		this.Object = Object;
		return Create(caller);
	}

	public virtual void ChangeObject (object Object)
	{
		this.Object = Object;
		OnFieldChanged(Field);
	}

	public virtual void Dispose() {
		field.Changed -= OnAnyFieldChanged;
	}

	/// <summary> Called when our field changes on any instance of same type as our Object. </summary>
	private void OnAnyFieldChanged(object Object, FieldOrProperty field, object oldValue) {
		if (this.Object == Object)
			OnFieldChanged(field);
	}

	/// <summary> Called when our data changes, use this for re-loading. </summary>
	protected virtual void OnFieldChanged(FieldOrProperty field) {}


	protected void CreateToolTip(object caller, Widget widget) {
		CreateToolTip(caller, widget, Field);
	}

	/// <summary> Static member accesible for other ICustomSettingsWidgets. </summary>
	public static void CreateToolTip(object caller, Widget widget, FieldOrProperty field) {
		// Create a tooltip if we can.
		PropertyPropertiesAttribute propertyProperties = (PropertyPropertiesAttribute)
			field.GetCustomAttribute(typeof(PropertyPropertiesAttribute));
		if ((propertyProperties != null) && (caller.GetType() == typeof(PropertiesView))) {
			PropertiesView propview = (PropertiesView)caller;
			propview.tooltips.SetTip(widget, propertyProperties.Tooltip, propertyProperties.Tooltip);
		}
	}
}
