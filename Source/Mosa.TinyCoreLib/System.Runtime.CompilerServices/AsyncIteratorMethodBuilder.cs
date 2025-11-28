namespace System.Runtime.CompilerServices;

public struct AsyncIteratorMethodBuilder
{
	private object _dummy;

	private int _dummyPrimitive;

	public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : INotifyCompletion where TStateMachine : IAsyncStateMachine
	{
	}

	public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : ICriticalNotifyCompletion where TStateMachine : IAsyncStateMachine
	{
	}

	public void Complete()
	{
	}

	public static AsyncIteratorMethodBuilder Create()
	{
		throw null;
	}

	public void MoveNext<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
	{
	}
}
