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

using System;
using LispReader;

namespace Undo {

	internal sealed class PropertyChangeCommand : Command {
		private FieldOrProperty field;
		private Object oldData;
		private Object newData;
		private Object obj;

		public override void Do() {
			field.SetValue(obj, newData);
		}

		public override void Undo() {
			field.SetValue(obj, oldData);
		}

		public PropertyChangeCommand(string title, FieldOrProperty field, Object obj, Object newData)
			: base(title) {
			this.obj = obj;
			this.field = field;
			this.oldData = field.GetValue(obj);
			this.newData = newData;
		}

		public PropertyChangeCommand(string title, FieldOrProperty field, Object obj, Object newData, Object oldData)
			: base(title) {
			this.obj = obj;
			this.field = field;
			this.oldData = oldData;
			this.newData = newData;
		}
	}
}

/* EOF */
