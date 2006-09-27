using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Gtk;
using LispReader;

public sealed class ChooseSectorWidget : CustomSettingsWidget {
	private ComboBox comboBox;

	public override Widget Create(object caller) {
		HBox box = new HBox();

		PropertiesView propview = (PropertiesView)caller;

		// Add the names of the sectors to a list
		List<String> sectorNames = new List<String>(propview.application.CurrentLevel.Sectors.Count);
		foreach(Sector sector in propview.application.CurrentLevel.Sectors) {
			sectorNames.Add(sector.Name);
		}

		// Populate a combo box with the sector names
		comboBox = new ComboBox(sectorNames.ToArray());

		// Get the index of the current value from the original list
		// due to limitations in ComboBox
		string val = (string)field.GetValue(Object);
		if (val != null)
			comboBox.Active = sectorNames.IndexOf(val);

		comboBox.Changed += OnComboBoxChanged;
		box.PackStart(comboBox, true, true, 0);

		box.Name = field.Name;

		CreateToolTip(caller, comboBox);

		return box;
	}

	private void OnComboBoxChanged(object o, EventArgs args) {
		try {
			ComboBox comboBox = (ComboBox)o;
			field.SetValue(_object, comboBox.ActiveText);
		} catch (Exception e) {
			ErrorDialog.Exception(e);
		}
	}
}

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property,
								AllowMultiple = false)]
public sealed class ChooseSectorSettingAttribute : CustomSettingsWidgetAttribute {
	public ChooseSectorSettingAttribute()
		: base(typeof(ChooseSectorWidget)) {
	}
}
