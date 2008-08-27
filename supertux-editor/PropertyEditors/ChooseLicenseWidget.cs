//  $Id$
//
//  Copyright (C) 2007 Christoph Sommer <christoph.sommer@2007.expires.deltadevelopment.de>
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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Gtk;
using LispReader;
using Undo;

public sealed class ChooseLicenseWidget : CustomSettingsWidget {
	private ComboBoxEntry comboBox;

	public override Widget Create(object caller) {

		List<String> licenseTemplateTexts = new List<String>();
		licenseTemplateTexts.Add("non-redistributable (forbid sharing and modification of this level)");
		licenseTemplateTexts.Add("GPL 2+ / CC-by-sa 3.0 (allow sharing and modification of this level)");

		comboBox = new ComboBoxEntry(licenseTemplateTexts.ToArray());

		// set current value
		string val = (string)Field.GetValue(Object);
		comboBox.Entry.Text = val;

		comboBox.Changed += OnComboBoxChanged;

		HBox box = new HBox();
		box.PackStart(comboBox, true, true, 0);
		box.Name = Field.Name;

		CreateToolTip(caller, comboBox);

		return box;
	}

	private void OnComboBoxChanged(object o, EventArgs args) {
		try {
			ComboBoxEntry comboBox = (ComboBoxEntry)o;
			string s = comboBox.ActiveText;

			// strip off comments from licenseTemplateTexts
			if (s == "non-redistributable (forbid sharing and modification of this level)") s = s.Substring(0, s.IndexOf(" ("));
			if (s == "GPL 2+ / CC-by-sa 3.0 (allow sharing and modification of this level)") s = s.Substring(0, s.IndexOf(" ("));

			PropertyChangeCommand command = new PropertyChangeCommand(
				"Changed value of " + Field.Name,
				Field,
				Object,
				s);
			command.Do();
			UndoManager.AddCommand(command);
		} catch (Exception e) {
			ErrorDialog.Exception(e);
		}
	}
}

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
public sealed class ChooseLicenseSettingAttribute : CustomSettingsWidgetAttribute {
	public ChooseLicenseSettingAttribute() : base(typeof(ChooseLicenseWidget)) {
	}
}
