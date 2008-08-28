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
using LispReader;

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
		/// The position in the List.
		/// </summary>
		protected int position;
		/// <summary>
		/// Object we're changing.
		/// </summary>
		protected object Object;
		/// <summary>
		/// Field with changed list.
		/// </summary>
		protected FieldOrProperty field;

		protected SortedListCommand(string title, object Object, FieldOrProperty field, T changedItem, int position)
			: base(title) {
			this.Object = Object;
			this.field = field;
			this.changedItem = changedItem;
			this.position = position;
			this.changedList = (List<T>) field.GetValue(Object);
		}

	}


	internal class SortedListAddCommand<T> : SortedListCommand<T> {
		public override void Do() {
			changedList.Insert(position, changedItem);
			field.FireChanged(Object);
		}

		public override void Undo() {
			changedList.RemoveAt(position);
			field.FireChanged(Object);
		}

		public SortedListAddCommand(string title, object Object, FieldOrProperty field, T changedItem)
			: base(title, Object, field, changedItem, 0) {
			position = changedList.Count;
		}
		public SortedListAddCommand(string title, object Object, FieldOrProperty field, T changedItem, int position)
			: base(title, Object, field, changedItem, position) { }
	}


	// TODO: Possible mem leak with objects hanging around forever?
	internal sealed class SortedListRemoveCommand<T> : SortedListAddCommand<T> {
		public override void Do() {
			base.Undo();
		}

		public override void Undo() {
			base.Do();
		}

		public SortedListRemoveCommand(string title, object Object, FieldOrProperty field, T changedItem)
			: base(title, Object, field, changedItem, 0) {
			position = changedList.IndexOf(changedItem);
		}
		public SortedListRemoveCommand(string title, object Object, FieldOrProperty field, int position)
			: base(title, Object, field, default(T), position) {
			changedItem = changedList[position];
		}
	}


	internal sealed class SortedListMoveCommand<T> : Command {
		private List<T> changedList;	/// <summary>The List the Item was/is in</summary>
		private int position;		/// <summary>Current number of item</summary>
		private int newPosition;	/// <summary>New number of item</summary>
		private object Object;		/// <summary> Object we're changing </summary>
		private FieldOrProperty field;	/// <summary> Field with changed list. </summary>

		public override void Do() {
			T changedItem = changedList[position];
			changedList.RemoveAt(position);
			changedList.Insert(newPosition, changedItem);
			field.FireChanged(Object);
		}

		public override void Undo() {
			T changedItem = changedList[newPosition];
			changedList.RemoveAt(newPosition);
			changedList.Insert(position, changedItem);
			field.FireChanged(Object);
		}

		public SortedListMoveCommand(string title, object Object, FieldOrProperty field, int position, int newPosition) : base(title) {
			this.Object = Object;
			this.field = field;
			this.position = position;
			this.newPosition = newPosition;
			this.changedList = (List<T>) this.field.GetValue(this.Object);
		}
	}
}
