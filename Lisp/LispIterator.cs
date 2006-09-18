namespace Lisp
{

public class LispIterator {
	private List IterList;
	private int Pos;
	private string ChildKey;
	private List ChildList;

	public string Key {
		get {
			return ChildKey;
		}
	}
	public List List {
		get {
			return ChildList;
		}
	}

	public LispIterator(List List) {
		IterList = List;
	}

	public bool MoveNext() {
		List list;
		do {
			if(Pos >= IterList.Length)
				return false;
			object o = IterList[Pos++];
			if(! (o is List))
				continue;
			list = (List) o;
			if(list.Length == 0 || ! (list[0] is Symbol))
				continue;

			break;
		} while(true);

		ChildKey = ((Symbol) list[0]).Name;
		ChildList = list;

		return true;
	}
}

}
