using System.ComponentModel;
using System.Runtime.Serialization;

namespace System;

public class MissingMethodException : MissingMemberException
{
	public override string Message
	{
		get
		{
			throw null;
		}
	}

	public MissingMethodException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected MissingMethodException(SerializationInfo info, StreamingContext context)
	{
	}

	public MissingMethodException(string? message)
	{
	}

	public MissingMethodException(string? message, Exception? inner)
	{
	}

	public MissingMethodException(string? className, string? methodName)
	{
	}
}
