using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Net.Http.Headers;

public class TransferCodingHeaderValue : ICloneable
{
	public ICollection<NameValueHeaderValue> Parameters
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

	protected TransferCodingHeaderValue(TransferCodingHeaderValue source)
	{
	}

	public TransferCodingHeaderValue(string value)
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

	public static TransferCodingHeaderValue Parse(string input)
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

	public static bool TryParse([NotNullWhen(true)] string? input, [NotNullWhen(true)] out TransferCodingHeaderValue? parsedValue)
	{
		throw null;
	}
}
