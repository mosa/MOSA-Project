using System.Diagnostics.CodeAnalysis;

namespace System.Threading;

public sealed class AsyncLocal<T>
{
	public T Value
	{
		[return: MaybeNull]
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public AsyncLocal()
	{
	}

	public AsyncLocal(Action<AsyncLocalValueChangedArgs<T>>? valueChangedHandler)
	{
	}
}
