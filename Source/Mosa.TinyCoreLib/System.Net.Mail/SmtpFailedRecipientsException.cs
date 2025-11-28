using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Net.Mail;

public class SmtpFailedRecipientsException : SmtpFailedRecipientException
{
	public SmtpFailedRecipientException[] InnerExceptions
	{
		get
		{
			throw null;
		}
	}

	public SmtpFailedRecipientsException()
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	protected SmtpFailedRecipientsException(SerializationInfo info, StreamingContext context)
	{
	}

	public SmtpFailedRecipientsException(string? message)
	{
	}

	public SmtpFailedRecipientsException(string? message, Exception? innerException)
	{
	}

	public SmtpFailedRecipientsException(string? message, SmtpFailedRecipientException[] innerExceptions)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}
}
