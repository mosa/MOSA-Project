using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Data;

public class SyntaxErrorException : InvalidExpressionException
{
	public SyntaxErrorException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected SyntaxErrorException(SerializationInfo info, StreamingContext context)
	{
	}

	public SyntaxErrorException(string? s)
	{
	}

	public SyntaxErrorException(string? message, Exception? innerException)
	{
	}
}
