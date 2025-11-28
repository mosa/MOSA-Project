using System.Runtime.Serialization;

namespace System.Text.Json;

public class JsonException : Exception
{
	public long? BytePositionInLine
	{
		get
		{
			throw null;
		}
	}

	public long? LineNumber
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

	public string? Path
	{
		get
		{
			throw null;
		}
	}

	public JsonException()
	{
	}

	protected JsonException(SerializationInfo info, StreamingContext context)
	{
	}

	public JsonException(string? message)
	{
	}

	public JsonException(string? message, Exception? innerException)
	{
	}

	public JsonException(string? message, string? path, long? lineNumber, long? bytePositionInLine)
	{
	}

	public JsonException(string? message, string? path, long? lineNumber, long? bytePositionInLine, Exception? innerException)
	{
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}
}
