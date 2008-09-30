//  $Id$
//
//  Copyright (C) 2008 Milos Kloucek <TuxMMlosh@elektromaniak.wz.cz>
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
using Lisp;
using LispReader;
using System.Collections.Generic;

[LispCustomSerializer(typeof(List<string>))]
public class StringListSerializer : ILispSerializer
{
	/// <summary>
	///		Creates an instance from the serialized object in
	///		<paramref name="list"/>
	/// </summary>
	/// <param name="list">The serialized object</param>
	/// <returns>The unserialized object</returns>
	/// <seealso cref="Write"/>
	public object Read(List list)
	{
		List<string> obj = new List<string>();

		for (int i = 1; i < list.Length; i++) {
			obj.Add((string)list[i]);
		}

		return obj;
	}
	/// <summary>
	///		Seralizes <paramref name="Object"/> using <paramref name="writer"/>
	/// </summary>
	/// <param name="writer">
	///		A <see cref="Writer"/> that <paramref name="Object"/> should be
	///		seralized to.</param>
	/// <param name="name">
	///		Name that should be used for the serialized lisp tree.
	/// </param>
	/// <param name="Object">
	///		The object to write.
	/// </param>
	/// <seealso cref="Read"/>
	public void Write(Writer writer, string name, object Object)
	{
		List<string> WrittenList = (List<string>) Object;

		if (WrittenList.Count < 1) return;

		string[] vals = new string[WrittenList.Count];
		for (int i = 0; i < WrittenList.Count; i++) {
			vals[i] = WrittenList[i];
		}
		writer.Write(name, vals);
	}
}
