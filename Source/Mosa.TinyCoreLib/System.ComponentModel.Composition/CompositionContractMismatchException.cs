using System.Runtime.Serialization;

namespace System.ComponentModel.Composition;

public class CompositionContractMismatchException : Exception
{
	public CompositionContractMismatchException()
	{
	}

	protected CompositionContractMismatchException(SerializationInfo info, StreamingContext context)
	{
	}

	public CompositionContractMismatchException(string? message)
	{
	}

	public CompositionContractMismatchException(string? message, Exception? innerException)
	{
	}
}
