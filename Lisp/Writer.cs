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

	/// <summary>
	/// Writes a comment to the stream.
	/// </summary>
	/// <param name="comment">Contents of the comment</param>
	public void WriteComment(string comment) {
		stream.WriteLine("; " + comment);
	}

	/// <summary>
	/// Starts a new lisp tree.
	/// </summary>
	/// <param name="name">The name of the lisp tree.</param>
	/// <seealso cref="EndList"/>
	public void StartList(string name) {
		indent();
		stream.WriteLine("(" + name);
		IndentDepth += 2;
		lists.Push(name);
	}

	/// <summary>
	/// Ends a lisptree opened with <see cref="StartList"/>.
	/// </summary>
	/// <param name="name">The name of the lisp tree.</param>
	/// <seealso cref="StartList"/>
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

	/// <summary>
	/// Write an object <paramref name="value"/> to
	/// the stream under the name
	/// <paramref name="name"/>.
	/// </summary>
	/// <param name="name">The name to write value to in lisp.</param>
	/// <param name="value">The object to write</param>
	/// <remarks>
	///		This function can write
	///		strings, lists that implements <see cref="IEnumerable"/>,
	///		and for anything else it will call <see cref="WriteValue"/>.
	///	</remarks>
	public void Write(string name, object value) {
		indent();
		stream.Write("(" + name);
		if((value is Lisp.List)) {
			stream.Write("\n");
			foreach(object o in (IEnumerable) value) {
				stream.Write(" ");
				WriteValue(o);
			}
			indent();
		} else if((value is IEnumerable) && !(value is string)) {
			if (value is Lisp.List)
			{
				stream.Write("\n");
			}
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

	/// <summary>
	/// Writes a string and marks it as translatable
	/// (for use with gettext?).
	/// </summary>
	/// <param name="Name">The name of the element to write.</param>
	/// <param name="value">The value of it.</param>
	public void WriteTranslatable(string Name, string value) {
		indent();
		stream.Write("(" + Name + " (_ ");
		WriteValue(value);
		stream.WriteLine("))");
	}

	/// <summary>
	/// Processes and writes a value <paramref name="val"/>.
	/// </summary>
	/// <remarks>
	///		If <paramref name="val"/> is a
	///		<list type="table">
	///			<listheader>
	///				<term>Type</term>
	///				<description>What will be written</description>
	///			</listheader>
	///			<item>
	///				<term><see cref="Boolean"/></term>
	///				<description>#t or #f will be written.</description>
	///			</item>
	///			<item>
	///				<term><see cref="Single"/> or <see cref="Double"/></term>
	///				<description>
	///					The floating point number will be formated with
	///					<see cref="CultureInfo.InvariantCulture"/>
	///				</description>
	///			</item>
	///			<item>
	///				<term>Other</term>
	///				<description>Call ToString() of <paramref name="val"/></description>
	///			</item>
	///		</list>
	/// </remarks>
	/// <param name="val">The value to write</param>
	private void WriteValue(object val) {
		if(val is bool) {
			stream.Write( ((bool) val) == true ? "#t" : "#f");
		} else if(val is int || val is uint) {
			stream.Write(val.ToString());
		} else if(val is float || val is double) {
			string num = String.Format(CultureInfo.InvariantCulture, "{0:G}", val);
			stream.Write(num);
		} else if(val is string) {
			stream.Write("\"");
			foreach(char c in val.ToString()) {
				if(c == '\"')
					stream.Write("\\\"");
				else if(c == '\\')
					stream.Write("\\\\");
				else
					stream.Write(c);
			}
			stream.Write("\"");
		} else if(val is Lisp.Symbol) {
			stream.Write(((Lisp.Symbol)val).Name);
		} else if(val is Lisp.List) {
			IndentDepth += 2;
			indent();
			stream.Write("(");
			bool first = true;
			foreach(object it in (Lisp.List)val) {
				if (!first) {
					stream.Write(" ");
				} else {
					first = false;
				}
				WriteValue(it);
			}
			IndentDepth -= 2;
			stream.Write(")\n");
		} else {
			stream.Write("\"" + val.ToString() + "\"");
		}
	}

	/// <summary>
	/// Indents and write the string passed in <paramref name="line"/>
	/// without any processing of it.
	/// </summary>
	/// <param name="line">The line to write.</param>
	public void WriteVerbatimLine(string line) {
		indent();
		stream.WriteLine(line);
	}

	/// <summary>
	/// Add spaces to indent to <see cref="IndentDepth"/>.
	/// </summary>
	private void indent() {
			stream.Write(new String(' ', IndentDepth));
	}
}

}
