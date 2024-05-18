using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.IO;

public class IOException : SystemException
{
	public IOException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected IOException(SerializationInfo info, StreamingContext context)
	{
	}

	public IOException(string? message)
	{
	}

	public IOException(string? message, Exception? innerException)
	{
	}

	public IOException(string? message, int hresult)
	{
	}
}
