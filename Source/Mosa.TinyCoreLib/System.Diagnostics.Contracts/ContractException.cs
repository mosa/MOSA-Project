using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Diagnostics.Contracts;

public sealed class ContractException : Exception
{
	public string? Condition
	{
		get
		{
			throw null;
		}
	}

	public string Failure
	{
		get
		{
			throw null;
		}
	}

	public ContractFailureKind Kind
	{
		get
		{
			throw null;
		}
	}

	public string? UserMessage
	{
		get
		{
			throw null;
		}
	}

	public ContractException(ContractFailureKind kind, string? failure, string? userMessage, string? condition, Exception? innerException)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}
}
