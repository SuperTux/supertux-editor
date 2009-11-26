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

/* EOF */
