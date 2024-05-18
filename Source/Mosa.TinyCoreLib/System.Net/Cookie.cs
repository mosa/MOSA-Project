using System.Diagnostics.CodeAnalysis;

namespace System.Net;

public sealed class Cookie
{
	public string Comment
	{
		get
		{
			throw null;
		}
		[param: AllowNull]
		set
		{
		}
	}

	public Uri? CommentUri
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool Discard
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Domain
	{
		get
		{
			throw null;
		}
		[param: AllowNull]
		set
		{
		}
	}

	public bool Expired
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public DateTime Expires
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool HttpOnly
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Name
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Path
	{
		get
		{
			throw null;
		}
		[param: AllowNull]
		set
		{
		}
	}

	public string Port
	{
		get
		{
			throw null;
		}
		[param: AllowNull]
		set
		{
		}
	}

	public bool Secure
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public DateTime TimeStamp
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
		[param: AllowNull]
		set
		{
		}
	}

	public int Version
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Cookie()
	{
	}

	public Cookie(string name, string? value)
	{
	}

	public Cookie(string name, string? value, string? path)
	{
	}

	public Cookie(string name, string? value, string? path, string? domain)
	{
	}

	public override bool Equals([NotNullWhen(true)] object? comparand)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
