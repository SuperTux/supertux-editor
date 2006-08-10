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
public sealed class ChooseColorWidget : CustomSettingsWidget
{
	private ColorButton colorButton;

	/// <summary>Should we let the user set the alpha?</summary>
	private bool useAlpha;

	public ChooseColorWidget()
	{
	}

	public override Widget Create(object caller)
	{
		Drawing.Color val = (Drawing.Color) field.GetValue(Object);
		
		colorButton = new ColorButton();
		
		// Get if we should use alpha
		ChooseColorSettingAttribute chooseColorSetting = (ChooseColorSettingAttribute)
			field.GetCustomAttribute(typeof(ChooseColorSettingAttribute));
		useAlpha = chooseColorSetting.UseAlpha;

		if (useAlpha)
			colorButton.UseAlpha = true;
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
		if (useAlpha)
			colorButton.Alpha = (ushort) (val.Alpha * 65536f);
		
		colorButton.ColorSet += OnChooseColor;
		
		colorButton.Name = field.Name;

		// Create a tooltip if we can.
		CreateToolTip(caller, colorButton);

		return colorButton;
	}
	
	private void OnChooseColor(object sender, EventArgs args)
	{
		Drawing.Color col = new Drawing.Color();
		col.Red = ((float) colorButton.Color.Red) / 65536f;
		col.Blue = ((float) colorButton.Color.Blue) / 65536f;
		col.Green = ((float) colorButton.Color.Green) / 65536f;
		col.Alpha = 1f;
		if (useAlpha)
			col.Alpha = ((float) colorButton.Alpha) / 65536f;
		field.SetValue(Object, col);
	}
}

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property,
                AllowMultiple=false)]
public sealed class ChooseColorSettingAttribute : CustomSettingsWidgetAttribute
{
	/// <summary>Should we let the user set the alpha?</summary>
	public bool UseAlpha = false;

	public ChooseColorSettingAttribute() : base(typeof(ChooseColorWidget))
	{
	}
}
