using System;
using Gtk;
using GtkSourceView;
using LispReader;

public class ScriptEditor
{	
	private static SourceLanguagesManager manager;
	private static SourceLanguage lang;

	private Window window;
	private SourceBuffer buffer;
	private FieldOrProperty field;
	private object object_;
	
	static ScriptEditor() {
		manager = new SourceLanguagesManager();
		lang = manager.GetLanguageFromMimeType ("text/x-squirrel");
	}	
	
	public ScriptEditor(string title, FieldOrProperty field, object object_)
	{
		this.field = field;
		this.object_ = object_;
		
		window = new Window("Supertux - ScriptEditor - " + title);
		window.SetSizeRequest(640, 500);
		
		buffer = new SourceBuffer (lang);
		buffer.Highlight = true;
		object val = field.GetValue(object_);
		buffer.Text = val != null ? val.ToString() : "";
		
		ScrolledWindow scrollWindow = new ScrolledWindow();
		SourceView view = new SourceView(buffer);
		view.AutoIndent = true;
		view.ShowMargin = true;
		view.TabsWidth = 4;
		view.SmartHomeEnd = true;
		view.ShowLineNumbers = true;
		view.ShowLineMarkers = true;
		
		scrollWindow.Add(view);
		
		VBox box = new VBox();
		box.PackStart(scrollWindow, true, true, 0);
		
		ButtonBox buttonBox = new HButtonBox();
		buttonBox.BorderWidth = 12;
		buttonBox.Spacing = 6;
		buttonBox.Layout = ButtonBoxStyle.End;		
		
		Button closeButton = new Button(Stock.Close);
		closeButton.Clicked += OnClose;
		buttonBox.Add(closeButton);
		
		box.PackStart(buttonBox, false, false, 0); 
		
		window.Add(box);
		window.ShowAll();
	}
	
	private void OnClose(object sender, EventArgs args)
	{
		field.SetValue(object_, buffer.Text);
		window.Destroy();
	}
}
