using System.Diagnostics.CodeAnalysis;

namespace System.Net.Http.Headers;

public sealed class TransferCodingWithQualityHeaderValue : TransferCodingHeaderValue, ICloneable
{
	public double? Quality
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public TransferCodingWithQualityHeaderValue(string value)
		: base((TransferCodingHeaderValue)null)
	{
	}

	public TransferCodingWithQualityHeaderValue(string value, double quality)
		: base((TransferCodingHeaderValue)null)
	{
	}

	public new static TransferCodingWithQualityHeaderValue Parse(string input)
	{
		throw null;
	}

	object ICloneable.Clone()
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? input, [NotNullWhen(true)] out TransferCodingWithQualityHeaderValue? parsedValue)
	{
		throw null;
	}
}
