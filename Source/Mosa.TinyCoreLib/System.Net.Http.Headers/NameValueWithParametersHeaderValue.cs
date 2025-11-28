using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Net.Http.Headers;

public class NameValueWithParametersHeaderValue : NameValueHeaderValue, ICloneable
{
	public ICollection<NameValueHeaderValue> Parameters
	{
		get
		{
			throw null;
		}
	}

	protected NameValueWithParametersHeaderValue(NameValueWithParametersHeaderValue source)
		: base((string)null)
	{
	}

	public NameValueWithParametersHeaderValue(string name)
		: base((string)null)
	{
	}

	public NameValueWithParametersHeaderValue(string name, string? value)
		: base((string)null)
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

	public new static NameValueWithParametersHeaderValue Parse(string input)
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

	public static bool TryParse([NotNullWhen(true)] string? input, [NotNullWhen(true)] out NameValueWithParametersHeaderValue? parsedValue)
	{
		throw null;
	}
}
