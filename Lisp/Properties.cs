//  $Id$
using System;
using System.Collections;
using System.Collections.Generic;

namespace Lisp
{

public class Properties {
	private Hashtable Props = new Hashtable();

	public Properties(List List) {
		for(int i = 0; i < List.Length; ++i) {
			object o = List[i];
			if(i == 0 && o is Symbol)
				continue;

			if(! (o is List))
				throw new Exception("Child of properties lisp is not a list");
			List ChildList = (List) o;
			if(ChildList.Length > 0) {
				if(! (ChildList[0] is Symbol))
					throw new Exception("property has no symbol as name");
				Symbol name = (Symbol) ChildList[0];

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
		if(! (list[1] is string))
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

		Val = (Enum)Enum.Parse(proptype ,(string)list[1]);
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
			AList.Add((string) list[i]);
		}
		return true;
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

	public void PrintUnusedWarnings() {
	}
}

}
