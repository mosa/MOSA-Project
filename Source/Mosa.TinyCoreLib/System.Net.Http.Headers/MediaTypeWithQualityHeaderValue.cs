using System.Diagnostics.CodeAnalysis;

namespace System.Net.Http.Headers;

public sealed class MediaTypeWithQualityHeaderValue : MediaTypeHeaderValue, ICloneable
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

	public MediaTypeWithQualityHeaderValue(string mediaType)
		: base((MediaTypeHeaderValue)null)
	{
	}

	public MediaTypeWithQualityHeaderValue(string mediaType, double quality)
		: base((MediaTypeHeaderValue)null)
	{
	}

	public new static MediaTypeWithQualityHeaderValue Parse(string input)
	{
		throw null;
	}

	object ICloneable.Clone()
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? input, [NotNullWhen(true)] out MediaTypeWithQualityHeaderValue? parsedValue)
	{
		throw null;
	}
}
