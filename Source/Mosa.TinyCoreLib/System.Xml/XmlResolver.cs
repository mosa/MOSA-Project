using System.Net;
using System.Threading.Tasks;

namespace System.Xml;

public abstract class XmlResolver
{
	public virtual ICredentials Credentials
	{
		set
		{
		}
	}

	public static XmlResolver ThrowingResolver
	{
		get
		{
			throw null;
		}
	}

	public static XmlResolver FileSystemResolver
	{
		get
		{
			throw null;
		}
	}

	public abstract object? GetEntity(Uri absoluteUri, string? role, Type? ofObjectToReturn);

	public virtual Task<object> GetEntityAsync(Uri absoluteUri, string? role, Type? ofObjectToReturn)
	{
		throw null;
	}

	public virtual Uri ResolveUri(Uri? baseUri, string? relativeUri)
	{
		throw null;
	}

	public virtual bool SupportsType(Uri absoluteUri, Type? type)
	{
		throw null;
	}
}
