namespace System.ComponentModel;

public interface ISynchronizeInvoke
{
	bool InvokeRequired { get; }

	IAsyncResult BeginInvoke(Delegate method, object?[]? args);

	object? EndInvoke(IAsyncResult result);

	object? Invoke(Delegate method, object?[]? args);
}
