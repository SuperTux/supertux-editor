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
using System.Reflection;
using Gtk;
using LispReader;
using Undo;

public sealed class ChooseResourceWidget : CustomSettingsWidget {
	private Entry entry;

	public override Widget Create(object caller) {
		HBox box = new HBox();
		entry = new Entry();
		OnFieldChanged(Field); //same code for initialization

		entry.FocusOutEvent += OnEntryChangeDone;
		box.PackStart(entry, true, true, 0);

		Button chooseButton = new Button("...");
		box.PackStart(chooseButton, false, false, 0);
		chooseButton.Clicked += OnChoose;

		box.Name = Field.Name;

		CreateToolTip(caller, entry);

		return box;
	}

	private void OnChoose(object o, EventArgs args) {
		FileChooserDialog dialog = new FileChooserDialog("Choose resource", null, FileChooserAction.Open, new object[] { });
		dialog.AddButton(Gtk.Stock.Cancel, Gtk.ResponseType.Cancel);
		dialog.AddButton(Gtk.Stock.Open, Gtk.ResponseType.Ok);
		dialog.DefaultResponse = Gtk.ResponseType.Ok;

		dialog.Action = FileChooserAction.Open;
		dialog.SetFilename(Settings.Instance.SupertuxData + entry.Text);
		int result = dialog.Run();
		if (result != (int) ResponseType.Ok) {
			dialog.Destroy();
			return;
		}
		string path;
		if (dialog.Filename.StartsWith(Settings.Instance.SupertuxData))
			path = dialog.Filename.Substring(Settings.Instance.SupertuxData.Length,
																						 dialog.Filename.Length - Settings.Instance.SupertuxData.Length);
		else
			path = System.IO.Path.GetFileName(dialog.Filename);
		// Fixes backslashes on windows:
		entry.Text = path.Replace("\\", "/");
		dialog.Destroy();

		OnEntryChangeDone(entry, null);
	}

	private void OnEntryChangeDone(object o, FocusOutEventArgs args) {
		try {
			Entry entry = (Entry) o;
			if ((string)Field.GetValue(Object) == entry.Text) return;
			PropertyChangeCommand command = new PropertyChangeCommand(
				"Changed value of " + Field.Name,
				Field,
				Object,
				entry.Text);
			command.Do();
			UndoManager.AddCommand(command);
		} catch (Exception e) {
			ErrorDialog.Exception(e);
			string val = (string) Field.GetValue(Object);
			if (val != null)
				entry.Text = val;
		}
	}

	/// <summary> Called when our data changes, use this for re-loading. </summary>
	protected override void OnFieldChanged(FieldOrProperty field) {
		string val = (string) Field.GetValue(Object);
		if (val != null)
			entry.Text = val;
		else
			entry.Text = "";
	}
}

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property,
                AllowMultiple=false)]
public sealed class ChooseResourceSetting : CustomSettingsWidgetAttribute
{
	public ChooseResourceSetting() : base(typeof(ChooseResourceWidget))
	{
	}
}

/* EOF */
