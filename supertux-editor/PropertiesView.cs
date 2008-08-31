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
	private List<Widget> editWidgets = new List<Widget>();	//All widgets that edit properties
	private List<ICustomSettingsWidget> customWidgets = new List<ICustomSettingsWidget>();//All custom widgets
	internal IEditorApplication application;
	private System.Object Object;
	//HACK: No bi-directional dictionary found... - it' simple: matching items have same ID.
	private List<object> widgetTable = new List<object>();	//self-managed widgets ...
	private List<FieldOrProperty> fieldTable = new List<FieldOrProperty>();	//... and fields that they edit
	private Label errorLabel;
	internal Tooltips tooltips;
	private Label titleLabel;

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
		VBox box = new VBox();
		tooltips = new Tooltips();

		titleLabel = new Label();
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
				Entry entry = new Entry();
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
				CheckButton checkButton = new CheckButton(field.Name);
				checkButton.Name = field.Name;
				checkButton.Active = (bool) field.GetValue(NewObject);
				widgetTable.Add(checkButton);
				fieldTable.Add(field);
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
			Entry entry = (Entry) o;
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
			CheckButton checkButton = (CheckButton) o;
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
			ComboBox comboBox = (ComboBox)o;
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

				Entry entry = (Entry) widgetTable[fieldTable.IndexOf(field)];
				object val = field.GetValue(Object);
				if(val != null)
					entry.Text = val.ToString();

			} else if(field.Type == typeof(bool)) {

				CheckButton checkButton = (CheckButton) widgetTable[fieldTable.IndexOf(field)];
				checkButton.Active = (bool) field.GetValue(Object);

			} else if(field.Type.IsEnum) {

				ComboBox comboBox = (ComboBox) widgetTable[fieldTable.IndexOf(field)];
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
