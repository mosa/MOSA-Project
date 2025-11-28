using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Runtime.InteropServices;

public class ExternalException : SystemException
{
	public virtual int ErrorCode
	{
		get
		{
			throw null;
		}
	}

	public ExternalException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected ExternalException(SerializationInfo info, StreamingContext context)
	{
	}

	public ExternalException(string? message)
	{
	}

	public ExternalException(string? message, Exception? inner)
	{
	}

	public ExternalException(string? message, int errorCode)
	{
	}

	public override string ToString()
	{
		throw null;
	}
}
