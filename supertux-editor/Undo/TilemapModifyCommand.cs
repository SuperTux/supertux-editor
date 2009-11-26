//  SuperTux Editor
//  Copyright (C) 2007 SuperTux Devel Team
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
using LispReader;

namespace Undo {

	internal sealed class TilemapModifyCommand : Command {
		internal Tilemap changedTilemap;
		internal TileBlock.StateData oldState;
		internal TileBlock.StateData newState;

		public override void Do() {
			changedTilemap.RestoreState(newState);
		}

		public override void Undo() {
			changedTilemap.RestoreState(oldState);
		}

		public TilemapModifyCommand(string title, Tilemap changedTilemap, TileBlock.StateData oldState, TileBlock.StateData newState) : base(title) {
			this.changedTilemap = changedTilemap;
			this.oldState = oldState;
			this.newState = newState;
		}
	}
}

/* EOF */
