using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Threading;

public class ThreadLocal<T> : IDisposable
{
	public bool IsValueCreated
	{
		get
		{
			throw null;
		}
	}

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

	public IList<T> Values
	{
		get
		{
			throw null;
		}
	}

	public ThreadLocal()
	{
	}

	public ThreadLocal(bool trackAllValues)
	{
	}

	public ThreadLocal(Func<T> valueFactory)
	{
	}

	public ThreadLocal(Func<T> valueFactory, bool trackAllValues)
	{
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	~ThreadLocal()
	{
	}

	public override string? ToString()
	{
		throw null;
	}
}
