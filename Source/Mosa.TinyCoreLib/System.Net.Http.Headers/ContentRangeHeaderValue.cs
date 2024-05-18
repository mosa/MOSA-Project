using System.Diagnostics.CodeAnalysis;

namespace System.Net.Http.Headers;

public class ContentRangeHeaderValue : ICloneable
{
	public long? From
	{
		get
		{
			throw null;
		}
	}

	public bool HasLength
	{
		get
		{
			throw null;
		}
	}

	public bool HasRange
	{
		get
		{
			throw null;
		}
	}

	public long? Length
	{
		get
		{
			throw null;
		}
	}

	public long? To
	{
		get
		{
			throw null;
		}
	}

	public string Unit
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ContentRangeHeaderValue(long length)
	{
	}

	public ContentRangeHeaderValue(long from, long to)
	{
	}

	public ContentRangeHeaderValue(long from, long to, long length)
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

	public static ContentRangeHeaderValue Parse(string input)
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

	public static bool TryParse([NotNullWhen(true)] string? input, [NotNullWhen(true)] out ContentRangeHeaderValue? parsedValue)
	{
		throw null;
	}
}
