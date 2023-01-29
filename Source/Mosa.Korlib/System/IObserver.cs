// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System;

public interface IObserver<in T>
{
	void OnNext(T value);

	void OnError(Exception error);

	void OnCompleted();
}
