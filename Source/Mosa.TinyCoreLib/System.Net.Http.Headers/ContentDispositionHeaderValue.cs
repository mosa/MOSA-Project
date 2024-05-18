using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Net.Http.Headers;

public class ContentDispositionHeaderValue : ICloneable
{
	public DateTimeOffset? CreationDate
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string DispositionType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? FileName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? FileNameStar
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public DateTimeOffset? ModificationDate
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? Name
	{
		get
		{
			throw null;
		}
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

	public DateTimeOffset? ReadDate
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public long? Size
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	protected ContentDispositionHeaderValue(ContentDispositionHeaderValue source)
	{
	}

	public ContentDispositionHeaderValue(string dispositionType)
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

	public static ContentDispositionHeaderValue Parse(string input)
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

	public static bool TryParse([NotNullWhen(true)] string? input, [NotNullWhen(true)] out ContentDispositionHeaderValue? parsedValue)
	{
		throw null;
	}
}
