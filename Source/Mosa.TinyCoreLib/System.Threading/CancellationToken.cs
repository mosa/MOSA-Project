using System.Diagnostics.CodeAnalysis;

namespace System.Threading;

public readonly struct CancellationToken : IEquatable<CancellationToken>
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public bool CanBeCanceled
	{
		get
		{
			throw null;
		}
	}

	public bool IsCancellationRequested
	{
		get
		{
			throw null;
		}
	}

	public static CancellationToken None
	{
		get
		{
			throw null;
		}
	}

	public WaitHandle WaitHandle
	{
		get
		{
			throw null;
		}
	}

	public CancellationToken(bool canceled)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? other)
	{
		throw null;
	}

	public bool Equals(CancellationToken other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(CancellationToken left, CancellationToken right)
	{
		throw null;
	}

	public static bool operator !=(CancellationToken left, CancellationToken right)
	{
		throw null;
	}

	public CancellationTokenRegistration Register(Action callback)
	{
		throw null;
	}

	public CancellationTokenRegistration Register(Action callback, bool useSynchronizationContext)
	{
		throw null;
	}

	public CancellationTokenRegistration Register(Action<object?, CancellationToken> callback, object? state)
	{
		throw null;
	}

	public CancellationTokenRegistration Register(Action<object?> callback, object? state)
	{
		throw null;
	}

	public CancellationTokenRegistration Register(Action<object?> callback, object? state, bool useSynchronizationContext)
	{
		throw null;
	}

	public void ThrowIfCancellationRequested()
	{
	}

	public CancellationTokenRegistration UnsafeRegister(Action<object?, CancellationToken> callback, object? state)
	{
		throw null;
	}

	public CancellationTokenRegistration UnsafeRegister(Action<object?> callback, object? state)
	{
		throw null;
	}
}
