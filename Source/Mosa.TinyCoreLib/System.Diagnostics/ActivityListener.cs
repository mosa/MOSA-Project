namespace System.Diagnostics;

public sealed class ActivityListener : IDisposable
{
	public Action<Activity>? ActivityStarted
	{
		get
		{
			throw null;
		}
		set
		{
			throw null;
		}
	}

	public Action<Activity>? ActivityStopped
	{
		get
		{
			throw null;
		}
		set
		{
			throw null;
		}
	}

	public Func<ActivitySource, bool>? ShouldListenTo
	{
		get
		{
			throw null;
		}
		set
		{
			throw null;
		}
	}

	public SampleActivity<string>? SampleUsingParentId
	{
		get
		{
			throw null;
		}
		set
		{
			throw null;
		}
	}

	public SampleActivity<ActivityContext>? Sample
	{
		get
		{
			throw null;
		}
		set
		{
			throw null;
		}
	}

	public ActivityListener()
	{
		throw null;
	}

	public void Dispose()
	{
		throw null;
	}
}
