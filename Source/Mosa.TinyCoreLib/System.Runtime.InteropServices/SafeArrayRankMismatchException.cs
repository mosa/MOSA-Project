using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Runtime.InteropServices;

public class SafeArrayRankMismatchException : SystemException
{
	public SafeArrayRankMismatchException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected SafeArrayRankMismatchException(SerializationInfo info, StreamingContext context)
	{
	}

	public SafeArrayRankMismatchException(string? message)
	{
	}

	public SafeArrayRankMismatchException(string? message, Exception? inner)
	{
	}
}
