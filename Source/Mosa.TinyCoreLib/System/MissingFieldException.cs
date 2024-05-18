using System.ComponentModel;
using System.Runtime.Serialization;

namespace System;

public class MissingFieldException : MissingMemberException
{
	public override string Message
	{
		get
		{
			throw null;
		}
	}

	public MissingFieldException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected MissingFieldException(SerializationInfo info, StreamingContext context)
	{
	}

	public MissingFieldException(string? message)
	{
	}

	public MissingFieldException(string? message, Exception? inner)
	{
	}

	public MissingFieldException(string? className, string? fieldName)
	{
	}
}
