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
using System.Collections.Generic;

namespace Undo {

	/// <summary>
	/// Delegate for undo, redo and AddCommand.
	/// </summary>
	/// <param name="command">The command that has just been processed</param>
	public delegate void UndoHandler(Command command);

	// TODO: modified flag.
	public static class UndoManager {
		private static IEditorApplication application;
		/// <summary>
		/// Commands that can be undone are here
		/// </summary>
		private static Stack<Command> UndoStack = new Stack<Command>();
		/// <summary>
		/// Commands that can be redone are here
		/// </summary>
		private static Stack<Command> RedoStack = new Stack<Command>();

		/// <summary>
		/// Item count, check before using other stuff
		/// so you don't try to get non-existing info.
		/// </summary>
		public static int UndoCount {
			get { return UndoStack.Count; }
		}

		/// <summary>
		/// Item count, check before using other stuff
		/// so you don't try to get non-existing info.
		/// </summary>
		public static int RedoCount {
			get { return RedoStack.Count; }
		}

		// TODO: Handle empty stack in a good way.
		/// <summary>
		/// Title of top undo item
		/// </summary>
		public static string UndoTitle {
			get { return UndoStack.Peek().Title; }
		}

		// TODO: Handle empty stack in a good way.
		/// <summary>
		/// Title of top redo item
		/// </summary>
		public static string RedoTitle {
			get { return RedoStack.Peek().Title; }
		}

		// TODO: Handle empty stack in a good way.
		public static void Undo() {
			Command command = UndoStack.Pop();
			command.Undo();
			RedoStack.Push(command);
			if (OnUndo != null)
				OnUndo(command);
		}

		// TODO: Handle empty stack in a good way.
		public static void Redo() {
			Command command = RedoStack.Pop();
			command.Undo();
			UndoStack.Push(command);
			if (OnRedo != null)
				OnRedo(command);
		}

		/// <summary>
		/// Pushes a Command on the undo stack.
		/// Also clears the redo stack.
		/// </summary>
		/// <remarks>
		/// The command is not run here. The reason is making
		/// it simpler to add support to existing editors. They
		/// will have to run it before calling this.
		/// </remarks>
		/// <param name="command">The command to operate on</param>
		public static void AddCommand(Command command) {
			UndoStack.Push(command);
			RedoStack.Clear();
			LogManager.Log(LogLevel.Debug, "UndoManager.AddCommand({0})", command.Title);
			if (OnAddCommand != null)
				OnAddCommand(command);
		}

		/// <summary>
		/// Clears undo/redo stacks, use when loading new level.
		/// </summary>
		public static void Clear() {
			RedoStack.Clear();
			UndoStack.Clear();
		}

		private static Command savedCommand;

		public static void MarkAsSaved() {
			if (UndoStack.Count < 1) {
				LogManager.Log(LogLevel.Debug, "UndoManager.MarkAsSaved() called when UndoStack was empty.");
				return;
			}
			savedCommand = UndoStack.Peek();
			LogManager.Log(LogLevel.Debug, "UndoManager.MarkAsSaved()");
		}

		public static bool IsDirty {
			get {
				if (UndoStack.Count < 1) return false;
				return savedCommand != UndoStack.Peek();
			}
		}

		public static event UndoHandler OnUndo;
		public static event UndoHandler OnRedo;
		public static event UndoHandler OnAddCommand;
	}
}
