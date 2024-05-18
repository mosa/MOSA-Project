using System.Runtime.Serialization;

namespace System.Reflection.Metadata;

public class ImageFormatLimitationException : Exception
{
	protected ImageFormatLimitationException(SerializationInfo info, StreamingContext context)
	{
	}

	public ImageFormatLimitationException()
	{
	}

	public ImageFormatLimitationException(string? message)
	{
	}

	public ImageFormatLimitationException(string? message, Exception? innerException)
	{
	}
}
