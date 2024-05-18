using System.Threading.Tasks;

namespace System.Runtime.CompilerServices;

public struct PoolingAsyncValueTaskMethodBuilder
{
	private object _dummy;

	private int _dummyPrimitive;

	public ValueTask Task
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

	public static PoolingAsyncValueTaskMethodBuilder Create()
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
public struct PoolingAsyncValueTaskMethodBuilder<TResult>
{
	private TResult _result;

	private object _dummy;

	private int _dummyPrimitive;

	public ValueTask<TResult> Task
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

	public static PoolingAsyncValueTaskMethodBuilder<TResult> Create()
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
