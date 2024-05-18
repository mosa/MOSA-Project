using System.Diagnostics.CodeAnalysis;

namespace System.Net.Http.Headers;

public class RangeConditionHeaderValue : ICloneable
{
	public DateTimeOffset? Date
	{
		get
		{
			throw null;
		}
	}

	public EntityTagHeaderValue? EntityTag
	{
		get
		{
			throw null;
		}
	}

	public RangeConditionHeaderValue(DateTimeOffset date)
	{
	}

	public RangeConditionHeaderValue(EntityTagHeaderValue entityTag)
	{
	}

	public RangeConditionHeaderValue(string entityTag)
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

	public static RangeConditionHeaderValue Parse(string input)
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

	public static bool TryParse([NotNullWhen(true)] string? input, [NotNullWhen(true)] out RangeConditionHeaderValue? parsedValue)
	{
		throw null;
	}
}
