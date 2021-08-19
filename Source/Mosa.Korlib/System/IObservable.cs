// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System
{
	public interface IObservable<out T>
	{
		IDisposable Subscribe(IObserver<T> observer);
	}
}
