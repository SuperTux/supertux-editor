//  $Id$
using System;
using Gtk;
using Gdk;
using Glade;
using LispReader;

/// <summary>
/// A dialogbox used to edit scripts and currently also other multi line strings.
/// </summary>
/// <remarks>
/// If you change this to add syntax highlighting or other
/// features that are specialised for Squirrel please create another
/// multi-line edit box that can be used in for
/// example <see cref="InfoBlock"/>.
/// </remarks>
public class ScriptEditor
{
	[Glade.Widget]
	private Dialog scriptDialog = null;
	[Glade.Widget]
	private TextView scriptEditor = null;

	private FieldOrProperty field;
	private object object_;

	public ScriptEditor(string title, FieldOrProperty field, object object_)
	{
		this.field = field;
		this.object_ = object_;

		Glade.XML gxml = new Glade.XML("editor.glade", "scriptDialog");
		gxml.Autoconnect(this);

		if(scriptDialog == null || scriptEditor == null)
			throw new Exception("Couldn't load ScriptEditor dialog");

		/*
		window = new Window("Supertux - ScriptEditor - " + title);
		window.SetSizeRequest(640, 500);
		*/

		object val = field.GetValue(object_);
		scriptEditor.Buffer.Text = val != null ? val.ToString() : String.Empty;

		scriptDialog.ShowAll();
	}

	protected void OnOk(object o, EventArgs args)
	{
		try {
			field.SetValue(object_, scriptEditor.Buffer.Text);
		} catch(Exception e) {
			ErrorDialog.Exception(e);
		}

		scriptDialog.Hide();
	}

	protected void OnCancel(object o, EventArgs args)
	{
		scriptDialog.Hide();
	}

}
