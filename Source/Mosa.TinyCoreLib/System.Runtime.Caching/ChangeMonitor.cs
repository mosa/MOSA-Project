namespace System.Runtime.Caching;

public abstract class ChangeMonitor : IDisposable
{
	public bool HasChanged
	{
		get
		{
			throw null;
		}
	}

	public bool IsDisposed
	{
		get
		{
			throw null;
		}
	}

	public abstract string UniqueId { get; }

	public void Dispose()
	{
	}

	protected abstract void Dispose(bool disposing);

	protected void InitializationComplete()
	{
	}

	public void NotifyOnChanged(OnChangedCallback onChangedCallback)
	{
	}

	protected void OnChanged(object state)
	{
	}
}
