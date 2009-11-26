//  SuperTux Editor
//  Copyright (C) 2006 Matthias Braun <matze@braunis.de>
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using LispReader;
using Undo;

/// <summary>
/// Used to set a custom tooltip for a LispChild.
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property,
                AllowMultiple = false)]
public sealed class PropertyPropertiesAttribute : Attribute {
	public string Tooltip = String.Empty;

	/// <summary>
	/// If true this property is hidden from the <see cref="PropertiesView"/>.
	/// </summary>
	public bool Hidden = false;

	/// <summary>
	/// If true and this property changes, it causes redraw in <see cref="SectorRenderer"/>.
	/// </summary>
	public bool RedrawOnChange = false;

	public PropertyPropertiesAttribute() {
	}
}

public class PropertiesView : Gtk.ScrolledWindow
{
	private List<Gtk.Widget> editWidgets = new List<Gtk.Widget>();	//All widgets that edit properties
	private List<ICustomSettingsWidget> customWidgets = new List<ICustomSettingsWidget>();//All custom widgets
	internal IToolApplication application;
	private System.Object Object;
	//HACK: No bi-directional dictionary found... - it' simple: matching items have same ID.
	private List<object> widgetTable = new List<object>();	//self-managed widgets ...
	private List<FieldOrProperty> fieldTable = new List<FieldOrProperty>();	//... and fields that they edit
	private Gtk.Label errorLabel;
	internal Gtk.Tooltips tooltips;
	private Gtk.Label titleLabel;

	public PropertiesView(IToolApplication application) {
		this.application = application;
	}

	private static object CreateObject(Type Type) {
		// create object
		ConstructorInfo Constructor = Type.GetConstructor(Type.EmptyTypes);
		if(Constructor == null)
			throw new Exception("Type '" + Type + "' has no public constructor without arguments");
		object Result = Constructor.Invoke(new object[] {});

		return Result;
	}

	public void SetObject(object NewObject, string title)
	{
		try {
			if (Object != null && NewObject != null && Object.GetType() == NewObject.GetType()) {
				foreach (ICustomSettingsWidget wg in customWidgets)	//notify all custom widgets
					wg.ChangeObject (NewObject);
				this.Object = NewObject;
				foreach (FieldOrProperty field in fieldTable)		//and also all self-managed widgets
					OnFieldChanged(NewObject, field, null);
					titleLabel.Markup = "<b>" + title + "</b>";
			} else {
				CreatePropertyWidgets(title, NewObject);
				this.Object = NewObject;
			}
		} catch(Exception e) {
			ErrorDialog.Exception(e);
		}
	}

	private void CreatePropertyWidgets(string title, object NewObject)
	{
		Gtk.VBox box = new Gtk.VBox();
		tooltips = new Gtk.Tooltips();

		titleLabel = new Gtk.Label();
		titleLabel.Xalign = 0;
		titleLabel.Xpad = 12;
		titleLabel.Ypad = 6;
		titleLabel.Markup = "<b>" + title + "</b>";
		box.PackStart(titleLabel, true, false, 0);

		Type type = NewObject.GetType();

		// Dispose all former custom editor widgets
		foreach(IDisposable disposable in customWidgets) {
			disposable.Dispose();
		}

		// Unregister our event handler from self-managed fields
		foreach(FieldOrProperty field in fieldTable) {
			field.Changed -= OnFieldChanged;
		}

		widgetTable.Clear();
		fieldTable.Clear();
		editWidgets.Clear();
		customWidgets.Clear();

		// iterate over all fields and properties
		foreach(FieldOrProperty field in FieldOrProperty.GetFieldsAndProperties(type)) {
			CustomSettingsWidgetAttribute customSettings = (CustomSettingsWidgetAttribute)
				field.GetCustomAttribute(typeof(CustomSettingsWidgetAttribute));
			if(customSettings != null) {
				Type customType = customSettings.Type;
				ICustomSettingsWidget customWidget = (ICustomSettingsWidget) CreateObject(customType);
				customWidgets.Add(customWidget);
				editWidgets.Add(customWidget.Create(this, NewObject, field));
				continue;
			}

			LispChildAttribute ChildAttrib = (LispChildAttribute)
				field.GetCustomAttribute(typeof(LispChildAttribute));
			if(ChildAttrib == null)
				continue;

			PropertyPropertiesAttribute propertyProperties = (PropertyPropertiesAttribute)
				field.GetCustomAttribute(typeof(PropertyPropertiesAttribute));

			if ((propertyProperties != null) && (propertyProperties.Hidden))
				continue;

			if(field.Type == typeof(string) || field.Type == typeof(float)
				|| field.Type == typeof(int)) {
				Gtk.Entry entry = new Gtk.Entry();
				entry.Name = field.Name;
				object val = field.GetValue(NewObject);
				if(val != null)
					entry.Text = val.ToString();
				widgetTable.Add(entry);
				fieldTable.Add(field);
				entry.Changed += OnEntryChanged;
				entry.FocusOutEvent += OnEntryChangeDone;
				editWidgets.Add(entry);
				AddTooltip(propertyProperties, entry);
			} else if(field.Type == typeof(bool)) {
				Gtk.CheckButton checkButton = new Gtk.CheckButton(field.Name);
				checkButton.Name = field.Name;
				checkButton.Active = (bool) field.GetValue(NewObject);
				widgetTable.Add(checkButton);
				fieldTable.Add(field);
				checkButton.Toggled += OnCheckButtonToggled;
				editWidgets.Add(checkButton);
				AddTooltip(propertyProperties, checkButton);
			} else if(field.Type.IsEnum) {
				// Create a combobox containing all the names of enum values.
				Gtk.ComboBox comboBox = new Gtk.ComboBox(Enum.GetNames(field.Type));
				// Set the name of the box.
				comboBox.Name = field.Name;
				// FIXME: This will break if:
				//        1) the first enum isn't 0 and/or
				//        2) the vaules are not sequential (0, 1, 3, 4 wouldn't work)
				object val = field.GetValue(NewObject);
				if (val != null)
					comboBox.Active = (int)val;
				widgetTable.Add(comboBox);
				fieldTable.Add(field);
				comboBox.Changed += OnComboBoxChanged;
				editWidgets.Add(comboBox);
				AddTooltip(propertyProperties, comboBox);
			}

		}

		// Register our event handler for self-managed fields
		foreach(FieldOrProperty field in fieldTable) {
			field.Changed += OnFieldChanged;
		}

		Gtk.Table table = new Gtk.Table((uint) editWidgets.Count, 2, false);
		table.ColumnSpacing = 6;
		table.RowSpacing = 6;
		table.BorderWidth = 12;
		for(uint i = 0; i < editWidgets.Count; ++i) {
			Gtk.Widget widget = editWidgets[(int) i];
			if(widget is Gtk.CheckButton) {
				table.Attach(widget, 0, 2, i, i+1,
				             Gtk.AttachOptions.Fill | Gtk.AttachOptions.Expand, Gtk.AttachOptions.Shrink, 0, 0);
			} else {
				Gtk.Label label = new Gtk.Label(widget.Name + ":");
				label.SetAlignment(0, 1);
				table.Attach(label, 0, 1, i, i+1,
				             Gtk.AttachOptions.Fill, Gtk.AttachOptions.Shrink, 0, 0);
				table.Attach(widget, 1, 2, i, i+1,
				             Gtk.AttachOptions.Fill | Gtk.AttachOptions.Expand, Gtk.AttachOptions.Shrink, 0, 0);
			}
		}
		box.PackStart(table, true, true, 0);

		// TODO add a (!) image in front of the label (and hide/show it depending
		// if there was an error)
		errorLabel = new Gtk.Label(String.Empty);
		errorLabel.Xalign = 0;
		errorLabel.Xpad = 12;
		box.PackStart(errorLabel, true, false, 0);

		box.ShowAll();

		Foreach(Remove);
		AddWithViewport(box);
	}

	/// <summary>
	/// Add a tooltip for a widget if a PropertyPropertiesAttribute is set.
	/// </summary>
	/// <param name="propertyProperties">Attribute with tooltip</param>
	/// <param name="widget">Widget to add tooltip to.</param>
	private void AddTooltip(PropertyPropertiesAttribute propertyProperties, Gtk.Widget widget) {
		if ((propertyProperties != null) && (propertyProperties.Tooltip.Length != 0))
			tooltips.SetTip(widget, propertyProperties.Tooltip, propertyProperties.Tooltip);
	}

	private void OnEntryChangeDone(object o, Gtk.FocusOutEventArgs args)
	{
		try {
			Gtk.Entry entry = (Gtk.Entry) o;
			FieldOrProperty field = fieldTable[widgetTable.IndexOf(entry)];
			PropertyChangeCommand command;
			if(field.Type == typeof(string)) {
				if ((string)field.GetValue(Object) == entry.Text) return;
				command = new PropertyChangeCommand(
					"Changed value of " + field.Name,
					field,
					Object,
					entry.Text);
			} else if(field.Type == typeof(float)) {
				float parsed = Single.Parse(entry.Text);
				if(parsed.ToString() != entry.Text && parsed.ToString() + "." != entry.Text )
					entry.Text = parsed.ToString();
				if ((float)field.GetValue(Object) == parsed) return;
				command = new PropertyChangeCommand(
					"Changed value of " + field.Name,
					field,
					Object,
					parsed);
			} else if(field.Type == typeof(int)) {
				int parsed = Int32.Parse(entry.Text);
				if(parsed.ToString() != entry.Text)
					entry.Text = parsed.ToString();
				if ((int)field.GetValue(Object) == parsed) return;
				command = new PropertyChangeCommand(
					"Changed value of " + field.Name,
					field,
					Object,
					parsed);
			} else {
				throw new ApplicationException(
					"PropertiesView.OnEntryChangeDone, \""  + field.Type.FullName + "\" is not implemented yet. " +
					"If you are a developer, please fix it, else report this full error message and what you did to cause it to the supertux developers.");
			}
			command.Do();
			UndoManager.AddCommand(command);
		} catch(FormatException fe) {
			errorLabel.Text = fe.Message;
			return;
		} catch(Exception e) {
			ErrorDialog.Exception(e);
			return;
		}
		errorLabel.Text = String.Empty;
	}

	private void OnEntryChanged(object o, EventArgs args)
	{
		try {
			Gtk.Entry entry = (Gtk.Entry) o;
			FieldOrProperty field = fieldTable[widgetTable.IndexOf(entry)];
			if(field.Type == typeof(string)) {
				return;
			} else if(field.Type == typeof(float)) {
				float parsed = Single.Parse(entry.Text);
				if(parsed.ToString() != entry.Text && parsed.ToString() + "." != entry.Text )
					entry.Text = parsed.ToString();
			} else if(field.Type == typeof(int)) {
				int parsed = Int32.Parse(entry.Text);
				if(parsed.ToString() != entry.Text)
					entry.Text = parsed.ToString();
			} else {
				throw new ApplicationException(
					"PropertiesView.OnEntryChanged, \""  + field.Type.FullName + "\" is not implemented yet. " +
					"If you are a developer, please fix it, else report this full error message and what you did to cause it to the supertux developers.");
			}
		} catch(FormatException fe) {
			errorLabel.Text = fe.Message;
			return;
		} catch(Exception e) {
			ErrorDialog.Exception(e);
			return;
		}
		errorLabel.Text = String.Empty;
	}

	private void OnCheckButtonToggled(object o, EventArgs args)
	{
		try {
			Gtk.CheckButton checkButton = (Gtk.CheckButton) o;
			FieldOrProperty field = fieldTable[widgetTable.IndexOf(checkButton)];
			bool oldValue = (bool) field.GetValue(Object);
			bool newValue = checkButton.Active;
			if (oldValue != newValue) {	//no change => no Undo action
				PropertyChangeCommand command = new PropertyChangeCommand(
					"Changed value of " + field.Name,
					field,
					Object,
					newValue);
				command.Do();
				UndoManager.AddCommand(command);
			}
		} catch(Exception e) {
			ErrorDialog.Exception(e);
		}
	}

	private void OnComboBoxChanged(object o, EventArgs args) {
		try {
			Gtk.ComboBox comboBox = (Gtk.ComboBox)o;
			FieldOrProperty field = fieldTable[widgetTable.IndexOf(comboBox)];
			// Parse the string back to enum.
			Enum oldValue = (Enum) field.GetValue(Object);
			Enum newValue = (Enum) Enum.Parse(field.Type, comboBox.ActiveText);
			if (!oldValue.Equals(newValue)) {	//no change => no Undo action
				PropertyChangeCommand command = new PropertyChangeCommand(
					"Changed value of " + field.Name,
					field,
					Object,
					newValue);
				command.Do();
				UndoManager.AddCommand(command);
			}
		} catch (Exception e) {
			ErrorDialog.Exception(e);
		}
	}

	/// <summary> Called when our field changes on any instance of same type as our Object. </summary>
	private void OnFieldChanged(object Object, FieldOrProperty field, object oldValue) {
		if (this.Object == Object) {
			if(field.Type == typeof(string) || field.Type == typeof(float)
				|| field.Type == typeof(int)) {

				Gtk.Entry entry = (Gtk.Entry) widgetTable[fieldTable.IndexOf(field)];
				object val = field.GetValue(Object);
				if(val != null)
					entry.Text = val.ToString();

			} else if(field.Type == typeof(bool)) {

				Gtk.CheckButton checkButton = (Gtk.CheckButton) widgetTable[fieldTable.IndexOf(field)];
				checkButton.Active = (bool) field.GetValue(Object);

			} else if(field.Type.IsEnum) {

				Gtk.ComboBox comboBox = (Gtk.ComboBox) widgetTable[fieldTable.IndexOf(field)];
				// FIXME: This will break if:
				//        1) the first enum isn't 0 and/or
				//        2) the vaules are not sequential (0, 1, 3, 4 wouldn't work)
				object val = field.GetValue(Object);
				if (val != null)
					comboBox.Active = (int)val;
			}
		}
	}
}

/* EOF */
