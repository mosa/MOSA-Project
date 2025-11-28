using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Net.Mail;

public class SmtpException : Exception
{
	public SmtpStatusCode StatusCode
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SmtpException()
	{
	}

	public SmtpException(SmtpStatusCode statusCode)
	{
	}

	public SmtpException(SmtpStatusCode statusCode, string? message)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	protected SmtpException(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}

	public SmtpException(string? message)
	{
	}

	public SmtpException(string? message, Exception? innerException)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}
}
