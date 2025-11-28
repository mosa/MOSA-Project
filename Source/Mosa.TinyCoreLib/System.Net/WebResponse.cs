using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;

namespace System.Net;

public abstract class WebResponse : MarshalByRefObject, IDisposable, ISerializable
{
	public virtual long ContentLength
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual string ContentType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual WebHeaderCollection Headers
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsFromCache
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsMutuallyAuthenticated
	{
		get
		{
			throw null;
		}
	}

	public virtual Uri ResponseUri
	{
		get
		{
			throw null;
		}
	}

	public virtual bool SupportsHeaders
	{
		get
		{
			throw null;
		}
	}

	protected WebResponse()
	{
	}

	[Obsolete("Serialization has been deprecated for WebResponse.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected WebResponse(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}

	public virtual void Close()
	{
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	[Obsolete("Serialization has been deprecated for WebResponse.")]
	protected virtual void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}

	public virtual Stream GetResponseStream()
	{
		throw null;
	}

	[Obsolete("Serialization has been deprecated for WebResponse.")]
	void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}
}
