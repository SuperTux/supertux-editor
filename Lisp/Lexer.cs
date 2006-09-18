using System;
using System.Text;
using System.IO;

namespace Lisp {

public sealed class Lexer {
	private TextReader stream;
	private char[] buffer;
	private char c;
	int bufpos;
	int buflen;

	public class EOFException : LispException {
	};

	public enum Token {
		EOF,
		OPEN_PAREN,
		CLOSE_PAREN,
		SYMBOL,
		STRING,
		INTEGER,
		REAL,
		TRUE,
		FALSE
	};

	private StringBuilder TokenStringBuilder;
	public string TokenString {
		get { return TokenStringBuilder.ToString(); }
	}
	public int LineNumber;

	public Lexer(TextReader stream) {
		this.stream = stream;
		buffer = new char[1025];
		NextChar();
	}

	public Token GetNextToken() {
		try {
			while(Char.IsWhiteSpace(c)) {
				if(c == '\n')
					LineNumber++;
				NextChar();
			}

			TokenStringBuilder = new StringBuilder();

			switch(c) {
				case ';': // comment
					while(true) {
						NextChar();
						if(c == '\n') {
							LineNumber++;
							break;
						}
					}
					NextChar();
					return GetNextToken();
				case '(':
					NextChar();
					return Token.OPEN_PAREN;
				case ')':
					NextChar();
					return Token.CLOSE_PAREN;
				case '"': { // string
					while(true) {
						NextChar();
						if(c == '"')
							break;

						if(c == '\\') {
							NextChar();
							switch(c) {
								case 'n':
									c = '\n';
									break;
								case 't':
									c = '\t';
									break;
							}
						}
						TokenStringBuilder.Append(c);
					}
					NextChar();
					return Token.STRING;
				}
				case '#': // constant
					NextChar();
					while(Char.IsLetterOrDigit(c) || c == '_') {
						TokenStringBuilder.Append(c);
						NextChar();
					}
					if(TokenString == "t")
						return Token.TRUE;
					if(TokenString == "f")
						return Token.FALSE;

					throw new LispException("Unknown constant '"
					                        + TokenString + "'");
				default:
					if(Char.IsDigit(c) || c == '-') {
						bool have_nondigits = false;
						bool have_digits = false;
						int have_floating_point = 0;

						do {
							if(Char.IsDigit(c))
								have_digits = true;
							else if(c == '.')
								have_floating_point++;
							else if(Char.IsLetter(c) || c == '_')
								have_nondigits = true;

							TokenStringBuilder.Append(c);
							NextChar();
						} while(!Char.IsWhiteSpace(c) && c != '\"' && c != '('
						        && c != ')' && c != ';');

						if(have_nondigits || !have_digits
						   || have_floating_point > 1)
							return Token.SYMBOL;
						else if(have_floating_point == 1)
							return Token.REAL;
						else
							return Token.INTEGER;
					} else {
						do {
							TokenStringBuilder.Append(c);
							NextChar();
						} while(!Char.IsWhiteSpace(c) && c != '\"' && c != '('
						        && c != ')' && c != ';');

						return Token.SYMBOL;
					}
			}
		} catch (EOFException) {
			return Token.EOF;
		}
	}

	private void NextChar() {
		if(bufpos >= buflen) {
			buflen = stream.Read(buffer, 0, 1024);
			if(buflen <= 0)
				throw new EOFException();
			bufpos = 0;
			// following hack appends an additional ' ' at the end of the file
			// to avoid problems when parsing symbols/elements and a sudden EOF:
			// This way we can avoid the need for an unget function.
			if(stream.Peek() < 0) {
				buffer[buflen] = ' ';
				++buflen;
			}
		}
		c = buffer[bufpos++];
	}
}

}
