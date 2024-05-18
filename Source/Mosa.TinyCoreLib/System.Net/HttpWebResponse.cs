using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;

namespace System.Net;

public class HttpWebResponse : WebResponse, ISerializable
{
	public string? CharacterSet
	{
		get
		{
			throw null;
		}
	}

	public string ContentEncoding
	{
		get
		{
			throw null;
		}
	}

	public override long ContentLength
	{
		get
		{
			throw null;
		}
	}

	public override string ContentType
	{
		get
		{
			throw null;
		}
	}

	public virtual CookieCollection Cookies
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override WebHeaderCollection Headers
	{
		get
		{
			throw null;
		}
	}

	public override bool IsMutuallyAuthenticated
	{
		get
		{
			throw null;
		}
	}

	public DateTime LastModified
	{
		get
		{
			throw null;
		}
	}

	public virtual string Method
	{
		get
		{
			throw null;
		}
	}

	public Version ProtocolVersion
	{
		get
		{
			throw null;
		}
	}

	public override Uri ResponseUri
	{
		get
		{
			throw null;
		}
	}

	public string Server
	{
		get
		{
			throw null;
		}
	}

	public virtual HttpStatusCode StatusCode
	{
		get
		{
			throw null;
		}
	}

	public virtual string StatusDescription
	{
		get
		{
			throw null;
		}
	}

	public override bool SupportsHeaders
	{
		get
		{
			throw null;
		}
	}

	[Obsolete("This API supports the .NET infrastructure and is not intended to be used directly from your code.", true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public HttpWebResponse()
	{
	}

	[Obsolete("Serialization has been deprecated for HttpWebResponse.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected HttpWebResponse(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}

	public override void Close()
	{
	}

	protected override void Dispose(bool disposing)
	{
	}

	[Obsolete("Serialization has been deprecated for HttpWebResponse.")]
	protected override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}

	public string GetResponseHeader(string headerName)
	{
		throw null;
	}

	public override Stream GetResponseStream()
	{
		throw null;
	}

	[Obsolete("Serialization has been deprecated for HttpWebResponse.")]
	void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}
}
