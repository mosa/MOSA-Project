using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Transactions;

public class TransactionInDoubtException : TransactionException
{
	public TransactionInDoubtException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected TransactionInDoubtException(SerializationInfo info, StreamingContext context)
	{
	}

	public TransactionInDoubtException(string? message)
	{
	}

	public TransactionInDoubtException(string? message, Exception? innerException)
	{
	}
}
