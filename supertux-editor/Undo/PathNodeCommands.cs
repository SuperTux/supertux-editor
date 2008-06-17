//  $Id$
//
//  Copyright (C) 2008 Milos Kloucek <MMlosh@supertuxdev.elektromaniak.cz>
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

namespace Undo {

	public abstract class PathNodeCommand : Command {
		/// <summary>
		/// The Node this action was on
		/// </summary>
		protected Path.Node changedPathNode;
		/// <summary>
		/// The Path the Node was/is in.
		/// </summary>
		protected Path changedPath;
		/// <summary>
		/// The position in the path, optional.
		/// </summary>
		protected int position;

		protected PathNodeCommand(string title, Path.Node changedPathNode, Path changedPath)
			: base(title) {
			this.changedPathNode = changedPathNode;
			this.changedPath = changedPath;
			position = -1;
		}

		protected PathNodeCommand(string title, Path.Node changedPathNode, Path changedPath, int position)
			: base(title) {
			this.changedPathNode = changedPathNode;
			this.changedPath = changedPath;
			this.position = position;
		}
	}


	internal class PathNodeAddCommand : PathNodeCommand {
		public override void Do() {
			if (position > -1) {	//If we have specified position, add it at this position
				changedPath.Nodes.Insert(position, changedPathNode);
			} else {	//If we haven't, we add it to the last and fill position field
				changedPath.Nodes.Add(changedPathNode);
				position = changedPath.Nodes.Count-1;
			}
		}

		public override void Undo() {
			changedPath.Nodes.Remove(changedPathNode);
		}

		public PathNodeAddCommand(string title, Path.Node changedPathNode, Path changedPath)
			: base(title, changedPathNode, changedPath) { }
		public PathNodeAddCommand(string title, Path.Node changedPathNode, Path changedPath, int position)
			: base(title, changedPathNode, changedPath, position) { }
	}


	// TODO: Possible mem leak with objects hanging around forever?
	internal sealed class PathNodeRemoveCommand : PathNodeAddCommand {
		public override void Do() {
			base.Undo();
		}

		public override void Undo() {
			base.Do();
		}
		public PathNodeRemoveCommand(string title, Path.Node changedPathNode, Path changedPath)
			: base(title, changedPathNode, changedPath) {
			position = changedPath.Nodes.IndexOf(changedPathNode);
		}
		public PathNodeRemoveCommand(string title, Path.Node changedPathNode, Path changedPath, int position)
			: base(title, changedPathNode, changedPath, position) { }
	}

}
