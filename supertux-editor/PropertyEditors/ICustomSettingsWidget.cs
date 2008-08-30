//  $Id$
using System;
using System.Reflection;
using Gtk;
using LispReader;

public interface ICustomSettingsWidget
{
	Widget Create(object caller, object Object, FieldOrProperty Field);
}
