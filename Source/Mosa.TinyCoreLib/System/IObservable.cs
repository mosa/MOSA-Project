namespace System;

public interface IObservable<out T>
{
	IDisposable Subscribe(IObserver<T> observer);
}
