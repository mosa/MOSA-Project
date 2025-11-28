using System.Runtime.Serialization;

namespace System.ComponentModel.Composition.Primitives;

public class ComposablePartException : Exception
{
	public ICompositionElement? Element
	{
		get
		{
			throw null;
		}
	}

	public ComposablePartException()
	{
	}

	protected ComposablePartException(SerializationInfo info, StreamingContext context)
	{
	}

	public ComposablePartException(string? message)
	{
	}

	public ComposablePartException(string? message, ICompositionElement? element)
	{
	}

	public ComposablePartException(string? message, ICompositionElement? element, Exception? innerException)
	{
	}

	public ComposablePartException(string? message, Exception? innerException)
	{
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}
}
