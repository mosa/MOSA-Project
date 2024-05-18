using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Runtime.InteropServices;

public class COMException : ExternalException
{
	public COMException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected COMException(SerializationInfo info, StreamingContext context)
	{
	}

	public COMException(string? message)
	{
	}

	public COMException(string? message, Exception? inner)
	{
	}

	public COMException(string? message, int errorCode)
	{
	}

	public override string ToString()
	{
		throw null;
	}
}
