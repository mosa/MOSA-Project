using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Runtime.InteropServices;

public class SafeArrayTypeMismatchException : SystemException
{
	public SafeArrayTypeMismatchException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected SafeArrayTypeMismatchException(SerializationInfo info, StreamingContext context)
	{
	}

	public SafeArrayTypeMismatchException(string? message)
	{
	}

	public SafeArrayTypeMismatchException(string? message, Exception? inner)
	{
	}
}
