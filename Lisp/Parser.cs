//  $Id$
using System;
using System.Globalization;
using System.IO;
using System.Collections;

namespace Lisp
{
/// <summary>
/// Lisp parser
/// </summary>
/// <seealso cref="Lexer"/>
public sealed class Parser {
	private Lexer Lexer;
	private Lexer.Token Token;

	public Parser(Lexer Lexer) {
		this.Lexer = Lexer;
	}

	public List Parse() {
		Token = Lexer.GetNextToken();
		if(Token != Lexer.Token.OPEN_PAREN)
			ParseError("file does not start with '('");

		List Result = InternParse();
		if(Token != Lexer.Token.EOF) {
			if(Token == Lexer.Token.CLOSE_PAREN)
				ParseError("too many ')'");
			else
				ParseError("extra tokens at end of file");
		}

		return Result;
	}

	private List InternParse() {
		ArrayList Entries = new ArrayList();
		while(Token != Lexer.Token.CLOSE_PAREN && Token != Lexer.Token.EOF) {
			switch(Token) {
				case Lexer.Token.OPEN_PAREN:
					Token = Lexer.GetNextToken();

					if(Token == Lexer.Token.SYMBOL
							&& Lexer.TokenString == "_") {
						Token = Lexer.GetNextToken();
						if(Token != Lexer.Token.STRING)
							ParseError("Expected string after '(_ ' sequence");
						// TODO translate
						Entries.Add(Lexer.TokenString);

						Token = Lexer.GetNextToken();
						if(Token != Lexer.Token.CLOSE_PAREN)
							ParseError("Expected ')' after '(_ \"\"' squence");
						break;
					}

					Entries.Add(InternParse());
					if(Token != Lexer.Token.CLOSE_PAREN)
						ParseError("Expected ')' token, got " + Token);
					break;

				case Lexer.Token.SYMBOL:
					Entries.Add(new Symbol(Lexer.TokenString));
					break;

				case Lexer.Token.STRING:
					Entries.Add(Lexer.TokenString);
					break;

				case Lexer.Token.INTEGER:
					int ival = Int32.Parse(Lexer.TokenString, NumberStyles.Integer, NumberFormatInfo.InvariantInfo);
					Entries.Add(ival);
					break;

				case Lexer.Token.REAL:
					float fval = Single.Parse(Lexer.TokenString, NumberStyles.Float, NumberFormatInfo.InvariantInfo);
					Entries.Add(fval);
					break;

				case Lexer.Token.TRUE:
					Entries.Add(true);
					break;

				case Lexer.Token.FALSE:
					Entries.Add(false);
					break;

				default:
					ParseError("Unexpected Token " + Token);
					break;
			}

			Token = Lexer.GetNextToken();
		}

		return new List(Entries.ToArray());
	}

	private void ParseError(string Message) {
		throw new LispException("Parse error in line "
		                        + Lexer.LineNumber + ": " + Message);
	}
}

}
