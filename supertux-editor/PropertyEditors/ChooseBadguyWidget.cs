//  $Id$
//
//  Copyright (C) 2008 Milos Kloucek <MMlosh@supertuxdev.elektromaniak.cz>
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
using System.Collections.Generic;
using Undo;

/// <summary>
/// Badguys editing widget for Dispensers.
/// </summary>
public sealed class ChooseBadguyWidget : CustomSettingsWidget
{
	private Entry entry;
	private bool changed = false;

	public override Widget Create(object caller)
	{
		VBox vBox = new VBox(false, 3);

		List<string> val = (List<string>) field.GetValue(Object);

		entry = new Entry();
		HBox hBox = new HBox(true, 3);

		string String = "";
		for (int i = 0; i < val.Count; i++) {
			String += val[i];
			if (i < val.Count -1) 
				String += ", ";
		}
		entry.Text = String; 

		entry.Changed += OnBadguyChanged;
		entry.FocusOutEvent += OnBadguyChangeDone;

		vBox.PackEnd(entry, true, true, 0);
		vBox.PackEnd(hBox, true, true, 0);

		// Create a tooltip if we can.
		CreateToolTip(caller, vBox);

		vBox.Name = field.Name;

		return vBox;
	}

	private void OnBadguyChanged(object sender, EventArgs args)
	{
		changed = true;
	}

	private void OnBadguyChangeDone(object sender, EventArgs args)
	{
		if (!changed) return; 
		changed = false;
		List<string> bad = new List<string>();
		string String1 = entry.Text;
		string String2 = "";

		while (String1.IndexOf(", ")>-1) {	//at least two not yet added items
			String2 = String1.Substring(0, String1.IndexOf(", "));
			String1 = String1.Substring(String1.IndexOf(", ")+2, String1.Length-String1.IndexOf(", ")-2);
			bad.Add(String2);
		}

		if (String1.Length>0){
			bad.Add(String1);

		}

		PropertyChangeCommand command = new PropertyChangeCommand(
			"Changed " + field.Name,
			field,
			Object,
			bad);
		command.Do();
		UndoManager.AddCommand(command);
	}
}

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property,
                AllowMultiple=false)]
public sealed class ChooseBadguySettingAttribute : CustomSettingsWidgetAttribute
{
	public ChooseBadguySettingAttribute() : base(typeof(ChooseBadguyWidget))
	{
	}
}
