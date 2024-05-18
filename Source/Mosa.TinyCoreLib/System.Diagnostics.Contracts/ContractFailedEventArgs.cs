namespace System.Diagnostics.Contracts;

public sealed class ContractFailedEventArgs : EventArgs
{
	public string? Condition
	{
		get
		{
			throw null;
		}
	}

	public ContractFailureKind FailureKind
	{
		get
		{
			throw null;
		}
	}

	public bool Handled
	{
		get
		{
			throw null;
		}
	}

	public string? Message
	{
		get
		{
			throw null;
		}
	}

	public Exception? OriginalException
	{
		get
		{
			throw null;
		}
	}

	public bool Unwind
	{
		get
		{
			throw null;
		}
	}

	public ContractFailedEventArgs(ContractFailureKind failureKind, string? message, string? condition, Exception? originalException)
	{
	}

	public void SetHandled()
	{
	}

	public void SetUnwind()
	{
	}
}
