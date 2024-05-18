using System.Diagnostics.CodeAnalysis;

namespace System.Diagnostics;

public readonly struct ActivityTraceId : IEquatable<ActivityTraceId>
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public void CopyTo(Span<byte> destination)
	{
	}

	public static ActivityTraceId CreateFromBytes(ReadOnlySpan<byte> idData)
	{
		throw null;
	}

	public static ActivityTraceId CreateFromString(ReadOnlySpan<char> idData)
	{
		throw null;
	}

	public static ActivityTraceId CreateFromUtf8String(ReadOnlySpan<byte> idData)
	{
		throw null;
	}

	public static ActivityTraceId CreateRandom()
	{
		throw null;
	}

	public bool Equals(ActivityTraceId traceId)
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

	public static bool operator ==(ActivityTraceId traceId1, ActivityTraceId traceId2)
	{
		throw null;
	}

	public static bool operator !=(ActivityTraceId traceId1, ActivityTraceId traceId2)
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
