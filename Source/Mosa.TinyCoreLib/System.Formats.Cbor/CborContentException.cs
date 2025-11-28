using System.Runtime.Serialization;

namespace System.Formats.Cbor;

public class CborContentException : Exception
{
	protected CborContentException(SerializationInfo info, StreamingContext context)
	{
	}

	public CborContentException(string? message)
	{
	}

	public CborContentException(string? message, Exception? inner)
	{
	}
}
