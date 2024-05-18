using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Transactions;

public class TransactionPromotionException : TransactionException
{
	public TransactionPromotionException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected TransactionPromotionException(SerializationInfo info, StreamingContext context)
	{
	}

	public TransactionPromotionException(string? message)
	{
	}

	public TransactionPromotionException(string? message, Exception? innerException)
	{
	}
}
