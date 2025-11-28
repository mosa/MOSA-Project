using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace System;

public class Lazy<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>
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
		get
		{
			throw null;
		}
	}

	public Lazy()
	{
	}

	public Lazy(bool isThreadSafe)
	{
	}

	public Lazy(Func<T> valueFactory)
	{
	}

	public Lazy(Func<T> valueFactory, bool isThreadSafe)
	{
	}

	public Lazy(Func<T> valueFactory, LazyThreadSafetyMode mode)
	{
	}

	public Lazy(LazyThreadSafetyMode mode)
	{
	}

	public Lazy(T value)
	{
	}

	public override string? ToString()
	{
		throw null;
	}
}
public class Lazy<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T, TMetadata> : Lazy<T>
{
	public TMetadata Metadata
	{
		get
		{
			throw null;
		}
	}

	public Lazy(Func<T> valueFactory, TMetadata metadata)
	{
	}

	public Lazy(Func<T> valueFactory, TMetadata metadata, bool isThreadSafe)
	{
	}

	public Lazy(Func<T> valueFactory, TMetadata metadata, LazyThreadSafetyMode mode)
	{
	}

	public Lazy(TMetadata metadata)
	{
	}

	public Lazy(TMetadata metadata, bool isThreadSafe)
	{
	}

	public Lazy(TMetadata metadata, LazyThreadSafetyMode mode)
	{
	}
}
