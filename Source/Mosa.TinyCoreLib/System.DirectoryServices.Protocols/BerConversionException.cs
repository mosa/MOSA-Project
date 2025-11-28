using System.Runtime.Serialization;

namespace System.DirectoryServices.Protocols;

public class BerConversionException : DirectoryException
{
	public BerConversionException()
	{
	}

	protected BerConversionException(SerializationInfo info, StreamingContext context)
	{
	}

	public BerConversionException(string message)
	{
	}

	public BerConversionException(string message, Exception inner)
	{
	}
}
