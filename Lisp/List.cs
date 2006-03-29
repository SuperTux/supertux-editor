using System;
using System.Collections;

namespace Lisp
{

public class List : IEnumerable {
    private object[] entries;

    public object this[int index] {
        get {
            return entries[index];
        }
        set {
            entries[index] = value;
        }
    }
    public int Length {
        get {
            return entries.Length;
        }
    }

    public IEnumerator GetEnumerator() {
        return entries.GetEnumerator();
    }

    public void Resize(int NewSize) {
        object[] newentries = new object[NewSize];
        for(int i = 0; i < System.Math.Min(NewSize, entries.Length); ++i) {
            newentries[i] = entries[i];
        }
        entries = newentries;
    }

    public List(object[] entries) {
        this.entries = entries;
    }

	public override string ToString() {
		string result = "(";
		foreach(object item in entries) {
			result += item.ToString() + " ";
		}
		result += ")";
		return result;
	}
}

}
