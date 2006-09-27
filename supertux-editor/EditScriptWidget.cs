//  $Id$
using System;
using LispReader;
using Gtk;

/// <summary>
/// A widget used to open a dialog box to
/// edit scripts in (and currently also other multi line strings).
/// </summary>
public sealed class EditScriptWidget : CustomSettingsWidget
{
	public override Widget Create(object caller)
	{
		Button button = new Button("Edit Script");
		button.Clicked += OnEdit;
		button.Name = field.Name;

		// Create a tooltip if we can.
		CreateToolTip(caller, button);
		return button;
	}

	private void OnEdit(object sender, EventArgs args)
	{
		new ScriptEditor(_object.GetType() + ":" + field.Name, field, Object);
	}
}

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property,
                AllowMultiple=false)]
public sealed class EditScriptSetting : CustomSettingsWidgetAttribute
{
	public EditScriptSetting() : base(typeof(EditScriptWidget))
	{
	}
}
