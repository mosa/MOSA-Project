using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Generic;

public sealed class ByteEqualityComparer : EqualityComparer<byte>
{
	public override bool Equals(byte x, byte y)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public override int GetHashCode(byte b)
	{
		throw null;
	}
}
