using System;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property,
				AllowMultiple=false)]
public class CustomSettingsWidgetAttribute : Attribute
{
	public Type Type;
	
	public CustomSettingsWidgetAttribute(Type type)
	{
		this.Type = type;	
	}
}

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property,
				AllowMultiple=false)]
public class ChooseResourceSetting : CustomSettingsWidgetAttribute
{
	public ChooseResourceSetting() : base(typeof(ChooseResourceWidget))
	{
	}
}

