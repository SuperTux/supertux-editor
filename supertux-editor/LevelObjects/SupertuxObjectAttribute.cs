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

// TODO: Write better documentation.
/// <summary>Attribute that marks a class as an object for use in levels/worldmaps.</summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
public sealed class SupertuxObjectAttribute : Attribute
{
	/// <summary>Tells when the object will be shown in the object list.</summary>
	public enum Usage {
		/// <summary>Should only be shown everywhere. This is the default.</summary>
		Any,
		/// <summary>Should not be shown at all.</summary>
		None,
		/// <summary>Should only be shown for worldmaps.</summary>
		WorldmapOnly,
		/// <summary>Should only be shown for "normal" levels.</summary>
		LevelOnly
	}

	public string Name;
	public string IconSprite;
	public string ObjectListAction;
	/// <summary>A <see cref="Usage"/> enum describing where the object can be used.</summary>
	public Usage Target;

	/// <summary>Constructs for <see cref="SupertuxObjectAttribute"/>.</summary>
	/// <param name="Name">Name of object in the level file</param>
	/// <param name="IconSprite">Icon used in the object list in the editor</param>
	public SupertuxObjectAttribute(string Name, string IconSprite) {
		this.Name = Name;
		this.IconSprite = IconSprite;
	}
}

/* EOF */
