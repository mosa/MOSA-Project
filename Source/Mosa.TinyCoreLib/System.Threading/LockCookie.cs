using System.Diagnostics.CodeAnalysis;

namespace System.Threading;

public struct LockCookie : IEquatable<LockCookie>
{
	private int _dummyPrimitive;

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals(LockCookie obj)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(LockCookie a, LockCookie b)
	{
		throw null;
	}

	public static bool operator !=(LockCookie a, LockCookie b)
	{
		throw null;
	}
}
