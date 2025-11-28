using System.Diagnostics.CodeAnalysis;

namespace System.Diagnostics;

public readonly struct ActivitySpanId : IEquatable<ActivitySpanId>
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public void CopyTo(Span<byte> destination)
	{
	}

	public static ActivitySpanId CreateFromBytes(ReadOnlySpan<byte> idData)
	{
		throw null;
	}

	public static ActivitySpanId CreateFromString(ReadOnlySpan<char> idData)
	{
		throw null;
	}

	public static ActivitySpanId CreateFromUtf8String(ReadOnlySpan<byte> idData)
	{
		throw null;
	}

	public static ActivitySpanId CreateRandom()
	{
		throw null;
	}

	public bool Equals(ActivitySpanId spanId)
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

	public static bool operator ==(ActivitySpanId spanId1, ActivitySpanId spandId2)
	{
		throw null;
	}

	public static bool operator !=(ActivitySpanId spanId1, ActivitySpanId spandId2)
	{
		throw null;
	}

	public string ToHexString()
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
