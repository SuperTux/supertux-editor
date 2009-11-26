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

using DataStructures;
using OpenGl;
using System;
using Gdk;
using Undo;

public sealed class ObjectAreaChangeCommand : Command
{
	/// <summary>
	/// The object this action was on
	/// </summary>
	private IObject changedObject;
	/// <summary>
	/// Old area
	/// </summary>
	private RectangleF originalArea;
	/// <summary>
	/// New area
	/// </summary>
	private RectangleF newArea;

	public override void Do() {
		changedObject.ChangeArea(newArea);
	}

	public override void Undo() {
		changedObject.ChangeArea(originalArea);
	}

	public ObjectAreaChangeCommand(string title,
	                               RectangleF originalArea, RectangleF newArea,
	                               IObject changedObject)
	: base(title)
	{
		this.changedObject = changedObject;
		this.originalArea = originalArea;
		this.newArea = newArea;
	}
}

/* EOF */
