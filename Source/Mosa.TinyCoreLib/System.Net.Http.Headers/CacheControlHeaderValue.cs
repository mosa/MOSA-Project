using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Net.Http.Headers;

public class CacheControlHeaderValue : ICloneable
{
	public ICollection<NameValueHeaderValue> Extensions
	{
		get
		{
			throw null;
		}
	}

	public TimeSpan? MaxAge
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool MaxStale
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public TimeSpan? MaxStaleLimit
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public TimeSpan? MinFresh
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool MustRevalidate
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool NoCache
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ICollection<string> NoCacheHeaders
	{
		get
		{
			throw null;
		}
	}

	public bool NoStore
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool NoTransform
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool OnlyIfCached
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool Private
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ICollection<string> PrivateHeaders
	{
		get
		{
			throw null;
		}
	}

	public bool ProxyRevalidate
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool Public
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public TimeSpan? SharedMaxAge
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static CacheControlHeaderValue Parse(string? input)
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

	public static bool TryParse(string? input, [NotNullWhen(true)] out CacheControlHeaderValue? parsedValue)
	{
		throw null;
	}
}
