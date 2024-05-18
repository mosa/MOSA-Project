namespace System;

public interface IObserver<in T>
{
	void OnCompleted();

	void OnError(Exception error);

	void OnNext(T value);
}
