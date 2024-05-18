using System.Diagnostics.CodeAnalysis;

namespace System.Threading;

public struct AsyncFlowControl : IEquatable<AsyncFlowControl>, IDisposable
{
	private object _dummy;

	private int _dummyPrimitive;

	public void Dispose()
	{
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals(AsyncFlowControl obj)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(AsyncFlowControl a, AsyncFlowControl b)
	{
		throw null;
	}

	public static bool operator !=(AsyncFlowControl a, AsyncFlowControl b)
	{
		throw null;
	}

	public void Undo()
	{
	}
}
