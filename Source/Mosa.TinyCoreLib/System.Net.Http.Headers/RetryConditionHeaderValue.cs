using System.Diagnostics.CodeAnalysis;

namespace System.Net.Http.Headers;

public class RetryConditionHeaderValue : ICloneable
{
	public DateTimeOffset? Date
	{
		get
		{
			throw null;
		}
	}

	public TimeSpan? Delta
	{
		get
		{
			throw null;
		}
	}

	public RetryConditionHeaderValue(DateTimeOffset date)
	{
	}

	public RetryConditionHeaderValue(TimeSpan delta)
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

	public static RetryConditionHeaderValue Parse(string input)
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

	public static bool TryParse([NotNullWhen(true)] string? input, [NotNullWhen(true)] out RetryConditionHeaderValue? parsedValue)
	{
		throw null;
	}
}
