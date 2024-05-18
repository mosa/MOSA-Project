using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;

namespace System.Threading.Tasks;

public class Task : IAsyncResult, IDisposable
{
	public object? AsyncState
	{
		get
		{
			throw null;
		}
	}

	public static Task CompletedTask
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

	public static int? CurrentId
	{
		get
		{
			throw null;
		}
	}

	public AggregateException? Exception
	{
		get
		{
			throw null;
		}
	}

	public static TaskFactory Factory
	{
		get
		{
			throw null;
		}
	}

	public int Id
	{
		get
		{
			throw null;
		}
	}

	public bool IsCanceled
	{
		get
		{
			throw null;
		}
	}

	public bool IsCompleted
	{
		get
		{
			throw null;
		}
	}

	public bool IsCompletedSuccessfully
	{
		get
		{
			throw null;
		}
	}

	[MemberNotNullWhen(true, "Exception")]
	public bool IsFaulted
	{
		[MemberNotNullWhen(true, "Exception")]
		get
		{
			throw null;
		}
	}

	public TaskStatus Status
	{
		get
		{
			throw null;
		}
	}

	WaitHandle IAsyncResult.AsyncWaitHandle
	{
		get
		{
			throw null;
		}
	}

	bool IAsyncResult.CompletedSynchronously
	{
		get
		{
			throw null;
		}
	}

	public Task(Action action)
	{
	}

	public Task(Action action, CancellationToken cancellationToken)
	{
	}

	public Task(Action action, CancellationToken cancellationToken, TaskCreationOptions creationOptions)
	{
	}

	public Task(Action action, TaskCreationOptions creationOptions)
	{
	}

	public Task(Action<object?> action, object? state)
	{
	}

	public Task(Action<object?> action, object? state, CancellationToken cancellationToken)
	{
	}

	public Task(Action<object?> action, object? state, CancellationToken cancellationToken, TaskCreationOptions creationOptions)
	{
	}

	public Task(Action<object?> action, object? state, TaskCreationOptions creationOptions)
	{
	}

	public ConfiguredTaskAwaitable ConfigureAwait(bool continueOnCapturedContext)
	{
		throw null;
	}

	public ConfiguredTaskAwaitable ConfigureAwait(ConfigureAwaitOptions options)
	{
		throw null;
	}

	public Task ContinueWith(Action<Task, object?> continuationAction, object? state)
	{
		throw null;
	}

	public Task ContinueWith(Action<Task, object?> continuationAction, object? state, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task ContinueWith(Action<Task, object?> continuationAction, object? state, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
	{
		throw null;
	}

	public Task ContinueWith(Action<Task, object?> continuationAction, object? state, TaskContinuationOptions continuationOptions)
	{
		throw null;
	}

	public Task ContinueWith(Action<Task, object?> continuationAction, object? state, TaskScheduler scheduler)
	{
		throw null;
	}

	public Task ContinueWith(Action<Task> continuationAction)
	{
		throw null;
	}

	public Task ContinueWith(Action<Task> continuationAction, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task ContinueWith(Action<Task> continuationAction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
	{
		throw null;
	}

	public Task ContinueWith(Action<Task> continuationAction, TaskContinuationOptions continuationOptions)
	{
		throw null;
	}

	public Task ContinueWith(Action<Task> continuationAction, TaskScheduler scheduler)
	{
		throw null;
	}

	public Task<TResult> ContinueWith<TResult>(Func<Task, object?, TResult> continuationFunction, object? state)
	{
		throw null;
	}

	public Task<TResult> ContinueWith<TResult>(Func<Task, object?, TResult> continuationFunction, object? state, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task<TResult> ContinueWith<TResult>(Func<Task, object?, TResult> continuationFunction, object? state, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
	{
		throw null;
	}

	public Task<TResult> ContinueWith<TResult>(Func<Task, object?, TResult> continuationFunction, object? state, TaskContinuationOptions continuationOptions)
	{
		throw null;
	}

	public Task<TResult> ContinueWith<TResult>(Func<Task, object?, TResult> continuationFunction, object? state, TaskScheduler scheduler)
	{
		throw null;
	}

	public Task<TResult> ContinueWith<TResult>(Func<Task, TResult> continuationFunction)
	{
		throw null;
	}

	public Task<TResult> ContinueWith<TResult>(Func<Task, TResult> continuationFunction, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task<TResult> ContinueWith<TResult>(Func<Task, TResult> continuationFunction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
	{
		throw null;
	}

	public Task<TResult> ContinueWith<TResult>(Func<Task, TResult> continuationFunction, TaskContinuationOptions continuationOptions)
	{
		throw null;
	}

	public Task<TResult> ContinueWith<TResult>(Func<Task, TResult> continuationFunction, TaskScheduler scheduler)
	{
		throw null;
	}

	public static Task Delay(int millisecondsDelay)
	{
		throw null;
	}

	public static Task Delay(int millisecondsDelay, CancellationToken cancellationToken)
	{
		throw null;
	}

	public static Task Delay(TimeSpan delay)
	{
		throw null;
	}

	public static Task Delay(TimeSpan delay, CancellationToken cancellationToken)
	{
		throw null;
	}

	public static Task Delay(TimeSpan delay, TimeProvider timeProvider)
	{
		throw null;
	}

	public static Task Delay(TimeSpan delay, TimeProvider timeProvider, CancellationToken cancellationToken)
	{
		throw null;
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public static Task FromCanceled(CancellationToken cancellationToken)
	{
		throw null;
	}

	public static Task<TResult> FromCanceled<TResult>(CancellationToken cancellationToken)
	{
		throw null;
	}

	public static Task FromException(Exception exception)
	{
		throw null;
	}

	public static Task<TResult> FromException<TResult>(Exception exception)
	{
		throw null;
	}

	public static Task<TResult> FromResult<TResult>(TResult result)
	{
		throw null;
	}

	public TaskAwaiter GetAwaiter()
	{
		throw null;
	}

	public static Task Run(Action action)
	{
		throw null;
	}

	public static Task Run(Action action, CancellationToken cancellationToken)
	{
		throw null;
	}

	public static Task Run(Func<Task?> function)
	{
		throw null;
	}

	public static Task Run(Func<Task?> function, CancellationToken cancellationToken)
	{
		throw null;
	}

	public void RunSynchronously()
	{
	}

	public void RunSynchronously(TaskScheduler scheduler)
	{
	}

	public static Task<TResult> Run<TResult>(Func<Task<TResult>?> function)
	{
		throw null;
	}

	public static Task<TResult> Run<TResult>(Func<Task<TResult>?> function, CancellationToken cancellationToken)
	{
		throw null;
	}

	public static Task<TResult> Run<TResult>(Func<TResult> function)
	{
		throw null;
	}

	public static Task<TResult> Run<TResult>(Func<TResult> function, CancellationToken cancellationToken)
	{
		throw null;
	}

	public void Start()
	{
	}

	public void Start(TaskScheduler scheduler)
	{
	}

	public void Wait()
	{
	}

	public bool Wait(int millisecondsTimeout)
	{
		throw null;
	}

	public bool Wait(int millisecondsTimeout, CancellationToken cancellationToken)
	{
		throw null;
	}

	public void Wait(CancellationToken cancellationToken)
	{
	}

	public bool Wait(TimeSpan timeout)
	{
		throw null;
	}

	public bool Wait(TimeSpan timeout, CancellationToken cancellationToken)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public static void WaitAll(params Task[] tasks)
	{
	}

	[UnsupportedOSPlatform("browser")]
	public static bool WaitAll(Task[] tasks, int millisecondsTimeout)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public static bool WaitAll(Task[] tasks, int millisecondsTimeout, CancellationToken cancellationToken)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public static void WaitAll(Task[] tasks, CancellationToken cancellationToken)
	{
	}

	[UnsupportedOSPlatform("browser")]
	public static bool WaitAll(Task[] tasks, TimeSpan timeout)
	{
		throw null;
	}

	public static int WaitAny(params Task[] tasks)
	{
		throw null;
	}

	public static int WaitAny(Task[] tasks, int millisecondsTimeout)
	{
		throw null;
	}

	public static int WaitAny(Task[] tasks, int millisecondsTimeout, CancellationToken cancellationToken)
	{
		throw null;
	}

	public static int WaitAny(Task[] tasks, CancellationToken cancellationToken)
	{
		throw null;
	}

	public static int WaitAny(Task[] tasks, TimeSpan timeout)
	{
		throw null;
	}

	public Task WaitAsync(CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task WaitAsync(TimeSpan timeout)
	{
		throw null;
	}

	public Task WaitAsync(TimeSpan timeout, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task WaitAsync(TimeSpan timeout, TimeProvider timeProvider)
	{
		throw null;
	}

	public Task WaitAsync(TimeSpan timeout, TimeProvider timeProvider, CancellationToken cancellationToken)
	{
		throw null;
	}

	public static Task WhenAll(IEnumerable<Task> tasks)
	{
		throw null;
	}

	public static Task WhenAll(params Task[] tasks)
	{
		throw null;
	}

	public static Task<TResult[]> WhenAll<TResult>(IEnumerable<Task<TResult>> tasks)
	{
		throw null;
	}

	public static Task<TResult[]> WhenAll<TResult>(params Task<TResult>[] tasks)
	{
		throw null;
	}

	public static Task<Task> WhenAny(IEnumerable<Task> tasks)
	{
		throw null;
	}

	public static Task<Task> WhenAny(Task task1, Task task2)
	{
		throw null;
	}

	public static Task<Task> WhenAny(params Task[] tasks)
	{
		throw null;
	}

	public static Task<Task<TResult>> WhenAny<TResult>(IEnumerable<Task<TResult>> tasks)
	{
		throw null;
	}

	public static Task<Task<TResult>> WhenAny<TResult>(Task<TResult> task1, Task<TResult> task2)
	{
		throw null;
	}

	public static Task<Task<TResult>> WhenAny<TResult>(params Task<TResult>[] tasks)
	{
		throw null;
	}

	public static YieldAwaitable Yield()
	{
		throw null;
	}
}
public class Task<TResult> : Task
{
	public new static TaskFactory<TResult> Factory
	{
		get
		{
			throw null;
		}
	}

	public TResult Result
	{
		get
		{
			throw null;
		}
	}

	public Task(Func<object?, TResult> function, object? state)
		: base(null)
	{
	}

	public Task(Func<object?, TResult> function, object? state, CancellationToken cancellationToken)
		: base(null)
	{
	}

	public Task(Func<object?, TResult> function, object? state, CancellationToken cancellationToken, TaskCreationOptions creationOptions)
		: base(null)
	{
	}

	public Task(Func<object?, TResult> function, object? state, TaskCreationOptions creationOptions)
		: base(null)
	{
	}

	public Task(Func<TResult> function)
		: base(null)
	{
	}

	public Task(Func<TResult> function, CancellationToken cancellationToken)
		: base(null)
	{
	}

	public Task(Func<TResult> function, CancellationToken cancellationToken, TaskCreationOptions creationOptions)
		: base(null)
	{
	}

	public Task(Func<TResult> function, TaskCreationOptions creationOptions)
		: base(null)
	{
	}

	public new ConfiguredTaskAwaitable<TResult> ConfigureAwait(bool continueOnCapturedContext)
	{
		throw null;
	}

	public new ConfiguredTaskAwaitable<TResult> ConfigureAwait(ConfigureAwaitOptions options)
	{
		throw null;
	}

	public Task ContinueWith(Action<Task<TResult>, object?> continuationAction, object? state)
	{
		throw null;
	}

	public Task ContinueWith(Action<Task<TResult>, object?> continuationAction, object? state, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task ContinueWith(Action<Task<TResult>, object?> continuationAction, object? state, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
	{
		throw null;
	}

	public Task ContinueWith(Action<Task<TResult>, object?> continuationAction, object? state, TaskContinuationOptions continuationOptions)
	{
		throw null;
	}

	public Task ContinueWith(Action<Task<TResult>, object?> continuationAction, object? state, TaskScheduler scheduler)
	{
		throw null;
	}

	public Task ContinueWith(Action<Task<TResult>> continuationAction)
	{
		throw null;
	}

	public Task ContinueWith(Action<Task<TResult>> continuationAction, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task ContinueWith(Action<Task<TResult>> continuationAction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
	{
		throw null;
	}

	public Task ContinueWith(Action<Task<TResult>> continuationAction, TaskContinuationOptions continuationOptions)
	{
		throw null;
	}

	public Task ContinueWith(Action<Task<TResult>> continuationAction, TaskScheduler scheduler)
	{
		throw null;
	}

	public Task<TNewResult> ContinueWith<TNewResult>(Func<Task<TResult>, object?, TNewResult> continuationFunction, object? state)
	{
		throw null;
	}

	public Task<TNewResult> ContinueWith<TNewResult>(Func<Task<TResult>, object?, TNewResult> continuationFunction, object? state, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task<TNewResult> ContinueWith<TNewResult>(Func<Task<TResult>, object?, TNewResult> continuationFunction, object? state, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
	{
		throw null;
	}

	public Task<TNewResult> ContinueWith<TNewResult>(Func<Task<TResult>, object?, TNewResult> continuationFunction, object? state, TaskContinuationOptions continuationOptions)
	{
		throw null;
	}

	public Task<TNewResult> ContinueWith<TNewResult>(Func<Task<TResult>, object?, TNewResult> continuationFunction, object? state, TaskScheduler scheduler)
	{
		throw null;
	}

	public Task<TNewResult> ContinueWith<TNewResult>(Func<Task<TResult>, TNewResult> continuationFunction)
	{
		throw null;
	}

	public Task<TNewResult> ContinueWith<TNewResult>(Func<Task<TResult>, TNewResult> continuationFunction, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task<TNewResult> ContinueWith<TNewResult>(Func<Task<TResult>, TNewResult> continuationFunction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
	{
		throw null;
	}

	public Task<TNewResult> ContinueWith<TNewResult>(Func<Task<TResult>, TNewResult> continuationFunction, TaskContinuationOptions continuationOptions)
	{
		throw null;
	}

	public Task<TNewResult> ContinueWith<TNewResult>(Func<Task<TResult>, TNewResult> continuationFunction, TaskScheduler scheduler)
	{
		throw null;
	}

	public new TaskAwaiter<TResult> GetAwaiter()
	{
		throw null;
	}

	public new Task<TResult> WaitAsync(CancellationToken cancellationToken)
	{
		throw null;
	}

	public new Task<TResult> WaitAsync(TimeSpan timeout)
	{
		throw null;
	}

	public new Task<TResult> WaitAsync(TimeSpan timeout, CancellationToken cancellationToken)
	{
		throw null;
	}

	public new Task<TResult> WaitAsync(TimeSpan timeout, TimeProvider timeProvider)
	{
		throw null;
	}

	public new Task<TResult> WaitAsync(TimeSpan timeout, TimeProvider timeProvider, CancellationToken cancellationToken)
	{
		throw null;
	}
}
