using System.ComponentModel;
using System.Runtime.Serialization;

namespace System;

public class BadImageFormatException : SystemException
{
	public string? FileName
	{
		get
		{
			throw null;
		}
	}

	public string? FusionLog
	{
		get
		{
			throw null;
		}
	}

	public override string Message
	{
		get
		{
			throw null;
		}
	}

	public BadImageFormatException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected BadImageFormatException(SerializationInfo info, StreamingContext context)
	{
	}

	public BadImageFormatException(string? message)
	{
	}

	public BadImageFormatException(string? message, Exception? inner)
	{
	}

	public BadImageFormatException(string? message, string? fileName)
	{
	}

	public BadImageFormatException(string? message, string? fileName, Exception? inner)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	public override string ToString()
	{
		throw null;
	}
}
