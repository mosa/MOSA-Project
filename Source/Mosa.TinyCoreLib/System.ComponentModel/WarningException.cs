using System.Runtime.Serialization;

namespace System.ComponentModel;

public class WarningException : SystemException
{
	public string? HelpTopic
	{
		get
		{
			throw null;
		}
	}

	public string? HelpUrl
	{
		get
		{
			throw null;
		}
	}

	public WarningException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected WarningException(SerializationInfo info, StreamingContext context)
	{
	}

	public WarningException(string? message)
	{
	}

	public WarningException(string? message, Exception? innerException)
	{
	}

	public WarningException(string? message, string? helpUrl)
	{
	}

	public WarningException(string? message, string? helpUrl, string? helpTopic)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}
}
