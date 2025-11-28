using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Net.Mail;

public class SmtpFailedRecipientException : SmtpException
{
	public string? FailedRecipient
	{
		get
		{
			throw null;
		}
	}

	public SmtpFailedRecipientException()
	{
	}

	public SmtpFailedRecipientException(SmtpStatusCode statusCode, string? failedRecipient)
	{
	}

	public SmtpFailedRecipientException(SmtpStatusCode statusCode, string? failedRecipient, string? serverResponse)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	protected SmtpFailedRecipientException(SerializationInfo info, StreamingContext context)
	{
	}

	public SmtpFailedRecipientException(string? message)
	{
	}

	public SmtpFailedRecipientException(string? message, Exception? innerException)
	{
	}

	public SmtpFailedRecipientException(string? message, string? failedRecipient, Exception? innerException)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}
}
