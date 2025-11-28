using System.ComponentModel;
using System.Runtime.Serialization;

namespace System;

public class UriFormatException : FormatException, ISerializable
{
	public UriFormatException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected UriFormatException(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}

	public UriFormatException(string? textString)
	{
	}

	public UriFormatException(string? textString, Exception? e)
	{
	}

	void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}
}
