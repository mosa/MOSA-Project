using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace System.Net;

public class FileWebRequest : WebRequest, ISerializable
{
	public override string? ConnectionGroupName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override long ContentLength
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override string? ContentType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override ICredentials? Credentials
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

	public override string Method
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override bool PreAuthenticate
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override IWebProxy? Proxy
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override Uri RequestUri
	{
		get
		{
			throw null;
		}
	}

	public override int Timeout
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override bool UseDefaultCredentials
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[Obsolete("Serialization has been deprecated for FileWebRequest.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected FileWebRequest(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}

	public override void Abort()
	{
	}

	public override IAsyncResult BeginGetRequestStream(AsyncCallback? callback, object? state)
	{
		throw null;
	}

	public override IAsyncResult BeginGetResponse(AsyncCallback? callback, object? state)
	{
		throw null;
	}

	public override Stream EndGetRequestStream(IAsyncResult asyncResult)
	{
		throw null;
	}

	public override WebResponse EndGetResponse(IAsyncResult asyncResult)
	{
		throw null;
	}

	[Obsolete("Serialization has been deprecated for FileWebRequest.")]
	protected override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}

	public override Stream GetRequestStream()
	{
		throw null;
	}

	public override Task<Stream> GetRequestStreamAsync()
	{
		throw null;
	}

	public override WebResponse GetResponse()
	{
		throw null;
	}

	public override Task<WebResponse> GetResponseAsync()
	{
		throw null;
	}

	[Obsolete("Serialization has been deprecated for FileWebRequest.")]
	void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}
}
