using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Transactions;

public class TransactionException : SystemException
{
	public TransactionException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected TransactionException(SerializationInfo info, StreamingContext context)
	{
	}

	public TransactionException(string? message)
	{
	}

	public TransactionException(string? message, Exception? innerException)
	{
	}
}
