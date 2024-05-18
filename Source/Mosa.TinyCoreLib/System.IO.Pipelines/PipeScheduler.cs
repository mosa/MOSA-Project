namespace System.IO.Pipelines;

public abstract class PipeScheduler
{
	public static PipeScheduler Inline
	{
		get
		{
			throw null;
		}
	}

	public static PipeScheduler ThreadPool
	{
		get
		{
			throw null;
		}
	}

	public abstract void Schedule(Action<object?> action, object? state);
}
