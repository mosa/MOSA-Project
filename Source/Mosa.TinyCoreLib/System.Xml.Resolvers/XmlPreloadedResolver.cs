using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace System.Xml.Resolvers;

public class XmlPreloadedResolver : XmlResolver
{
	public override ICredentials Credentials
	{
		set
		{
		}
	}

	public IEnumerable<Uri> PreloadedUris
	{
		get
		{
			throw null;
		}
	}

	public XmlPreloadedResolver()
	{
	}

	public XmlPreloadedResolver(XmlKnownDtds preloadedDtds)
	{
	}

	public XmlPreloadedResolver(XmlResolver? fallbackResolver)
	{
	}

	public XmlPreloadedResolver(XmlResolver? fallbackResolver, XmlKnownDtds preloadedDtds)
	{
	}

	public XmlPreloadedResolver(XmlResolver? fallbackResolver, XmlKnownDtds preloadedDtds, IEqualityComparer<Uri>? uriComparer)
	{
	}

	public void Add(Uri uri, byte[] value)
	{
	}

	public void Add(Uri uri, byte[] value, int offset, int count)
	{
	}

	public void Add(Uri uri, Stream value)
	{
	}

	public void Add(Uri uri, string value)
	{
	}

	public override object? GetEntity(Uri absoluteUri, string? role, Type? ofObjectToReturn)
	{
		throw null;
	}

	public override Task<object> GetEntityAsync(Uri absoluteUri, string? role, Type? ofObjectToReturn)
	{
		throw null;
	}

	public void Remove(Uri uri)
	{
	}

	public override Uri ResolveUri(Uri? baseUri, string? relativeUri)
	{
		throw null;
	}

	public override bool SupportsType(Uri absoluteUri, Type? type)
	{
		throw null;
	}
}
