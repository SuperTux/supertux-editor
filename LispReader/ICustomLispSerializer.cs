using Lisp;

public interface ICustomLispSerializer {
	void CustomLispRead(Properties Props);
	void CustomLispWrite(Writer Writer);
	void FinishRead();
}
