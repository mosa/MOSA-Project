using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.IO;

public class FileLoadException : IOException
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

	public FileLoadException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected FileLoadException(SerializationInfo info, StreamingContext context)
	{
	}

	public FileLoadException(string? message)
	{
	}

	public FileLoadException(string? message, Exception? inner)
	{
	}

	public FileLoadException(string? message, string? fileName)
	{
	}

	public FileLoadException(string? message, string? fileName, Exception? inner)
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
