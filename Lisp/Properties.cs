//  $Id$
using System;
using System.Collections;
using System.Collections.Generic;

namespace Lisp
{

public class Properties {
	private Hashtable Props = new Hashtable();

	private ArrayList LinearList;

	public Properties(List List) {
		LinearList = new ArrayList();

		for(int i = 0; i < List.Length; ++i) {
			object o = List[i];
			if(i == 0 && o is Symbol)
				continue;

			List ChildList = o as List;

			LinearList.Add(ChildList);

			if(o == null)
				throw new LispException("Child of properties lisp is not a list");
			if(ChildList.Length > 0) {
				Symbol name = ChildList[0] as Symbol;
				if(name == null)
					throw new LispException("property has no symbol as name");

				object old = Props[name.Name];
				if(old == null) {
					ArrayList AList = new ArrayList();
					AList.Add(ChildList);
					Props[name.Name] = AList;
				} else {
					((ArrayList) old).Add(ChildList);
				}
			}
		}
	}

	private List Find(string Name) {
		ArrayList AList = (ArrayList) Props[Name];
		if(AList == null)
			return null;

		return (List) AList[0];
	}

	/// <summary>Checks if element exists</summary>
	/// <param name="Name">Name of element to find.</param>
	/// <returns>False if element doesn't exist, otherwise true.</returns>
	public bool Exists(string Name) {
		List list = Find(Name);
		if (list == null)
			return false;
		return true;
	}

	public bool Get(string Name, ref int Val) {
		List list = Find(Name);
		if(list == null)
			return false;
		if(! (list[1] is int))
			return false;

		Val = (int) list[1];
		return true;
	}

	public bool Get(string Name, ref uint Val) {
		List list = Find(Name);
		if(list == null)
			return false;
		if(! (list[1] is int))
			return false;
		int v = (int) list[1];
		if(v < 0)
			return false;

		Val = (uint) v;
		return true;
	}

	public bool Get(string Name, ref float Val) {
		List list = Find(Name);
		if(list == null)
			return false;
		if(list[1] is float) {
			Val = (float) list[1];
			return true;
		}
		if(list[1] is int) {
			Val = (float) ((int) list[1]);
			return true;
		}
		return false;
	}

	public bool Get(string Name, ref string Val) {
		List list = Find(Name);
		if(list == null)
			return false;
		if(!(list.Length >= 2 && list[1] is string))
			return false;

		Val = (string) list[1];
		return true;
	}

	public bool Get(string Name, ref bool Val) {
		List list = Find(Name);
		if(list == null)
			return false;
		if(! (list[1] is bool))
			return false;

		Val = (bool) list[1];
		return true;
	}

	/// <summary>
	/// Gets a string from somewhere unknown
	/// (due to bad documentation in the rest of this class) into an Enum.
	/// </summary>
	/// <param name="Name">Name of attribute to find.</param>
	/// <param name="Val">The <see cref="Enum"/> is returned in this.</param>
	/// <param name="proptype">A <see cref="Type"/> of the Enum we want.</param>
	/// <returns>False if we failed to get the value Name, otherwise true.</returns>
	public bool Get(string Name, ref Enum Val, Type proptype) {
		List list = Find(Name);
		if (list == null)
			return false;
		if (!(list[1] is string))
			return false;

		try {
			Val = (Enum)Enum.Parse(proptype, (string)list[1]);
		} catch(System.ArgumentException) {
			Console.WriteLine($"Properties.Get(): failed to convert: '{list[1]}' to enum at '{Name}'");
			throw;
		}

		return true;
	}

	public bool Get(string Name, ref List Val) {
		List list = Find(Name);
		if(list == null)
			return false;
		Val = list;
		return true;
	}

	public bool GetStringList(string Name, List<string> AList) {
		List list = Find(Name);
		if(list == null)
			return false;
		for(int i = 1; i < list.Length; ++i) {
			try {
				AList.Add((string) list[i]);
			}
			catch(InvalidCastException) {
				AList.Add(((Lisp.Symbol)list[i]).Name);
			}
		}
		return true;
	}

	public bool GetLispList(string Name, out Lisp.List result) {
		List list = Find(Name);
		if(list == null) {
			result = null;
			return false;
		} else {
			object[] entries = new object[list.Length - 1];
			for(int i = 1; i < list.Length; ++i) {
				entries[i - 1] = list[i];
			}
			result = new List(entries);

			return true;
		}
	}

	public bool GetUIntList(string Name, List<uint> AList) {
		List list = Find(Name);
		if(list == null)
			return false;
		for(int i = 1; i < list.Length; ++i) {
			int v = (int) list[i];
			AList.Add((uint) v);
		}
		return true;
	}

	public bool GetIntList(string Name, List<int> AList) {
		List list = Find(Name);
		if(list == null)
			return false;
		for(int i = 1; i < list.Length; ++i) {
			AList.Add((int) list[i]);
		}
		return true;
	}

	public bool GetFloatList(string Name, List<float> AList) {
		List list = Find(Name);
		if(list == null)
			return false;
		for(int i = 1; i < list.Length; ++i) {
			if( list[i] is float) {
				AList.Add((float) list[i]);
			} else if(list[1] is int) {
				AList.Add((float) ((int) list[i]));
			} else {
				return false;
			}
		}
		return true;
	}

	public IList GetList(string ChildType) {
		ArrayList AList = (ArrayList) Props[ChildType];
		if(AList == null)
			return new ArrayList();

		return AList;
	}

	public IList GetList() {
		return LinearList;
	}

	public void PrintUnusedWarnings() {
	}
}

}
