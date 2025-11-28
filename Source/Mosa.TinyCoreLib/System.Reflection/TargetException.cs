using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Reflection;

public class TargetException : ApplicationException
{
	public TargetException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected TargetException(SerializationInfo info, StreamingContext context)
	{
	}

	public TargetException(string? message)
	{
	}

	public TargetException(string? message, Exception? inner)
	{
	}
}
