using System.Threading;

namespace System.ComponentModel;

public sealed class AsyncOperation
{
	public SynchronizationContext SynchronizationContext
	{
		get
		{
			throw null;
		}
	}

	public object? UserSuppliedState
	{
		get
		{
			throw null;
		}
	}

	internal AsyncOperation()
	{
	}

	~AsyncOperation()
	{
	}

	public void OperationCompleted()
	{
	}

	public void Post(SendOrPostCallback d, object? arg)
	{
	}

	public void PostOperationCompleted(SendOrPostCallback d, object? arg)
	{
	}
}
