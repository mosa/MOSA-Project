namespace System.Threading.Tasks;

public static class TaskExtensions
{
	public static Task Unwrap(this Task<Task> task)
	{
		throw null;
	}

	public static Task<TResult> Unwrap<TResult>(this Task<Task<TResult>> task)
	{
		throw null;
	}
}
