using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Reflection;

public class CustomAttributeFormatException : FormatException
{
	public CustomAttributeFormatException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected CustomAttributeFormatException(SerializationInfo info, StreamingContext context)
	{
	}

	public CustomAttributeFormatException(string? message)
	{
	}

	public CustomAttributeFormatException(string? message, Exception? inner)
	{
	}
}
