//  SuperTux Editor
//  Copyright (C) 2008 SuperTux Devel Team
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

	internal class PathShiftCommand : Command {
		/// <summary>
		/// The Path the Node was/is in.
		/// </summary>
		protected Path changedPath;
		/// <summary>
		/// The shift delta.
		/// </summary>
		protected int delta;

		public override void Do() {
			changedPath.Shift(delta);
		}

		public override void Undo() {
			changedPath.Shift(-delta);
		}

		public PathShiftCommand(string title, Path changedPath, int delta)
			: base(title) {
			this.delta = delta;
			this.changedPath = changedPath;
		}
	}

}

/* EOF */
