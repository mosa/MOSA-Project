using System.Diagnostics.CodeAnalysis;

namespace System.Net.Http.Headers;

public class WarningHeaderValue : ICloneable
{
	public string Agent
	{
		get
		{
			throw null;
		}
	}

	public int Code
	{
		get
		{
			throw null;
		}
	}

	public DateTimeOffset? Date
	{
		get
		{
			throw null;
		}
	}

	public string Text
	{
		get
		{
			throw null;
		}
	}

	public WarningHeaderValue(int code, string agent, string text)
	{
	}

	public WarningHeaderValue(int code, string agent, string text, DateTimeOffset date)
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

	public static WarningHeaderValue Parse(string input)
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

	public static bool TryParse([NotNullWhen(true)] string? input, [NotNullWhen(true)] out WarningHeaderValue? parsedValue)
	{
		throw null;
	}
}
