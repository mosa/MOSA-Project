using System.Runtime.Serialization;

namespace System.ComponentModel.Composition;

public class ImportCardinalityMismatchException : Exception
{
	public ImportCardinalityMismatchException()
	{
	}

	protected ImportCardinalityMismatchException(SerializationInfo info, StreamingContext context)
	{
	}

	public ImportCardinalityMismatchException(string? message)
	{
	}

	public ImportCardinalityMismatchException(string? message, Exception? innerException)
	{
	}
}
