using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Runtime.CompilerServices;

public sealed class SwitchExpressionException : InvalidOperationException
{
	public override string Message
	{
		get
		{
			throw null;
		}
	}

	public object? UnmatchedValue
	{
		get
		{
			throw null;
		}
	}

	public SwitchExpressionException()
	{
	}

	public SwitchExpressionException(Exception? innerException)
	{
	}

	public SwitchExpressionException(object? unmatchedValue)
	{
	}

	public SwitchExpressionException(string? message)
	{
	}

	public SwitchExpressionException(string? message, Exception? innerException)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}
}
