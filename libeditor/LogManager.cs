// $Id$
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

/// <summary>
/// Loglevels for logging.
/// </summary>
/// <seealso cref="LogManager"/>
public enum LogLevel {
	/// <summary>
	/// Debug messages for developers.
	/// </summary>
	Debug,
	/// <summary>
	/// Warnings/Errors for developers, they should fix the issue.
	/// These <strong>should</strong> never show up in a release.
	/// </summary>
	DebugWarning,
	Info,
	Warning,
	Error,
	/// <summary>
	/// The world will end (or at least this part of it), maybe with
	/// emergency save of level, maybe not.
	/// </summary>
	Fatal
}

public static class LogManager {

	/// <summary>
	/// Returns string to use as prefix for <paramref name="loglevel"/>
	/// </summary>
	/// <param name="loglevel">The loglevel</param>
	/// <returns>The prefix to use in the format of "XXX: ".</returns>
	private static string GetLevelString(LogLevel loglevel) {
		switch (loglevel) {
			case LogLevel.Debug:
				return "DEBUG: ";
			case LogLevel.DebugWarning:
				return "DEBUGWARN: ";
			case LogLevel.Info:
				return "INFO:  ";
			case LogLevel.Warning:
				return "WARN:  ";
			case LogLevel.Error:
				return "ERROR: ";
			case LogLevel.Fatal:
				return "FATAL: ";
			default:
				return loglevel.ToString();
		}
	}

	/// <summary>
	/// Log a message with <paramref name="loglevel"/>
	/// </summary>
	/// <remarks>
	/// Currently this logs to STDERR for <see cref="LogLevel.Error"/> and
	/// <see cref="LogLevel.Fatal"/> and other levels to STDOUT.
	/// </remarks>
	/// <param name="loglevel">The log level of this message.</param>
	/// <param name="message">The message to log</param>
	public static void Log(LogLevel loglevel, string message) {
		if (loglevel == LogLevel.Fatal || loglevel == LogLevel.Error)
			Console.Error.WriteLine(loglevel.ToString() + ": " + message);
		Console.WriteLine(GetLevelString(loglevel) + message);
	}

	/// <summary>
	/// Log a message with <paramref name="loglevel"/>
	/// </summary>
	/// <remarks>
	/// Currently this logs to STDERR for <see cref="LogLevel.Error"/> and
	/// <see cref="LogLevel.Fatal"/> and other levels to STDOUT.
	/// </remarks>
	/// <param name="loglevel">The log level of this message.</param>
	/// <param name="format">A format string.</param>
	/// <param name="arg0">First object for format string</param>
	public static void Log(LogLevel loglevel, string format, object arg0) {
		if (loglevel == LogLevel.Fatal || loglevel == LogLevel.Error)
			Console.Error.WriteLine(loglevel.ToString() + ": " + format, arg0);
		Console.WriteLine(GetLevelString(loglevel) + format, arg0);
	}

	/// <summary>
	/// Log a message with <paramref name="loglevel"/>
	/// </summary>
	/// <remarks>
	/// Currently this logs to STDERR for <see cref="LogLevel.Error"/> and
	/// <see cref="LogLevel.Fatal"/> and other levels to STDOUT.
	/// </remarks>
	/// <param name="loglevel">The log level of this message.</param>
	/// <param name="format">A format string.</param>
	/// <param name="arg0">First object for format string</param>
	/// <param name="arg1">Second object for format string</param>
	public static void Log(LogLevel loglevel, string format, object arg0, object arg1) {
		if (loglevel == LogLevel.Fatal || loglevel == LogLevel.Error)
			Console.Error.WriteLine(loglevel.ToString() + ": " + format, arg0, arg1);
		Console.WriteLine(GetLevelString(loglevel) + format, arg0, arg1);
	}

	/// <summary>
	/// Log a message with <paramref name="loglevel"/>
	/// </summary>
	/// <remarks>
	/// Currently this logs to STDERR for <see cref="LogLevel.Error"/> and
	/// <see cref="LogLevel.Fatal"/> and other levels to STDOUT.
	/// </remarks>
	/// <param name="loglevel">The log level of this message.</param>
	/// <param name="format">A format string.</param>
	/// <param name="args">Array of object for format string</param>
	public static void Log(LogLevel loglevel, string format, params object[] args) {
		if (loglevel == LogLevel.Fatal || loglevel == LogLevel.Error)
			Console.Error.WriteLine(loglevel.ToString() + ": " + format, args);
		Console.WriteLine(GetLevelString(loglevel) + format, args);
	}


}
