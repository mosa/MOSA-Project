using System.Diagnostics.CodeAnalysis;

namespace System;

public class UriBuilder
{
	public string Fragment
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

	public string Host
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

	public string Password
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

	public int Port
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Query
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

	public string Scheme
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

	public Uri Uri
	{
		get
		{
			throw null;
		}
	}

	public string UserName
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

	public UriBuilder()
	{
	}

	public UriBuilder([StringSyntax("Uri")] string uri)
	{
	}

	public UriBuilder(string? schemeName, string? hostName)
	{
	}

	public UriBuilder(string? scheme, string? host, int portNumber)
	{
	}

	public UriBuilder(string? scheme, string? host, int port, string? pathValue)
	{
	}

	public UriBuilder(string? scheme, string? host, int port, string? path, string? extraValue)
	{
	}

	public UriBuilder(Uri uri)
	{
	}

	public override bool Equals([NotNullWhen(true)] object? rparam)
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
