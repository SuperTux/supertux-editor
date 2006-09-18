using System;
using System.IO;
using System.Reflection;
using Gtk;
using LispReader;

public sealed class ChooseResourceWidget : CustomSettingsWidget
{
	private Entry entry;

	public override Widget Create(object caller)
	{
		HBox box = new HBox();
		entry = new Entry();
		string val = (string) field.GetValue(Object);
		if(val != null)
			entry.Text = val;

		entry.Changed += OnEntryChanged;
		box.PackStart(entry, true, true, 0);

		Button chooseButton = new Button("...");
		box.PackStart(chooseButton, false, false, 0);
		chooseButton.Clicked += OnChoose;

		box.Name = field.Name;

		CreateToolTip(caller, entry);

		return box;
	}

	private void OnChoose(object o, EventArgs args)
	{
		FileChooserDialog dialog = new FileChooserDialog("Choose resource", null, FileChooserAction.Open, new object[] {});
		dialog.AddButton(Gtk.Stock.Cancel, Gtk.ResponseType.Cancel);
		dialog.AddButton(Gtk.Stock.Open, Gtk.ResponseType.Ok);
		dialog.DefaultResponse = Gtk.ResponseType.Ok;

		dialog.Action = FileChooserAction.Open;
		dialog.SetFilename(Settings.Instance.SupertuxData + entry.Text);
		int result = dialog.Run();
		if(result != (int) ResponseType.Ok) {
			dialog.Destroy();
			return;
		}

		if(dialog.Filename.StartsWith(Settings.Instance.SupertuxData))
			entry.Text = dialog.Filename.Substring(Settings.Instance.SupertuxData.Length,
			                                       dialog.Filename.Length - Settings.Instance.SupertuxData.Length);
		else
			entry.Text = System.IO.Path.GetFileName(dialog.Filename);
		// Fixes backslashes on windows:
		entry.Text = entry.Text.Replace("\\", "/");
		dialog.Destroy();
	}

	private void OnEntryChanged(object o, EventArgs arg)
	{
		try {
			Entry entry = (Entry) o;
			field.SetValue(_object, entry.Text);
		} catch(Exception e) {
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
