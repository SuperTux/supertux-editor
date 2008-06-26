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
using System.Collections.Generic;

namespace Undo {

	public abstract class SortedListCommand<T> : Command {
		/// <summary>
		/// Item this action was on
		/// </summary>
		protected T changedItem;
		/// <summary>
		/// The List the Item was/is in.
		/// </summary>
		protected List<T> changedList;
		/// <summary>
		/// The position in the List, optional.
		/// </summary>
		protected int position;

		protected SortedListCommand(string title, T changedItem, List<T> changedList)
			: base(title) {
			this.changedItem = changedItem;
			this.changedList = changedList;
			position = -1;
		}

		protected SortedListCommand(string title, T changedItem, List<T> changedList, int position)
			: base(title) {
			this.changedItem = changedItem;
			this.changedList = changedList;
			this.position = position;
		}

	}


	internal class SortedListAddCommand<T> : SortedListCommand<T> {
		public override void Do() {
			if (position > -1) {	//If we have specified position, add it at this position
				changedList.Insert(position, changedItem);
			} else {	//If we haven't, we add it to the last and fill position field
				changedList.Add(changedItem);
				position = changedList.Count-1;
			}
		}

		public override void Undo() {
			if (position == -1)
				position = changedList.IndexOf(changedItem);
			changedList.RemoveAt(position);
		}

		public SortedListAddCommand(string title, T changedItem, List<T> changedList)
			: base(title, changedItem, changedList) { }
		public SortedListAddCommand(string title, T changedItem, List<T> changedList, int position)
			: base(title, changedItem, changedList, position) { }
	}


	// TODO: Possible mem leak with objects hanging around forever?
	internal sealed class SortedListRemoveCommand<T> : SortedListAddCommand<T> {
		public override void Do() {
			base.Undo();
		}

		public override void Undo() {
			base.Do();
		}
		public SortedListRemoveCommand(string title, T changedItem, List<T> changedList)
			: base(title, changedItem, changedList) { }
		public SortedListRemoveCommand(string title, T changedItem, List<T> changedList, int position)
			: base(title, changedItem, changedList, position) { }
	}


	internal sealed class SortedListMoveCommand<T> : Command {
		private List<T> changedList;	/// <summary>The List the Item was/is in</summary>
		private int position;		/// <summary>Current number of item</summary>
		private int newPosition;	/// <summary>New number of item</summary>

		public override void Do() {
			T changedItem = changedList[position];
			changedList.RemoveAt(position);
			changedList.Insert(newPosition, changedItem);
		}

		public override void Undo() {
			T changedItem = changedList[newPosition];
			changedList.RemoveAt(newPosition);
			changedList.Insert(position, changedItem);
		}

		public SortedListMoveCommand(string title, List<T> changedList, int position, int newPosition) : base(title) {
			this.changedList = changedList;
			this.position = position;
			this.newPosition = newPosition;
		}
	}
}
