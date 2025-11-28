using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace System.Threading;

public readonly struct CancellationTokenRegistration : IAsyncDisposable, IDisposable, IEquatable<CancellationTokenRegistration>
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public CancellationToken Token
	{
		get
		{
			throw null;
		}
	}

	public void Dispose()
	{
	}

	public ValueTask DisposeAsync()
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals(CancellationTokenRegistration other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(CancellationTokenRegistration left, CancellationTokenRegistration right)
	{
		throw null;
	}

	public static bool operator !=(CancellationTokenRegistration left, CancellationTokenRegistration right)
	{
		throw null;
	}

	public bool Unregister()
	{
		throw null;
	}
}
