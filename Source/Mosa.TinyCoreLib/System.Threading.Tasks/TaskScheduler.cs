using System.Collections.Generic;

namespace System.Threading.Tasks;

public abstract class TaskScheduler
{
	public static TaskScheduler Current
	{
		get
		{
			throw null;
		}
	}

	public static TaskScheduler Default
	{
		get
		{
			throw null;
		}
	}

	public int Id
	{
		get
		{
			throw null;
		}
	}

	public virtual int MaximumConcurrencyLevel
	{
		get
		{
			throw null;
		}
	}

	public static event EventHandler<UnobservedTaskExceptionEventArgs>? UnobservedTaskException
	{
		add
		{
		}
		remove
		{
		}
	}

	public static TaskScheduler FromCurrentSynchronizationContext()
	{
		throw null;
	}

	protected abstract IEnumerable<Task>? GetScheduledTasks();

	protected internal abstract void QueueTask(Task task);

	protected internal virtual bool TryDequeue(Task task)
	{
		throw null;
	}

	protected bool TryExecuteTask(Task task)
	{
		throw null;
	}

	protected abstract bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued);
}
