namespace System.Net;

public class CookieContainer
{
	public const int DefaultCookieLengthLimit = 4096;

	public const int DefaultCookieLimit = 300;

	public const int DefaultPerDomainCookieLimit = 20;

	public int Capacity
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int Count
	{
		get
		{
			throw null;
		}
	}

	public int MaxCookieSize
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int PerDomainCapacity
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CookieContainer()
	{
	}

	public CookieContainer(int capacity)
	{
	}

	public CookieContainer(int capacity, int perDomainCapacity, int maxCookieSize)
	{
	}

	public void Add(Cookie cookie)
	{
	}

	public void Add(CookieCollection cookies)
	{
	}

	public void Add(Uri uri, Cookie cookie)
	{
	}

	public void Add(Uri uri, CookieCollection cookies)
	{
	}

	public CookieCollection GetAllCookies()
	{
		throw null;
	}

	public string GetCookieHeader(Uri uri)
	{
		throw null;
	}

	public CookieCollection GetCookies(Uri uri)
	{
		throw null;
	}

	public void SetCookies(Uri uri, string cookieHeader)
	{
	}
}
