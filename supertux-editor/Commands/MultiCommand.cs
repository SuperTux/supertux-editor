//  SuperTux Editor
//  Copyright (C) 2009 Milos Kloucek <TuxMMlosh [AT] elektromaniak [DOT] wz [DOT] cz>
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

using System.Collections.Generic;
namespace Undo {

	internal sealed class MultiCommand : Command 
	{
		/// <summary>
		/// List of commands in this group.
		/// </summary>
		private List<Command> commandList;

		public void Add(Command command) 
		{
			commandList.Add(command);
		}

		public override void Do() {
			foreach (Command command in commandList)
				command.Do();
		}

		public override void Undo() {
			foreach (Command command in commandList)
				command.Undo();
		}

		public MultiCommand(string title)
			: base(title)
		{
			this.commandList = new List<Command>();
		}

		public MultiCommand(string title, List<Command> commandList)
			: base(title) 
		{
			this.commandList = commandList;
		}
	}
}

/* EOF */
