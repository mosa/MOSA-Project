using System.ComponentModel;
using System.Runtime.Serialization;

namespace System;

public class TypeLoadException : SystemException
{
	public override string Message
	{
		get
		{
			throw null;
		}
	}

	public string TypeName
	{
		get
		{
			throw null;
		}
	}

	public TypeLoadException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected TypeLoadException(SerializationInfo info, StreamingContext context)
	{
	}

	public TypeLoadException(string? message)
	{
	}

	public TypeLoadException(string? message, Exception? inner)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}
}
