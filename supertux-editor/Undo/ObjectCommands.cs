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

namespace Undo {

	public abstract class ObjectCommand : Command {
		/// <summary>
		/// The object this action was on
		/// </summary>
		protected IGameObject changedObject;
		/// <summary>
		/// The object the object was/is in.
		/// </summary>
		protected Sector sector;

		protected ObjectCommand(string title, IGameObject changedObject, Sector sector)
			: base(title) {
			this.changedObject = changedObject;
			this.sector = sector;
		}
	}

	internal class ObjectAddCommand : ObjectCommand {
		public override void Do() {
			sector.Add(changedObject, true);
		}

		public override void Undo() {
			sector.Remove(changedObject, true);
		}

		public ObjectAddCommand(string title, IGameObject changedObject, Sector sector)
			: base(title, changedObject, sector) { }
	}

	// TODO: Possible mem leak with objects hanging around forever?
	internal sealed class ObjectRemoveCommand : ObjectAddCommand {
		public override void Do() {
			base.Undo();
		}

		public override void Undo() {
			base.Do();
		}

		public ObjectRemoveCommand(string title, IGameObject changedObject, Sector sector)
			: base(title, changedObject, sector) { }
	}

}

/* EOF */
