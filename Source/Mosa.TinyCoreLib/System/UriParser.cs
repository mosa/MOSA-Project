namespace System;

public abstract class UriParser
{
	protected virtual string GetComponents(Uri uri, UriComponents components, UriFormat format)
	{
		throw null;
	}

	protected virtual void InitializeAndValidate(Uri uri, out UriFormatException? parsingError)
	{
		throw null;
	}

	protected virtual bool IsBaseOf(Uri baseUri, Uri relativeUri)
	{
		throw null;
	}

	public static bool IsKnownScheme(string schemeName)
	{
		throw null;
	}

	protected virtual bool IsWellFormedOriginalString(Uri uri)
	{
		throw null;
	}

	protected virtual UriParser OnNewUri()
	{
		throw null;
	}

	protected virtual void OnRegister(string schemeName, int defaultPort)
	{
	}

	public static void Register(UriParser uriParser, string schemeName, int defaultPort)
	{
	}

	protected virtual string? Resolve(Uri baseUri, Uri? relativeUri, out UriFormatException? parsingError)
	{
		throw null;
	}
}
