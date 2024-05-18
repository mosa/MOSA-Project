using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Resources;

[EditorBrowsable(EditorBrowsableState.Never)]
public class MissingManifestResourceException : SystemException
{
	public MissingManifestResourceException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	protected MissingManifestResourceException(SerializationInfo info, StreamingContext context)
	{
	}

	public MissingManifestResourceException(string? message)
	{
	}

	public MissingManifestResourceException(string? message, Exception? inner)
	{
	}
}
