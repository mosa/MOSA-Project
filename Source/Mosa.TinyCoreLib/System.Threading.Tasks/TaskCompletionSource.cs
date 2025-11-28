using System.Collections.Generic;

namespace System.Threading.Tasks;

public class TaskCompletionSource
{
	public Task Task
	{
		get
		{
			throw null;
		}
	}

	public TaskCompletionSource()
	{
	}

	public TaskCompletionSource(object? state)
	{
	}

	public TaskCompletionSource(object? state, TaskCreationOptions creationOptions)
	{
	}

	public TaskCompletionSource(TaskCreationOptions creationOptions)
	{
	}

	public void SetCanceled()
	{
	}

	public void SetCanceled(CancellationToken cancellationToken)
	{
	}

	public void SetException(IEnumerable<Exception> exceptions)
	{
	}

	public void SetException(Exception exception)
	{
	}

	public void SetResult()
	{
	}

	public bool TrySetCanceled()
	{
		throw null;
	}

	public bool TrySetCanceled(CancellationToken cancellationToken)
	{
		throw null;
	}

	public bool TrySetException(IEnumerable<Exception> exceptions)
	{
		throw null;
	}

	public bool TrySetException(Exception exception)
	{
		throw null;
	}

	public bool TrySetResult()
	{
		throw null;
	}
}
public class TaskCompletionSource<TResult>
{
	public Task<TResult> Task
	{
		get
		{
			throw null;
		}
	}

	public TaskCompletionSource()
	{
	}

	public TaskCompletionSource(object? state)
	{
	}

	public TaskCompletionSource(object? state, TaskCreationOptions creationOptions)
	{
	}

	public TaskCompletionSource(TaskCreationOptions creationOptions)
	{
	}

	public void SetCanceled()
	{
	}

	public void SetCanceled(CancellationToken cancellationToken)
	{
	}

	public void SetException(IEnumerable<Exception> exceptions)
	{
	}

	public void SetException(Exception exception)
	{
	}

	public void SetResult(TResult result)
	{
	}

	public bool TrySetCanceled()
	{
		throw null;
	}

	public bool TrySetCanceled(CancellationToken cancellationToken)
	{
		throw null;
	}

	public bool TrySetException(IEnumerable<Exception> exceptions)
	{
		throw null;
	}

	public bool TrySetException(Exception exception)
	{
		throw null;
	}

	public bool TrySetResult(TResult result)
	{
		throw null;
	}
}
