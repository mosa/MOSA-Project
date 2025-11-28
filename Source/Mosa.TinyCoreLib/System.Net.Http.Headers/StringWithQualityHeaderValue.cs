using System.Diagnostics.CodeAnalysis;

namespace System.Net.Http.Headers;

public class StringWithQualityHeaderValue : ICloneable
{
	public double? Quality
	{
		get
		{
			throw null;
		}
	}

	public string Value
	{
		get
		{
			throw null;
		}
	}

	public StringWithQualityHeaderValue(string value)
	{
	}

	public StringWithQualityHeaderValue(string value, double quality)
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

	public static StringWithQualityHeaderValue Parse(string input)
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

	public static bool TryParse([NotNullWhen(true)] string? input, [NotNullWhen(true)] out StringWithQualityHeaderValue? parsedValue)
	{
		throw null;
	}
}
