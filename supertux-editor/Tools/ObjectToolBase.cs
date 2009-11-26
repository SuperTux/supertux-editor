//  SuperTux Editor
//  Copyright (C) 2007 Arvid Norlander <anmaster AT berlios DOT de>
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

using DataStructures;
using OpenGl;
using System;
using Gdk;
using Undo;

// TODO: More things should be moved into this class.
/// <summary>
/// Base class for editors editing objects.
/// </summary>
public abstract class ObjectEditorBase : EditorBase
{
	protected Sector sector;

	/// <summary>
	/// Returns unit to snap to, based on passed Modifier keys and application settings
	/// </summary>
	protected int SnapValue(ModifierType Modifiers)
	{
		if ((Modifiers & ModifierType.ShiftMask) != 0) return 32;
		if ((Modifiers & ModifierType.ControlMask) != 0) return 16;
		if (application.SnapToGrid) return 32;
		return 0;
	}
}

/* EOF */
