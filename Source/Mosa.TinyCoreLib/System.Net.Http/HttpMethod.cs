using System.Diagnostics.CodeAnalysis;

namespace System.Net.Http;

public class HttpMethod : IEquatable<HttpMethod>
{
	public static HttpMethod Delete
	{
		get
		{
			throw null;
		}
	}

	public static HttpMethod Get
	{
		get
		{
			throw null;
		}
	}

	public static HttpMethod Head
	{
		get
		{
			throw null;
		}
	}

	public string Method
	{
		get
		{
			throw null;
		}
	}

	public static HttpMethod Options
	{
		get
		{
			throw null;
		}
	}

	public static HttpMethod Patch
	{
		get
		{
			throw null;
		}
	}

	public static HttpMethod Post
	{
		get
		{
			throw null;
		}
	}

	public static HttpMethod Put
	{
		get
		{
			throw null;
		}
	}

	public static HttpMethod Trace
	{
		get
		{
			throw null;
		}
	}

	public static HttpMethod Connect
	{
		get
		{
			throw null;
		}
	}

	public HttpMethod(string method)
	{
	}

	public bool Equals([NotNullWhen(true)] HttpMethod? other)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(HttpMethod? left, HttpMethod? right)
	{
		throw null;
	}

	public static bool operator !=(HttpMethod? left, HttpMethod? right)
	{
		throw null;
	}

	public static HttpMethod Parse(ReadOnlySpan<char> method)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
