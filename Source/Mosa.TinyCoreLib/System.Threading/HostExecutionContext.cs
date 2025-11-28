namespace System.Threading;

public class HostExecutionContext : IDisposable
{
	protected internal object? State
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public HostExecutionContext()
	{
	}

	public HostExecutionContext(object? state)
	{
	}

	public virtual HostExecutionContext CreateCopy()
	{
		throw null;
	}

	public void Dispose()
	{
	}

	public virtual void Dispose(bool disposing)
	{
	}
}
