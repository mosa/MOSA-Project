using System.Diagnostics.CodeAnalysis;

namespace System.Net.Http.Headers;

public class RangeItemHeaderValue : ICloneable
{
	public long? From
	{
		get
		{
			throw null;
		}
	}

	public long? To
	{
		get
		{
			throw null;
		}
	}

	public RangeItemHeaderValue(long? from, long? to)
	{
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	object ICloneable.Clone()
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
