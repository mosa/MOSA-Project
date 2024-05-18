using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Net.Http.Headers;

public class MediaTypeHeaderValue : ICloneable
{
	public string? CharSet
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? MediaType
	{
		get
		{
			throw null;
		}
		[param: DisallowNull]
		set
		{
		}
	}

	public ICollection<NameValueHeaderValue> Parameters
	{
		get
		{
			throw null;
		}
	}

	protected MediaTypeHeaderValue(MediaTypeHeaderValue source)
	{
	}

	public MediaTypeHeaderValue(string mediaType)
	{
	}

	public MediaTypeHeaderValue(string mediaType, string? charSet)
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

	public static MediaTypeHeaderValue Parse(string input)
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

	public static bool TryParse([NotNullWhen(true)] string? input, [NotNullWhen(true)] out MediaTypeHeaderValue? parsedValue)
	{
		throw null;
	}
}
