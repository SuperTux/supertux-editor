using Lisp;

namespace Lisp
{

	public interface ICustomLispSerializer {
		void CustomLispRead(Properties Props);
		void CustomLispWrite(Writer Writer);
		void FinishRead();
	}

	public interface ILispSerializer {
		object Read(List list);
		void Write(Writer writer, string name, object Object);
	}

}
