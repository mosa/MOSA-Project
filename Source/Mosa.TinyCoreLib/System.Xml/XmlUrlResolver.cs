using System.Net;
using System.Net.Cache;
using System.Runtime.Versioning;
using System.Threading.Tasks;

namespace System.Xml;

public class XmlUrlResolver : XmlResolver
{
	public RequestCachePolicy CachePolicy
	{
		set
		{
		}
	}

	[UnsupportedOSPlatform("browser")]
	public override ICredentials? Credentials
	{
		set
		{
		}
	}

	[UnsupportedOSPlatform("browser")]
	public IWebProxy? Proxy
	{
		set
		{
		}
	}

	public override object? GetEntity(Uri absoluteUri, string? role, Type? ofObjectToReturn)
	{
		throw null;
	}

	public override Task<object> GetEntityAsync(Uri absoluteUri, string? role, Type? ofObjectToReturn)
	{
		throw null;
	}

	public override Uri ResolveUri(Uri? baseUri, string? relativeUri)
	{
		throw null;
	}
}
