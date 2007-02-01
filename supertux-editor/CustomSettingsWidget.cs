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
