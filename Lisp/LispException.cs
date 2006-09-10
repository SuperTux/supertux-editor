using System;

namespace Lisp {

	public class LispException : Exception {
		public LispException() : base() { }
		public LispException(string message) : base(message) { }
		public LispException(string message, Exception innerException) : base(message, innerException) { }
	}

}