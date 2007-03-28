//  $Id$
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Gtk;
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
	/// If true this object is hidden from the <see cref="PropertiesView"/>.
	/// </summary>
	public bool Hidden = false;

	public PropertyPropertiesAttribute() {
	}
}

public class PropertiesView : ScrolledWindow
{
	internal IEditorApplication application;
	private System.Object Object;
	private Dictionary<string, FieldOrProperty> fieldTable = new Dictionary<string, FieldOrProperty>();
	private Label errorLabel;
	internal Tooltips tooltips;

	public PropertiesView(IEditorApplication application) {
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
			CreatePropertyWidgets(title, NewObject);
			this.Object = NewObject;
		} catch(Exception e) {
			ErrorDialog.Exception(e);
		}
	}

	private void CreatePropertyWidgets(string title, object NewObject)
	{
		VBox box = new VBox();
		tooltips = new Tooltips();

		Label titleLabel = new Label();
		titleLabel.Xalign = 0;
		titleLabel.Xpad = 12;
		titleLabel.Ypad = 6;
		titleLabel.Markup = "<b>" + title + "</b>";
		box.PackStart(titleLabel, true, false, 0);

		// iterate over all fields and properties
		Type type = NewObject.GetType();
		fieldTable.Clear();
		List<Widget> editWidgets = new List<Widget>();
		foreach(FieldOrProperty field in FieldOrProperty.GetFieldsAndProperties(type)) {
			CustomSettingsWidgetAttribute customSettings = (CustomSettingsWidgetAttribute)
				field.GetCustomAttribute(typeof(CustomSettingsWidgetAttribute));
			if(customSettings != null) {
				Type customType = customSettings.Type;
				ICustomSettingsWidget customWidget = (ICustomSettingsWidget) CreateObject(customType);
				customWidget.Object = NewObject;
				customWidget.Field = field;
				editWidgets.Add(customWidget.Create(this));
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
				Entry entry = new Entry();
				entry.Name = field.Name;
				object val = field.GetValue(NewObject);
				if(val != null)
					entry.Text = val.ToString();
				fieldTable[field.Name] = field;
				entry.Changed += OnEntryChanged;
				entry.FocusOutEvent += OnEntryChangeDone;
				editWidgets.Add(entry);
				AddTooltip(propertyProperties, entry);
			} else if(field.Type == typeof(bool)) {
				CheckButton checkButton = new CheckButton(field.Name);
				checkButton.Name = field.Name;
				checkButton.Active = (bool) field.GetValue(NewObject);
				fieldTable[field.Name] = field;
				checkButton.Toggled += OnCheckButtonToggled;
				editWidgets.Add(checkButton);
				AddTooltip(propertyProperties, checkButton);
			} else if(field.Type.IsEnum) {
				// Create a combobox containing all the names of enum values.
				ComboBox comboBox = new ComboBox(Enum.GetNames(field.Type));
				// Set the name of the box.
				comboBox.Name = field.Name;
				// FIXME: This will break if:
				//        1) the first enum isn't 0 and/or
				//        2) the vaules are not sequential (0, 1, 3, 4 wouldn't work)
				object val = field.GetValue(NewObject);
				if (val != null)
					comboBox.Active = (int)val;
				fieldTable[field.Name] = field;
				comboBox.Changed += OnComboBoxChanged;
				editWidgets.Add(comboBox);
				// FIXME: Why doesn't this work for the ComboBox?
				AddTooltip(propertyProperties, comboBox);
			}

		}

		Table table = new Table((uint) editWidgets.Count, 2, false);
		table.ColumnSpacing = 6;
		table.RowSpacing = 6;
		table.BorderWidth = 12;
		for(uint i = 0; i < editWidgets.Count; ++i) {
			Widget widget = editWidgets[(int) i];
			if(widget is CheckButton) {
				table.Attach(widget, 0, 2, i, i+1,
				             AttachOptions.Fill | AttachOptions.Expand, AttachOptions.Shrink, 0, 0);
			} else {
				Label label = new Label(widget.Name + ":");
				label.SetAlignment(0, 1);
				table.Attach(label, 0, 1, i, i+1,
				             AttachOptions.Fill, AttachOptions.Shrink, 0, 0);
				table.Attach(widget, 1, 2, i, i+1,
				             AttachOptions.Fill | AttachOptions.Expand, AttachOptions.Shrink, 0, 0);
			}
		}
		box.PackStart(table, true, true, 0);

		// TODO add a (!) image in front of the label (and hide/show it depending
		// if there was an error)
		errorLabel = new Label(String.Empty);
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
	private void AddTooltip(PropertyPropertiesAttribute propertyProperties, Widget widget) {
		if ((propertyProperties != null) && (propertyProperties.Tooltip.Length != 0))
			tooltips.SetTip(widget, propertyProperties.Tooltip, propertyProperties.Tooltip);
	}

	private void OnEntryChangeDone(object o, FocusOutEventArgs args)
	{
		try {
			Entry entry = (Entry) o;
			FieldOrProperty field = fieldTable[entry.Name];
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
			Entry entry = (Entry) o;
			FieldOrProperty field = fieldTable[entry.Name];
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
			CheckButton checkButton = (CheckButton) o;
			FieldOrProperty field = fieldTable[checkButton.Name];
			PropertyChangeCommand command = new PropertyChangeCommand(
				"Changed value of " + field.Name,
				field,
				Object,
				checkButton.Active);
			command.Do();
			UndoManager.AddCommand(command);
		} catch(Exception e) {
			ErrorDialog.Exception(e);
		}
	}

	private void OnComboBoxChanged(object o, EventArgs args) {
		try {
			ComboBox comboBox = (ComboBox)o;
			FieldOrProperty field = fieldTable[comboBox.Name];
			// Parse the string back to enum.
			PropertyChangeCommand command = new PropertyChangeCommand(
				"Changed value of " + field.Name,
				field,
				Object,
				Enum.Parse(field.Type, comboBox.ActiveText));
			command.Do();
			UndoManager.AddCommand(command);
		} catch (Exception e) {
			ErrorDialog.Exception(e);
		}
	}
}
