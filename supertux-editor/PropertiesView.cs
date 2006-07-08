using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Gtk;
using LispReader;


/// <summary>
/// Used to set a custom tooltip for a LispChild.
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property,
								AllowMultiple = false)]
public sealed class CustomTooltipAttribute : Attribute {
	public string Tooltip;

	public CustomTooltipAttribute(string tooltip) {
		this.Tooltip = tooltip;
	}
}

public class PropertiesView : ScrolledWindow
{
	private System.Object Object;
	private Dictionary<string, FieldOrProperty> fieldTable = new Dictionary<string, FieldOrProperty>();
	private Label errorLabel;
	internal Tooltips tooltips;
	
	private object CreateObject(Type Type) {
		// create object
		ConstructorInfo Constructor = Type.GetConstructor(Type.EmptyTypes);
		if(Constructor == null)
			throw new Exception("Type '" + Type + "' has no public constructor without arguments");
		object Result = Constructor.Invoke(new object[] {});

		return Result;
	}
	
	public void SetObject(object Object, string title)
	{
		this.Object = Object;
		try {
			CreatePropertyWidgets(title);
		} catch(Exception e) {
			ErrorDialog.Exception(e);
		}
	}
	
	private void CreatePropertyWidgets(string title)
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
		Type type = Object.GetType();
		fieldTable.Clear();
		List<Widget> editWidgets = new List<Widget>();
		foreach(FieldOrProperty field in FieldOrProperty.GetFieldsAndProperties(type)) {
			CustomSettingsWidgetAttribute customSettings = (CustomSettingsWidgetAttribute)
				field.GetCustomAttribute(typeof(CustomSettingsWidgetAttribute));
			if(customSettings != null) {
				Type customType = customSettings.Type;
				ICustomSettingsWidget customWidget = (ICustomSettingsWidget) CreateObject(customType);
				customWidget.Object = Object;
				customWidget.Field = field;
				editWidgets.Add(customWidget.Create(this));
				continue;				
			}
			
			LispChildAttribute ChildAttrib = (LispChildAttribute)
				field.GetCustomAttribute(typeof(LispChildAttribute));
			if(ChildAttrib == null)
				continue;

			CustomTooltipAttribute customTooltip = (CustomTooltipAttribute)
				field.GetCustomAttribute(typeof(CustomTooltipAttribute));

			if(field.Type == typeof(string) || field.Type == typeof(float)
				|| field.Type == typeof(int)) {
				Entry entry = new Entry();
				entry.Name = field.Name;
				object val = field.GetValue(Object);
				if(val != null)
					entry.Text = val.ToString();
				fieldTable[field.Name] = field;
				entry.Changed += OnEntryChanged;
				editWidgets.Add(entry);
				AddTooltip(customTooltip, entry);
			} else if(field.Type == typeof(bool)) {
				CheckButton checkButton = new CheckButton(field.Name);
				checkButton.Name = field.Name;
				checkButton.Active = (bool) field.GetValue(Object);
				fieldTable[field.Name] = field;
				checkButton.Toggled += OnCheckButtonToggled;
				editWidgets.Add(checkButton);
				AddTooltip(customTooltip, checkButton);
			} else if(field.Type.IsEnum) {
				// Create a combobox containing all the names of enum values.
				ComboBox comboBox = new ComboBox(Enum.GetNames(field.Type));
				// Set the name of the box.
				comboBox.Name = field.Name;
				// FIXME: This will break if:
				//        1) the first enum isn't 0 and/or 
				//        2) the vaules are not sequential (0, 1, 3, 4 wouldn't work)
				object val = field.GetValue(Object);
				if (val != null)
					comboBox.Active = (int)val;
				fieldTable[field.Name] = field;
				comboBox.Changed += OnComboBoxChanged;
				editWidgets.Add(comboBox);
				// FIXME: Why doesn't this work for the ComboBox?
				AddTooltip(customTooltip, comboBox);
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
		errorLabel = new Label("");
		errorLabel.Xalign = 0;
		errorLabel.Xpad = 12;
		box.PackStart(errorLabel, true, false, 0);
		
		box.ShowAll();
		
		Foreach(Remove);
		AddWithViewport(box);
	}

	/// <summary>
	/// Add a tooltip for a widget if a CustomTooltipAttribute is set.
	/// </summary>
	/// <param name="customTooltip">Attribute with tooltip</param>
	/// <param name="widget">Widget to add tooltip to.</param>
	private void AddTooltip(CustomTooltipAttribute customTooltip, Widget widget) {
		if (customTooltip != null)
			tooltips.SetTip(widget, customTooltip.Tooltip, customTooltip.Tooltip);
	}
	
	private void OnEntryChanged(object o, EventArgs args)
	{
		try {
			Entry entry = (Entry) o;
			FieldOrProperty field = fieldTable[entry.Name];
			if(field.Type == typeof(string)) {
				field.SetValue(Object, entry.Text);
			} else if(field.Type == typeof(float)) {
				float parsed = Single.Parse(entry.Text);
				if(parsed.ToString() != entry.Text)
					entry.Text = parsed.ToString();
				field.SetValue(Object, parsed);
			} else if(field.Type == typeof(int)) {
				int parsed = Int32.Parse(entry.Text);
				if(parsed.ToString() != entry.Text)
					entry.Text = parsed.ToString();
				field.SetValue(Object, parsed);
			} else {
				throw new Exception("Not implemented yet");
			}
		} catch(FormatException fe) {
			errorLabel.Text = fe.Message;
			return;
		} catch(Exception e) {
			ErrorDialog.Exception(e);
			return;
		}
		errorLabel.Text = "";
	}
	
	private void OnCheckButtonToggled(object o, EventArgs args)
	{
		try {
			CheckButton checkButton = (CheckButton) o;
			FieldOrProperty field = fieldTable[checkButton.Name];
			field.SetValue(Object, checkButton.Active);
		} catch(Exception e) {
			ErrorDialog.Exception(e);
		}			
	}

	private void OnComboBoxChanged(object o, EventArgs args) {
		try {
			ComboBox comboBox = (ComboBox)o;
			FieldOrProperty field = fieldTable[comboBox.Name];
			// Parse the string back to enum.
			field.SetValue(Object, Enum.Parse(field.Type, comboBox.ActiveText));
		} catch (Exception e) {
			ErrorDialog.Exception(e);
		}
	}	
}
