//  $Id$
using System;
using Gtk;
using Gdk;
using Glade;
using LispReader;
using Undo;

/// <summary>
/// A dialogbox used to edit scripts and currently also other multi line strings.
/// </summary>
/// <remarks>
/// If you change this to add syntax highlighting or other
/// features that are specialized for Squirrel please create another
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

		scriptDialog.Title = title + "  -  " + scriptDialog.Title;
		OnFieldChanged(object_, field, null);	//same code as for initialization
		field.Changed += OnFieldChanged;

		scriptDialog.ShowAll();
	}

	protected void OnOk(object o, EventArgs args)
	{
		try {
			PropertyChangeCommand command = new PropertyChangeCommand(
				"Changed value of " + field.Name,
				field,
				object_,
				scriptEditor.Buffer.Text);
			command.Do();
			UndoManager.AddCommand(command);
		} catch(Exception e) {
			ErrorDialog.Exception(e);
		}

		field.Changed -= OnFieldChanged;
		scriptDialog.Hide();
	}

	protected void OnCancel(object o, EventArgs args)
	{
		field.Changed -= OnFieldChanged;
		scriptDialog.Hide();
	}

	/// <summary> Called when our field changes on any instance of same type as our Object. </summary>
	private void OnFieldChanged(object Object, FieldOrProperty field, object oldValue) {
		if (object_ == Object) {
			object val = field.GetValue(object_);
			scriptEditor.Buffer.Text = val != null ? val.ToString() : String.Empty;
		}
	}
}
