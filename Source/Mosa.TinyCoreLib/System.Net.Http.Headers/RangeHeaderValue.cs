using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Net.Http.Headers;

public class RangeHeaderValue : ICloneable
{
	public ICollection<RangeItemHeaderValue> Ranges
	{
		get
		{
			throw null;
		}
	}

	public string Unit
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public RangeHeaderValue()
	{
	}

	public RangeHeaderValue(long? from, long? to)
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

	public static RangeHeaderValue Parse(string input)
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

	public static bool TryParse([NotNullWhen(true)] string? input, [NotNullWhen(true)] out RangeHeaderValue? parsedValue)
	{
		throw null;
	}
}
