using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Runtime.InteropServices;

public class InvalidComObjectException : SystemException
{
	public InvalidComObjectException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected InvalidComObjectException(SerializationInfo info, StreamingContext context)
	{
	}

	public InvalidComObjectException(string? message)
	{
	}

	public InvalidComObjectException(string? message, Exception? inner)
	{
	}
}
