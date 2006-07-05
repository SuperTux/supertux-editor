using System;
using Drawing;
using Gtk;
using Gdk;
using LispReader;

/// <summary>
/// Colour choosing widget for properties.
/// </summary>
/// <remarks>
/// TODO: Add support to set alpha value too.
/// </remarks>
public class ChooseColorWidget : ICustomSettingsWidget
{	
	public FieldOrProperty field;
	public FieldOrProperty Field {
		get {
			return field;
		}
		set {
			field = value;
		}
	}
	
	public object _object;
	public object Object {
		get {
			return _object;
		}
		set {
			_object = value;
		}
	}
	
	private ColorButton colorButton;
	
	public ChooseColorWidget()
	{
	}
	
	public Widget Create()
	{
		Drawing.Color val = (Drawing.Color) field.GetValue(Object);
		
		colorButton = new ColorButton();
		Gdk.Color color = new Gdk.Color(
		                                (byte) (val.Red * 255f),
		                                (byte) (val.Green * 255f),
		                                (byte) (val.Blue * 255f));
		/*
		color.Red = (ushort) (val.Red * 65536f);
		color.Green = (ushort) (val.Green * 65536f);
		color.Blue = (ushort) (val.Blue * 65536f);
		*/
		colorButton.Color = color;
		
		colorButton.ColorSet += OnChooseColor;
		
		colorButton.Name = field.Name;
		return colorButton;
	}
	
	private void OnChooseColor(object sender, EventArgs args)
	{
		Drawing.Color col = new Drawing.Color();
		col.Red = ((float) colorButton.Color.Red) / 65536f;
		col.Blue = ((float) colorButton.Color.Blue) / 65536f;
		col.Green = ((float) colorButton.Color.Green) / 65536f;
		col.Alpha = 1.0f;
		field.SetValue(Object, col);
	}	
}

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property,
                AllowMultiple=false)]
public sealed class ChooseColorSetting : CustomSettingsWidgetAttribute
{
	public ChooseColorSetting() : base(typeof(ChooseColorWidget))
	{
	}
}
