namespace System.Threading.Tasks;

public class ConcurrentExclusiveSchedulerPair
{
	public Task Completion
	{
		get
		{
			throw null;
		}
	}

	public TaskScheduler ConcurrentScheduler
	{
		get
		{
			throw null;
		}
	}

	public TaskScheduler ExclusiveScheduler
	{
		get
		{
			throw null;
		}
	}

	public ConcurrentExclusiveSchedulerPair()
	{
	}

	public ConcurrentExclusiveSchedulerPair(TaskScheduler taskScheduler)
	{
	}

	public ConcurrentExclusiveSchedulerPair(TaskScheduler taskScheduler, int maxConcurrencyLevel)
	{
	}

	public ConcurrentExclusiveSchedulerPair(TaskScheduler taskScheduler, int maxConcurrencyLevel, int maxItemsPerTask)
	{
	}

	public void Complete()
	{
	}
}
