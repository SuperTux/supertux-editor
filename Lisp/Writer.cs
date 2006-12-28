//  $Id$
using System;
using System.IO;
using System.Collections;
using System.Globalization;

namespace Lisp
{

public sealed class Writer {
	private TextWriter stream;
	private int IndentDepth;
	private Stack lists = new Stack();

	public Writer(TextWriter stream) {
		this.stream = stream;
	}

	public void WriteComment(string comment) {
		stream.WriteLine("; " + comment);
	}

	public void StartList(string name) {
		indent();
		stream.WriteLine("(" + name);
		IndentDepth += 2;
		lists.Push(name);
	}

	public void EndList(string name) {
		if(lists.Count == 0)
			throw new LispException("Trying to close list while none is open");
		string back = (string) lists.Pop();
		if(name != back)
			throw new LispException(String.Format("Trying to close {0} which is not open", name));

		IndentDepth -= 2;
		indent();
		stream.WriteLine(")");
	}

	public void Write(string name, object value) {
		indent();
		stream.Write("(" + name);
		if(value is string) {
			stream.Write(" \"");
			foreach(char c in value.ToString()) {
				if(c == '\"')
					stream.Write("\\\"");
				else if(c == '\\')
					stream.Write("\\\\");
				else
					stream.Write(c);
			}
			stream.Write("\"");
		} else if(value is IEnumerable) {
			foreach(object o in (IEnumerable) value) {
				stream.Write(" ");
				WriteValue(o);
			}
		} else {
			stream.Write(" ");
			WriteValue(value);
		}
		stream.WriteLine(")");
	}

	public void WriteTranslatable(string Name, string value) {
		indent();
		stream.WriteLine("(" + Name + " (_ \"" + value + "\"))");
	}

	private void WriteValue(object val) {
		if(val is bool) {
			stream.Write( ((bool) val) == true ? "#t" : "#f");
		} else if(val is int || val is uint) {
			stream.Write(val.ToString());
		} else if(val is float || val is double) {
			string num = String.Format(CultureInfo.InvariantCulture, "{0:G}", val);
			stream.Write(num);
		} else {
			stream.Write("\"" + val.ToString() + "\"");
		}
	}

	public void WriteVerbatimLine(string line) {
		indent();
		stream.WriteLine(line);
	}

	private void indent() {
			stream.Write(new String(' ', IndentDepth));
	}
}

}
