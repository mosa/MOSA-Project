namespace System.Threading.Tasks;

public static class TaskToAsyncResult
{
	public static IAsyncResult Begin(Task task, AsyncCallback? callback, object? state)
	{
		throw null;
	}

	public static void End(IAsyncResult asyncResult)
	{
		throw null;
	}

	public static TResult End<TResult>(IAsyncResult asyncResult)
	{
		throw null;
	}

	public static Task Unwrap(IAsyncResult asyncResult)
	{
		throw null;
	}

	public static Task<TResult> Unwrap<TResult>(IAsyncResult asyncResult)
	{
		throw null;
	}
}
