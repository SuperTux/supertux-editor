// $Id$
//
//  Copyright (C) 2006 Arvid Norlander <anmaster AT berlios DOT de>
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
	DEBUG,
	INFO,
	WARNING,
	ERROR,
	FATAL
}

public static class LogManager {

	public static void WriteLine(LogLevel loglevel, string message) {
		if (loglevel == LogLevel.FATAL || loglevel == LogLevel.ERROR)
			Console.Error.WriteLine(loglevel.ToString() + ": " + message);
		Console.WriteLine(loglevel.ToString() + ": " + message);
	}

	public static void WriteLine(LogLevel loglevel, string message, object arg0) {
		if (loglevel == LogLevel.FATAL || loglevel == LogLevel.ERROR)
			Console.Error.WriteLine(loglevel.ToString() + ": " + message, arg0);
		Console.WriteLine(loglevel.ToString() + ": " + message, arg0);
	}

}
