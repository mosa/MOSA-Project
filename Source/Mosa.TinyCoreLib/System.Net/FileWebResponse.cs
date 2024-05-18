using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;

namespace System.Net;

public class FileWebResponse : WebResponse, ISerializable
{
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

	public override WebHeaderCollection Headers
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

	public override bool SupportsHeaders
	{
		get
		{
			throw null;
		}
	}

	[Obsolete("Serialization has been deprecated for FileWebResponse.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected FileWebResponse(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}

	public override void Close()
	{
	}

	[Obsolete("Serialization has been deprecated for FileWebResponse.")]
	protected override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}

	public override Stream GetResponseStream()
	{
		throw null;
	}

	[Obsolete("Serialization has been deprecated for FileWebResponse.")]
	void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}
}
