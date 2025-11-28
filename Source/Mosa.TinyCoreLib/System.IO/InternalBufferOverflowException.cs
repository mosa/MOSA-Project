using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.IO;

public class InternalBufferOverflowException : SystemException
{
	public InternalBufferOverflowException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected InternalBufferOverflowException(SerializationInfo info, StreamingContext context)
	{
	}

	public InternalBufferOverflowException(string? message)
	{
	}

	public InternalBufferOverflowException(string? message, Exception? inner)
	{
	}
}
