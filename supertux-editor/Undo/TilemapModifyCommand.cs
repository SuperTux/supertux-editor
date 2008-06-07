//  $Id: PropertyCommands.cs 4928 2007-03-07 21:18:40Z anmaster $
//
//  Copyright (C) 2007 Arvid Norlander <anmaster AT berlios DOT de>
//
//  This program is free software; you can redistribute it and/or
//  modify it under the terms of the GNU General Public License
//  as published by the Free Software Foundation; either version 2
//  of the License, or (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA
//  02111-1307, USA.
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
