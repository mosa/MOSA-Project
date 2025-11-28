namespace System.Runtime.CompilerServices;

public interface INotifyCompletion
{
	void OnCompleted(Action continuation);
}
