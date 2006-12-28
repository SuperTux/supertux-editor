//  $Id$
//
// ErrorDialog.cs
//
// Author:
//   Lluis Sanchez Gual
//
// Copyright (C) 2005 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using Gtk;
using Glade;

public class ErrorDialog : IDisposable
{
	[Glade.Widget("ErrorDialog")]
	Dialog dialog = null;
	[Glade.Widget]
	Button okButton = null;
	[Glade.Widget]
	Label descriptionLabel = null;
	[Glade.Widget]
	Gtk.TextView detailsTextView = null;
	[Glade.Widget]
	Gtk.Expander expander = null;

	TextTag tagNoWrap;
	TextTag tagWrap;

	public static void Exception(Exception e)
	{
		Exception("Unexpected Exception", e);
	}

	public static void Exception(string Mess, Exception e)
	{
		ErrorDialog dialog = new ErrorDialog(null);

		dialog.Message = Mess + ": " + e.Message;
		do {
			dialog.AddDetails("\"" + e.Message + "\"\n", false);
			dialog.AddDetails(e.StackTrace, false);

			if(e.InnerException != null) {
				dialog.AddDetails("\n\n--Caused by--\n\n", false);
			}
			e = e.InnerException;
		} while(e != null);

		dialog.Show();
	}

	/// <summary>
	/// Show error dialog for non-exceptions.
	/// </summary>
	/// <param name="message">Error message to show</param>
	public static void ShowError(string message) {
		ErrorDialog dialog = new ErrorDialog(null);
		dialog.dialog.Title = "Supertux-Editor Error";
		dialog.Message = message;
		dialog.Show();
	}

	/// <summary>
	/// Show error dialog for non-exceptions.
	/// </summary>
	/// <param name="message">Error message to show</param>
	/// <param name="details">Message to put under "details".</param>
	public static void ShowError(string message, string details) {
		ErrorDialog dialog = new ErrorDialog(null);
		dialog.dialog.Title = "Supertux-Editor Error";
		dialog.Message = message;
		dialog.AddDetails(details, true);
		dialog.Show();
	}


	public ErrorDialog(Window parent)
	{
		new Glade.XML(null, "errordialog.glade", "ErrorDialog", null).Autoconnect(this);
		dialog.TransientFor = parent;
		okButton.Clicked += new EventHandler(OnClose);
		expander.Activated += new EventHandler(OnExpanded);
		descriptionLabel.ModifyBg(StateType.Normal, new Gdk.Color(255, 0, 0));

		tagNoWrap = new TextTag("nowrap");
		tagNoWrap.WrapMode = WrapMode.None;
		detailsTextView.Buffer.TagTable.Add(tagNoWrap);

		tagWrap = new TextTag("wrap");
		tagWrap.WrapMode = WrapMode.Word;
		detailsTextView.Buffer.TagTable.Add(tagWrap);

		expander.Visible = false;
	}

	public string Message
	{
		get { return descriptionLabel.Text; }
		set
		{
			string message = value;
			while(message.EndsWith("\r") || message.EndsWith("\n"))
				message = message.Substring(0, message.Length - 1);
			if(!message.EndsWith("."))
				message += ".";
			descriptionLabel.Text = message;
		}
	}

	public void AddDetails(string text, bool wrapped)
	{
		TextIter it = detailsTextView.Buffer.EndIter;
		if(wrapped)
			detailsTextView.Buffer.InsertWithTags(ref it, text, tagWrap);
		else
			detailsTextView.Buffer.InsertWithTags(ref it, text, tagNoWrap);
		expander.Visible = true;
	}

	public void Show()
	{
		dialog.Show();
	}

	public void Run()
	{
		dialog.Show();
		dialog.Run();
	}

	public void Dispose()
	{
		dialog.Destroy();
		dialog.Dispose();
	}

	void OnClose(object sender, EventArgs args)
	{
		dialog.Destroy();
	}

	void OnExpanded(object sender, EventArgs args)
	{
		GLib.Timeout.Add(100, new GLib.TimeoutHandler(UpdateSize));
	}

	bool UpdateSize()
	{
		int w, h;
		dialog.GetSize(out w, out h);
		dialog.Resize(w, 1);
		return false;
	}
}
