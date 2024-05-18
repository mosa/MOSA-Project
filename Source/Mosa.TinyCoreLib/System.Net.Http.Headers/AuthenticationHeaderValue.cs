using System.Diagnostics.CodeAnalysis;

namespace System.Net.Http.Headers;

public class AuthenticationHeaderValue : ICloneable
{
	public string? Parameter
	{
		get
		{
			throw null;
		}
	}

	public string Scheme
	{
		get
		{
			throw null;
		}
	}

	public AuthenticationHeaderValue(string scheme)
	{
	}

	public AuthenticationHeaderValue(string scheme, string? parameter)
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

	public static AuthenticationHeaderValue Parse(string input)
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

	public static bool TryParse([NotNullWhen(true)] string? input, [NotNullWhen(true)] out AuthenticationHeaderValue? parsedValue)
	{
		throw null;
	}
}
