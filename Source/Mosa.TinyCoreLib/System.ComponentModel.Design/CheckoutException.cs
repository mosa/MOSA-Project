using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.ComponentModel.Design;

public class CheckoutException : ExternalException
{
	public static readonly CheckoutException Canceled;

	public CheckoutException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected CheckoutException(SerializationInfo info, StreamingContext context)
	{
	}

	public CheckoutException(string? message)
	{
	}

	public CheckoutException(string? message, Exception? innerException)
	{
	}

	public CheckoutException(string? message, int errorCode)
	{
	}
}
