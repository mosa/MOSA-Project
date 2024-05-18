namespace System.Runtime.CompilerServices;

public interface ICriticalNotifyCompletion : INotifyCompletion
{
	void UnsafeOnCompleted(Action continuation);
}
