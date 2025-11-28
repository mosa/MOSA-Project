using System.Diagnostics.CodeAnalysis;

namespace System.Net.Http.Headers;

public class NameValueHeaderValue : ICloneable
{
	public string Name
	{
		get
		{
			throw null;
		}
	}

	public string? Value
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	protected NameValueHeaderValue(NameValueHeaderValue source)
	{
	}

	public NameValueHeaderValue(string name)
	{
	}

	public NameValueHeaderValue(string name, string? value)
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

	public static NameValueHeaderValue Parse(string input)
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

	public static bool TryParse([NotNullWhen(true)] string? input, [NotNullWhen(true)] out NameValueHeaderValue? parsedValue)
	{
		throw null;
	}
}
