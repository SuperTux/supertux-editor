//  $Id$
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

namespace Undo {

	public abstract class Command {

		protected string title;
		/// <summary>
		/// The title of this command to be shown to the user.
		/// </summary>
		public string Title {
			get {
				return title;
			}
		}

		/// <summary>
		/// Execute the command.
		/// </summary>
		/// <remarks>
		/// Must only be called after undo has be called
		/// or before either undo or do has been called.
		/// </remarks>
		public abstract void Do();
		/// <summary>
		/// Undo the command.
		/// </summary>
		/// <remarks>
		/// Must only be called after do has be called.
		/// </remarks>
		public abstract void Undo();

		/// <summary>
		/// Protected constructor for use in specifc Command.
		/// </summary>
		/// <param name="title">The title shown to the user.</param>
		protected Command(string title) {
			this.title = title;
		}

	}
}
