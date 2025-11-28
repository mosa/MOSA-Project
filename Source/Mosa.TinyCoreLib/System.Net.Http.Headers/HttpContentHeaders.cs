using System.Collections.Generic;

namespace System.Net.Http.Headers;

public sealed class HttpContentHeaders : HttpHeaders
{
	public ICollection<string> Allow
	{
		get
		{
			throw null;
		}
	}

	public ContentDispositionHeaderValue? ContentDisposition
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ICollection<string> ContentEncoding
	{
		get
		{
			throw null;
		}
	}

	public ICollection<string> ContentLanguage
	{
		get
		{
			throw null;
		}
	}

	public long? ContentLength
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Uri? ContentLocation
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public byte[]? ContentMD5
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ContentRangeHeaderValue? ContentRange
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public MediaTypeHeaderValue? ContentType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public DateTimeOffset? Expires
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public DateTimeOffset? LastModified
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	internal HttpContentHeaders()
	{
	}
}
