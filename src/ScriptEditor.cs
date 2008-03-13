//  $Id$
using System;
using Gtk;
using Gdk;
using Glade;
using LispReader;

/// <summary>
/// A widget used for editing scripts
/// </summary>
public class TextEditor : ScrolledWindow
{
	private TextView textView;

	private FieldOrProperty field;
	private object object_;

	public TextEditor()
	{
		HscrollbarPolicy = PolicyType.Automatic;
		VscrollbarPolicy = PolicyType.Always;
		textView = new TextView();
		Add(textView);

		textView.Backspace += OnChange;
		textView.CutClipboard += OnChange;
		textView.DeleteFromCursor += OnChange;
		textView.InsertAtCursor += OnChange;
		textView.PasteClipboard += OnChange;
	}

	public void SetTarget(FieldOrProperty field, object object_)
	{
		this.field = field;
		this.object_ = object_;
		object val = field.GetValue(object_);
		textView.Buffer.Text = val != null ? val.ToString() : String.Empty;
	}

	private void OnChange(object o, EventArgs e)
	{
		field.SetValue(object_, textView.Buffer.Text);
	}
}
