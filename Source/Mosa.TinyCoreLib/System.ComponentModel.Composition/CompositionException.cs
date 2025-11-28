using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.ComponentModel.Composition;

public class CompositionException : Exception
{
	public ReadOnlyCollection<CompositionError> Errors
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

	public ReadOnlyCollection<Exception> RootCauses
	{
		get
		{
			throw null;
		}
	}

	public CompositionException()
	{
	}

	public CompositionException(IEnumerable<CompositionError>? errors)
	{
	}

	public CompositionException(string? message)
	{
	}

	public CompositionException(string? message, Exception? innerException)
	{
	}
}
