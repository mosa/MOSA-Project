using System.Runtime.Serialization;

namespace System.Formats.Asn1;

public class AsnContentException : Exception
{
	public AsnContentException()
	{
	}

	protected AsnContentException(SerializationInfo info, StreamingContext context)
	{
	}

	public AsnContentException(string? message)
	{
	}

	public AsnContentException(string? message, Exception? inner)
	{
	}
}
