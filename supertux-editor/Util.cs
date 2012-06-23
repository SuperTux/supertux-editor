//  SuperTux Editor
//  Copyright (C) 2006 Matthias Braun <matze@braunis.de>
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
using System.IO;
using Lisp;
using Resources;

/// <summary>This class contains some convenience functions</summary>
public static class Util {
	public static List Load(string Filename, string RootElement) {
		return Load(new StreamReader(ResourceManager.Instance.Get(Filename)), Filename, RootElement);
	}

	public static List Load(TextReader Reader, string Source, string RootElement) {
		try {
			Lexer Lexer = new Lexer(Reader);
			Parser Parser = new Parser(Lexer);

			List Root = Parser.Parse();
			Properties RootP = new Properties(Root);

			List Result = null;
			if(!RootP.Get(RootElement, ref Result))
				throw new LispException("'" + Source + "' is not a " + RootElement + " file");

			return Result;
		} catch(Exception e) {
			throw new LispException("Problem parsing '" + Source + "': " + e.Message, e);
		}
	}
}

/* EOF */
