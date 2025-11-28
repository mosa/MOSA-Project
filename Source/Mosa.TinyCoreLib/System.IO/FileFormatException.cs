using System.Runtime.Serialization;

namespace System.IO;

public class FileFormatException : FormatException
{
	public Uri? SourceUri
	{
		get
		{
			throw null;
		}
	}

	protected FileFormatException(SerializationInfo info, StreamingContext context)
	{
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	public FileFormatException()
	{
	}

	public FileFormatException(string? message)
	{
	}

	public FileFormatException(string? message, Exception? innerException)
	{
	}

	public FileFormatException(Uri? sourceUri)
	{
	}

	public FileFormatException(Uri? sourceUri, Exception? innerException)
	{
	}

	public FileFormatException(Uri? sourceUri, string? message)
	{
	}

	public FileFormatException(Uri? sourceUri, string? message, Exception? innerException)
	{
	}
}
