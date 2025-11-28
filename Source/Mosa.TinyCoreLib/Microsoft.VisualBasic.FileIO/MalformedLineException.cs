using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Microsoft.VisualBasic.FileIO;

public class MalformedLineException : Exception
{
	[EditorBrowsable(EditorBrowsableState.Always)]
	public long LineNumber
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public MalformedLineException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected MalformedLineException(SerializationInfo info, StreamingContext context)
	{
	}

	public MalformedLineException(string? message)
	{
	}

	public MalformedLineException(string? message, Exception? innerException)
	{
	}

	public MalformedLineException(string? message, long lineNumber)
	{
	}

	public MalformedLineException(string? message, long lineNumber, Exception? innerException)
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
