namespace System.ComponentModel;

public class RunWorkerCompletedEventArgs : AsyncCompletedEventArgs
{
	public object? Result
	{
		get
		{
			throw null;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public new object? UserState
	{
		get
		{
			throw null;
		}
	}

	public RunWorkerCompletedEventArgs(object? result, Exception? error, bool cancelled)
		: base(null, cancelled: false, null)
	{
	}
}
