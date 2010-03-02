//  SuperTux Editor
//  Copyright (C) 2010 SuperTux Devel Team
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

using System.Collections;
using System.Collections.Generic;

 /*
 // Simple reference comparer, including its generic variant
 // Used to make Dictionaries/Hashtables immune to object.GetHashCode() changes
 */

public class ReferenceComparer : IEqualityComparer
{
	public new bool Equals(object lhs, object rhs)
	{
		return System.Object.ReferenceEquals(lhs, rhs);
	}
	
	public int GetHashCode(object obj)
	{
		return System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(obj);
	}
}

public class ReferenceComparer<T> : IEqualityComparer<T>
{
	public bool Equals(T lhs, T rhs)
	{
		return System.Object.ReferenceEquals(lhs, rhs);
	}
	
	public int GetHashCode(T obj)
	{
		return System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(obj);
	}
}

