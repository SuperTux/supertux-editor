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
using Gdk;

public sealed class TilePaintTool : TileToolBase, ITool
{
	public TilePaintTool(Application application, Tileset Tileset, TileSelection selection)
		: base(application, Tileset, selection) 
	{
		ActionName = "Change Tiles";
	}

	public override void PerformActionOnTile(FieldPos TilePos, ModifierType Modifiers)
	{
		selection.ApplyToTilemap(TilePos, application.CurrentTilemap, ((Modifiers & ModifierType.ControlMask) == 0));
	}
}

/* EOF */
