using System;

namespace System.System
{
	public interface IObserver<in T>
	{
		void OnNext(T value);
		void OnError(Exception error);
		void OnCompleted();
	}
}
