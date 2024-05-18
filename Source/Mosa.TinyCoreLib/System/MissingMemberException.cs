using System.ComponentModel;
using System.Runtime.Serialization;

namespace System;

public class MissingMemberException : MemberAccessException
{
	protected string? ClassName;

	protected string? MemberName;

	protected byte[]? Signature;

	public override string Message
	{
		get
		{
			throw null;
		}
	}

	public MissingMemberException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected MissingMemberException(SerializationInfo info, StreamingContext context)
	{
	}

	public MissingMemberException(string? message)
	{
	}

	public MissingMemberException(string? message, Exception? inner)
	{
	}

	public MissingMemberException(string? className, string? memberName)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}
}
