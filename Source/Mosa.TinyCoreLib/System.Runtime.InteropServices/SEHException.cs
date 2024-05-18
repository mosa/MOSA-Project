using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Runtime.InteropServices;

public class SEHException : ExternalException
{
	public SEHException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected SEHException(SerializationInfo info, StreamingContext context)
	{
	}

	public SEHException(string? message)
	{
	}

	public SEHException(string? message, Exception? inner)
	{
	}

	public virtual bool CanResume()
	{
		throw null;
	}
}
