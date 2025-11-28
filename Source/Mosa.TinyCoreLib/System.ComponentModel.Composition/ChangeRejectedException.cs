using System.Collections.Generic;

namespace System.ComponentModel.Composition;

public class ChangeRejectedException : CompositionException
{
	public override string Message
	{
		get
		{
			throw null;
		}
	}

	public ChangeRejectedException()
	{
	}

	public ChangeRejectedException(IEnumerable<CompositionError>? errors)
	{
	}

	public ChangeRejectedException(string? message)
	{
	}

	public ChangeRejectedException(string? message, Exception? innerException)
	{
	}
}
