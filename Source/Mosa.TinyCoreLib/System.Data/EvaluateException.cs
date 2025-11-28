using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Data;

public class EvaluateException : InvalidExpressionException
{
	public EvaluateException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected EvaluateException(SerializationInfo info, StreamingContext context)
	{
	}

	public EvaluateException(string? s)
	{
	}

	public EvaluateException(string? message, Exception? innerException)
	{
	}
}
