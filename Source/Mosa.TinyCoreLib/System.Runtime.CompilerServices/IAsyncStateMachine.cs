namespace System.Runtime.CompilerServices;

public interface IAsyncStateMachine
{
	void MoveNext();

	void SetStateMachine(IAsyncStateMachine stateMachine);
}
