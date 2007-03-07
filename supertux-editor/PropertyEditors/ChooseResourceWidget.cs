//  $Id$
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
		string val = (string) field.GetValue(Object);
		if (val != null)
			entry.Text = val;

		entry.FocusOutEvent += OnEntryChangeDone;
		box.PackStart(entry, true, true, 0);

		Button chooseButton = new Button("...");
		box.PackStart(chooseButton, false, false, 0);
		chooseButton.Clicked += OnChoose;

		box.Name = field.Name;

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
		PropertyChangeCommand command = new PropertyChangeCommand(
			"Changed value of " + field.Name,
			field,
			_object,
			entry.Text);
		command.Do();
		UndoManager.AddCommand(command);
		dialog.Destroy();
	}

	private void OnEntryChangeDone(object o, FocusOutEventArgs args) {
		try {
			Entry entry = (Entry) o;
			if ((string)field.GetValue(_object) == entry.Text) return;
			PropertyChangeCommand command = new PropertyChangeCommand(
				"Changed value of " + field.Name,
				field,
				_object,
				entry.Text);
			command.Do();
			UndoManager.AddCommand(command);
		} catch (Exception e) {
			ErrorDialog.Exception(e);
		}
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
