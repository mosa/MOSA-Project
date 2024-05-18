using System.Threading.Tasks;

namespace System.Runtime.CompilerServices;

public struct AsyncTaskMethodBuilder
{
	private object _dummy;

	private int _dummyPrimitive;

	public Task Task
	{
		get
		{
			throw null;
		}
	}

	public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : INotifyCompletion where TStateMachine : IAsyncStateMachine
	{
	}

	public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : ICriticalNotifyCompletion where TStateMachine : IAsyncStateMachine
	{
	}

	public static AsyncTaskMethodBuilder Create()
	{
		throw null;
	}

	public void SetException(Exception exception)
	{
	}

	public void SetResult()
	{
	}

	public void SetStateMachine(IAsyncStateMachine stateMachine)
	{
	}

	public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
	{
	}
}
public struct AsyncTaskMethodBuilder<TResult>
{
	private object _dummy;

	private int _dummyPrimitive;

	public Task<TResult> Task
	{
		get
		{
			throw null;
		}
	}

	public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : INotifyCompletion where TStateMachine : IAsyncStateMachine
	{
	}

	public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : ICriticalNotifyCompletion where TStateMachine : IAsyncStateMachine
	{
	}

	public static AsyncTaskMethodBuilder<TResult> Create()
	{
		throw null;
	}

	public void SetException(Exception exception)
	{
	}

	public void SetResult(TResult result)
	{
	}

	public void SetStateMachine(IAsyncStateMachine stateMachine)
	{
	}

	public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
	{
	}
}
