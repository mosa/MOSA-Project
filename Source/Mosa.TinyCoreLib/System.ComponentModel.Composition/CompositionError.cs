using System.ComponentModel.Composition.Primitives;

namespace System.ComponentModel.Composition;

public class CompositionError
{
	public string Description
	{
		get
		{
			throw null;
		}
	}

	public ICompositionElement? Element
	{
		get
		{
			throw null;
		}
	}

	public Exception? Exception
	{
		get
		{
			throw null;
		}
	}

	public CompositionError(string? message)
	{
	}

	public CompositionError(string? message, ICompositionElement? element)
	{
	}

	public CompositionError(string? message, ICompositionElement? element, Exception? exception)
	{
	}

	public CompositionError(string? message, Exception? exception)
	{
	}

	public override string ToString()
	{
		throw null;
	}
}
