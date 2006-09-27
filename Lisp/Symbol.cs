//  $Id$
namespace Lisp {

public class Symbol {
	private string name;
	public string Name {
		get {
			return name;
		}
	}

	public Symbol(string Name) {
		this.name = Name;
	}

	public override string ToString() {
		return "`" + name + "`";
	}
}

}
