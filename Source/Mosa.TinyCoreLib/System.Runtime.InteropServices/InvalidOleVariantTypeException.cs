using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Runtime.InteropServices;

public class InvalidOleVariantTypeException : SystemException
{
	public InvalidOleVariantTypeException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected InvalidOleVariantTypeException(SerializationInfo info, StreamingContext context)
	{
	}

	public InvalidOleVariantTypeException(string? message)
	{
	}

	public InvalidOleVariantTypeException(string? message, Exception? inner)
	{
	}
}
