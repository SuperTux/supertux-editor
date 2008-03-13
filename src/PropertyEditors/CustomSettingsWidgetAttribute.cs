//  $Id$
using System;

/// <summary>
/// Base class for custom settings widgets.
/// </summary>
public abstract class CustomSettingsWidgetAttribute : Attribute
{
	public Type Type;

	protected CustomSettingsWidgetAttribute(Type type)
	{
		this.Type = type;
	}
}
