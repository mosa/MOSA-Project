using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.IO;

public class FileNotFoundException : IOException
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

	public FileNotFoundException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected FileNotFoundException(SerializationInfo info, StreamingContext context)
	{
	}

	public FileNotFoundException(string? message)
	{
	}

	public FileNotFoundException(string? message, Exception? innerException)
	{
	}

	public FileNotFoundException(string? message, string? fileName)
	{
	}

	public FileNotFoundException(string? message, string? fileName, Exception? innerException)
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
