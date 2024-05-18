namespace System.Runtime.CompilerServices;

public struct AsyncVoidMethodBuilder
{
	private object _dummy;

	private int _dummyPrimitive;

	public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : INotifyCompletion where TStateMachine : IAsyncStateMachine
	{
	}

	public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : ICriticalNotifyCompletion where TStateMachine : IAsyncStateMachine
	{
	}

	public static AsyncVoidMethodBuilder Create()
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
