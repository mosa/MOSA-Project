namespace System.Threading.Tasks;

public class TaskFactory
{
	public CancellationToken CancellationToken
	{
		get
		{
			throw null;
		}
	}

	public TaskContinuationOptions ContinuationOptions
	{
		get
		{
			throw null;
		}
	}

	public TaskCreationOptions CreationOptions
	{
		get
		{
			throw null;
		}
	}

	public TaskScheduler? Scheduler
	{
		get
		{
			throw null;
		}
	}

	public TaskFactory()
	{
	}

	public TaskFactory(CancellationToken cancellationToken)
	{
	}

	public TaskFactory(CancellationToken cancellationToken, TaskCreationOptions creationOptions, TaskContinuationOptions continuationOptions, TaskScheduler? scheduler)
	{
	}

	public TaskFactory(TaskCreationOptions creationOptions, TaskContinuationOptions continuationOptions)
	{
	}

	public TaskFactory(TaskScheduler? scheduler)
	{
	}

	public Task ContinueWhenAll(Task[] tasks, Action<Task[]> continuationAction)
	{
		throw null;
	}

	public Task ContinueWhenAll(Task[] tasks, Action<Task[]> continuationAction, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task ContinueWhenAll(Task[] tasks, Action<Task[]> continuationAction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
	{
		throw null;
	}

	public Task ContinueWhenAll(Task[] tasks, Action<Task[]> continuationAction, TaskContinuationOptions continuationOptions)
	{
		throw null;
	}

	public Task<TResult> ContinueWhenAll<TResult>(Task[] tasks, Func<Task[], TResult> continuationFunction)
	{
		throw null;
	}

	public Task<TResult> ContinueWhenAll<TResult>(Task[] tasks, Func<Task[], TResult> continuationFunction, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task<TResult> ContinueWhenAll<TResult>(Task[] tasks, Func<Task[], TResult> continuationFunction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
	{
		throw null;
	}

	public Task<TResult> ContinueWhenAll<TResult>(Task[] tasks, Func<Task[], TResult> continuationFunction, TaskContinuationOptions continuationOptions)
	{
		throw null;
	}

	public Task ContinueWhenAll<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Action<Task<TAntecedentResult>[]> continuationAction)
	{
		throw null;
	}

	public Task ContinueWhenAll<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Action<Task<TAntecedentResult>[]> continuationAction, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task ContinueWhenAll<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Action<Task<TAntecedentResult>[]> continuationAction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
	{
		throw null;
	}

	public Task ContinueWhenAll<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Action<Task<TAntecedentResult>[]> continuationAction, TaskContinuationOptions continuationOptions)
	{
		throw null;
	}

	public Task<TResult> ContinueWhenAll<TAntecedentResult, TResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>[], TResult> continuationFunction)
	{
		throw null;
	}

	public Task<TResult> ContinueWhenAll<TAntecedentResult, TResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>[], TResult> continuationFunction, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task<TResult> ContinueWhenAll<TAntecedentResult, TResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>[], TResult> continuationFunction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
	{
		throw null;
	}

	public Task<TResult> ContinueWhenAll<TAntecedentResult, TResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>[], TResult> continuationFunction, TaskContinuationOptions continuationOptions)
	{
		throw null;
	}

	public Task ContinueWhenAny(Task[] tasks, Action<Task> continuationAction)
	{
		throw null;
	}

	public Task ContinueWhenAny(Task[] tasks, Action<Task> continuationAction, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task ContinueWhenAny(Task[] tasks, Action<Task> continuationAction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
	{
		throw null;
	}

	public Task ContinueWhenAny(Task[] tasks, Action<Task> continuationAction, TaskContinuationOptions continuationOptions)
	{
		throw null;
	}

	public Task<TResult> ContinueWhenAny<TResult>(Task[] tasks, Func<Task, TResult> continuationFunction)
	{
		throw null;
	}

	public Task<TResult> ContinueWhenAny<TResult>(Task[] tasks, Func<Task, TResult> continuationFunction, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task<TResult> ContinueWhenAny<TResult>(Task[] tasks, Func<Task, TResult> continuationFunction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
	{
		throw null;
	}

	public Task<TResult> ContinueWhenAny<TResult>(Task[] tasks, Func<Task, TResult> continuationFunction, TaskContinuationOptions continuationOptions)
	{
		throw null;
	}

	public Task ContinueWhenAny<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Action<Task<TAntecedentResult>> continuationAction)
	{
		throw null;
	}

	public Task ContinueWhenAny<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Action<Task<TAntecedentResult>> continuationAction, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task ContinueWhenAny<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Action<Task<TAntecedentResult>> continuationAction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
	{
		throw null;
	}

	public Task ContinueWhenAny<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Action<Task<TAntecedentResult>> continuationAction, TaskContinuationOptions continuationOptions)
	{
		throw null;
	}

	public Task<TResult> ContinueWhenAny<TAntecedentResult, TResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>, TResult> continuationFunction)
	{
		throw null;
	}

	public Task<TResult> ContinueWhenAny<TAntecedentResult, TResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>, TResult> continuationFunction, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task<TResult> ContinueWhenAny<TAntecedentResult, TResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>, TResult> continuationFunction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
	{
		throw null;
	}

	public Task<TResult> ContinueWhenAny<TAntecedentResult, TResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>, TResult> continuationFunction, TaskContinuationOptions continuationOptions)
	{
		throw null;
	}

	public Task FromAsync(Func<AsyncCallback, object?, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, object? state)
	{
		throw null;
	}

	public Task FromAsync(Func<AsyncCallback, object?, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, object? state, TaskCreationOptions creationOptions)
	{
		throw null;
	}

	public Task FromAsync(IAsyncResult asyncResult, Action<IAsyncResult> endMethod)
	{
		throw null;
	}

	public Task FromAsync(IAsyncResult asyncResult, Action<IAsyncResult> endMethod, TaskCreationOptions creationOptions)
	{
		throw null;
	}

	public Task FromAsync(IAsyncResult asyncResult, Action<IAsyncResult> endMethod, TaskCreationOptions creationOptions, TaskScheduler scheduler)
	{
		throw null;
	}

	public Task<TResult> FromAsync<TResult>(Func<AsyncCallback, object?, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, object? state)
	{
		throw null;
	}

	public Task<TResult> FromAsync<TResult>(Func<AsyncCallback, object?, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, object? state, TaskCreationOptions creationOptions)
	{
		throw null;
	}

	public Task FromAsync<TArg1>(Func<TArg1, AsyncCallback, object?, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, TArg1 arg1, object? state)
	{
		throw null;
	}

	public Task FromAsync<TArg1>(Func<TArg1, AsyncCallback, object?, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, TArg1 arg1, object? state, TaskCreationOptions creationOptions)
	{
		throw null;
	}

	public Task<TResult> FromAsync<TResult>(IAsyncResult asyncResult, Func<IAsyncResult, TResult> endMethod)
	{
		throw null;
	}

	public Task<TResult> FromAsync<TResult>(IAsyncResult asyncResult, Func<IAsyncResult, TResult> endMethod, TaskCreationOptions creationOptions)
	{
		throw null;
	}

	public Task<TResult> FromAsync<TResult>(IAsyncResult asyncResult, Func<IAsyncResult, TResult> endMethod, TaskCreationOptions creationOptions, TaskScheduler scheduler)
	{
		throw null;
	}

	public Task<TResult> FromAsync<TArg1, TResult>(Func<TArg1, AsyncCallback, object?, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, object? state)
	{
		throw null;
	}

	public Task<TResult> FromAsync<TArg1, TResult>(Func<TArg1, AsyncCallback, object?, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, object? state, TaskCreationOptions creationOptions)
	{
		throw null;
	}

	public Task FromAsync<TArg1, TArg2>(Func<TArg1, TArg2, AsyncCallback, object?, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, TArg1 arg1, TArg2 arg2, object? state)
	{
		throw null;
	}

	public Task FromAsync<TArg1, TArg2>(Func<TArg1, TArg2, AsyncCallback, object?, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, TArg1 arg1, TArg2 arg2, object? state, TaskCreationOptions creationOptions)
	{
		throw null;
	}

	public Task<TResult> FromAsync<TArg1, TArg2, TResult>(Func<TArg1, TArg2, AsyncCallback, object?, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, object? state)
	{
		throw null;
	}

	public Task<TResult> FromAsync<TArg1, TArg2, TResult>(Func<TArg1, TArg2, AsyncCallback, object?, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, object? state, TaskCreationOptions creationOptions)
	{
		throw null;
	}

	public Task FromAsync<TArg1, TArg2, TArg3>(Func<TArg1, TArg2, TArg3, AsyncCallback, object?, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, TArg1 arg1, TArg2 arg2, TArg3 arg3, object? state)
	{
		throw null;
	}

	public Task FromAsync<TArg1, TArg2, TArg3>(Func<TArg1, TArg2, TArg3, AsyncCallback, object?, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, TArg1 arg1, TArg2 arg2, TArg3 arg3, object? state, TaskCreationOptions creationOptions)
	{
		throw null;
	}

	public Task<TResult> FromAsync<TArg1, TArg2, TArg3, TResult>(Func<TArg1, TArg2, TArg3, AsyncCallback, object?, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, TArg3 arg3, object? state)
	{
		throw null;
	}

	public Task<TResult> FromAsync<TArg1, TArg2, TArg3, TResult>(Func<TArg1, TArg2, TArg3, AsyncCallback, object?, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, TArg3 arg3, object? state, TaskCreationOptions creationOptions)
	{
		throw null;
	}

	public Task StartNew(Action action)
	{
		throw null;
	}

	public Task StartNew(Action action, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task StartNew(Action action, CancellationToken cancellationToken, TaskCreationOptions creationOptions, TaskScheduler scheduler)
	{
		throw null;
	}

	public Task StartNew(Action action, TaskCreationOptions creationOptions)
	{
		throw null;
	}

	public Task StartNew(Action<object?> action, object? state)
	{
		throw null;
	}

	public Task StartNew(Action<object?> action, object? state, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task StartNew(Action<object?> action, object? state, CancellationToken cancellationToken, TaskCreationOptions creationOptions, TaskScheduler scheduler)
	{
		throw null;
	}

	public Task StartNew(Action<object?> action, object? state, TaskCreationOptions creationOptions)
	{
		throw null;
	}

	public Task<TResult> StartNew<TResult>(Func<object?, TResult> function, object? state)
	{
		throw null;
	}

	public Task<TResult> StartNew<TResult>(Func<object?, TResult> function, object? state, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task<TResult> StartNew<TResult>(Func<object?, TResult> function, object? state, CancellationToken cancellationToken, TaskCreationOptions creationOptions, TaskScheduler scheduler)
	{
		throw null;
	}

	public Task<TResult> StartNew<TResult>(Func<object?, TResult> function, object? state, TaskCreationOptions creationOptions)
	{
		throw null;
	}

	public Task<TResult> StartNew<TResult>(Func<TResult> function)
	{
		throw null;
	}

	public Task<TResult> StartNew<TResult>(Func<TResult> function, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task<TResult> StartNew<TResult>(Func<TResult> function, CancellationToken cancellationToken, TaskCreationOptions creationOptions, TaskScheduler scheduler)
	{
		throw null;
	}

	public Task<TResult> StartNew<TResult>(Func<TResult> function, TaskCreationOptions creationOptions)
	{
		throw null;
	}
}
public class TaskFactory<TResult>
{
	public CancellationToken CancellationToken
	{
		get
		{
			throw null;
		}
	}

	public TaskContinuationOptions ContinuationOptions
	{
		get
		{
			throw null;
		}
	}

	public TaskCreationOptions CreationOptions
	{
		get
		{
			throw null;
		}
	}

	public TaskScheduler? Scheduler
	{
		get
		{
			throw null;
		}
	}

	public TaskFactory()
	{
	}

	public TaskFactory(CancellationToken cancellationToken)
	{
	}

	public TaskFactory(CancellationToken cancellationToken, TaskCreationOptions creationOptions, TaskContinuationOptions continuationOptions, TaskScheduler? scheduler)
	{
	}

	public TaskFactory(TaskCreationOptions creationOptions, TaskContinuationOptions continuationOptions)
	{
	}

	public TaskFactory(TaskScheduler? scheduler)
	{
	}

	public Task<TResult> ContinueWhenAll(Task[] tasks, Func<Task[], TResult> continuationFunction)
	{
		throw null;
	}

	public Task<TResult> ContinueWhenAll(Task[] tasks, Func<Task[], TResult> continuationFunction, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task<TResult> ContinueWhenAll(Task[] tasks, Func<Task[], TResult> continuationFunction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
	{
		throw null;
	}

	public Task<TResult> ContinueWhenAll(Task[] tasks, Func<Task[], TResult> continuationFunction, TaskContinuationOptions continuationOptions)
	{
		throw null;
	}

	public Task<TResult> ContinueWhenAll<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>[], TResult> continuationFunction)
	{
		throw null;
	}

	public Task<TResult> ContinueWhenAll<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>[], TResult> continuationFunction, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task<TResult> ContinueWhenAll<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>[], TResult> continuationFunction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
	{
		throw null;
	}

	public Task<TResult> ContinueWhenAll<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>[], TResult> continuationFunction, TaskContinuationOptions continuationOptions)
	{
		throw null;
	}

	public Task<TResult> ContinueWhenAny(Task[] tasks, Func<Task, TResult> continuationFunction)
	{
		throw null;
	}

	public Task<TResult> ContinueWhenAny(Task[] tasks, Func<Task, TResult> continuationFunction, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task<TResult> ContinueWhenAny(Task[] tasks, Func<Task, TResult> continuationFunction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
	{
		throw null;
	}

	public Task<TResult> ContinueWhenAny(Task[] tasks, Func<Task, TResult> continuationFunction, TaskContinuationOptions continuationOptions)
	{
		throw null;
	}

	public Task<TResult> ContinueWhenAny<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>, TResult> continuationFunction)
	{
		throw null;
	}

	public Task<TResult> ContinueWhenAny<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>, TResult> continuationFunction, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task<TResult> ContinueWhenAny<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>, TResult> continuationFunction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
	{
		throw null;
	}

	public Task<TResult> ContinueWhenAny<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>, TResult> continuationFunction, TaskContinuationOptions continuationOptions)
	{
		throw null;
	}

	public Task<TResult> FromAsync(Func<AsyncCallback, object?, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, object? state)
	{
		throw null;
	}

	public Task<TResult> FromAsync(Func<AsyncCallback, object?, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, object? state, TaskCreationOptions creationOptions)
	{
		throw null;
	}

	public Task<TResult> FromAsync(IAsyncResult asyncResult, Func<IAsyncResult, TResult> endMethod)
	{
		throw null;
	}

	public Task<TResult> FromAsync(IAsyncResult asyncResult, Func<IAsyncResult, TResult> endMethod, TaskCreationOptions creationOptions)
	{
		throw null;
	}

	public Task<TResult> FromAsync(IAsyncResult asyncResult, Func<IAsyncResult, TResult> endMethod, TaskCreationOptions creationOptions, TaskScheduler scheduler)
	{
		throw null;
	}

	public Task<TResult> FromAsync<TArg1>(Func<TArg1, AsyncCallback, object?, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, object? state)
	{
		throw null;
	}

	public Task<TResult> FromAsync<TArg1>(Func<TArg1, AsyncCallback, object?, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, object? state, TaskCreationOptions creationOptions)
	{
		throw null;
	}

	public Task<TResult> FromAsync<TArg1, TArg2>(Func<TArg1, TArg2, AsyncCallback, object?, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, object? state)
	{
		throw null;
	}

	public Task<TResult> FromAsync<TArg1, TArg2>(Func<TArg1, TArg2, AsyncCallback, object?, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, object? state, TaskCreationOptions creationOptions)
	{
		throw null;
	}

	public Task<TResult> FromAsync<TArg1, TArg2, TArg3>(Func<TArg1, TArg2, TArg3, AsyncCallback, object?, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, TArg3 arg3, object? state)
	{
		throw null;
	}

	public Task<TResult> FromAsync<TArg1, TArg2, TArg3>(Func<TArg1, TArg2, TArg3, AsyncCallback, object?, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, TArg3 arg3, object? state, TaskCreationOptions creationOptions)
	{
		throw null;
	}

	public Task<TResult> StartNew(Func<object?, TResult> function, object? state)
	{
		throw null;
	}

	public Task<TResult> StartNew(Func<object?, TResult> function, object? state, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task<TResult> StartNew(Func<object?, TResult> function, object? state, CancellationToken cancellationToken, TaskCreationOptions creationOptions, TaskScheduler scheduler)
	{
		throw null;
	}

	public Task<TResult> StartNew(Func<object?, TResult> function, object? state, TaskCreationOptions creationOptions)
	{
		throw null;
	}

	public Task<TResult> StartNew(Func<TResult> function)
	{
		throw null;
	}

	public Task<TResult> StartNew(Func<TResult> function, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task<TResult> StartNew(Func<TResult> function, CancellationToken cancellationToken, TaskCreationOptions creationOptions, TaskScheduler scheduler)
	{
		throw null;
	}

	public Task<TResult> StartNew(Func<TResult> function, TaskCreationOptions creationOptions)
	{
		throw null;
	}
}
