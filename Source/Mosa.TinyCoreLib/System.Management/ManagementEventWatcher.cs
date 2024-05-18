using System.ComponentModel;

namespace System.Management;

[ToolboxItem(false)]
public class ManagementEventWatcher : Component
{
	public EventWatcherOptions Options
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public EventQuery Query
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ManagementScope Scope
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public event EventArrivedEventHandler EventArrived
	{
		add
		{
		}
		remove
		{
		}
	}

	public event StoppedEventHandler Stopped
	{
		add
		{
		}
		remove
		{
		}
	}

	public ManagementEventWatcher()
	{
	}

	public ManagementEventWatcher(EventQuery query)
	{
	}

	public ManagementEventWatcher(ManagementScope scope, EventQuery query)
	{
	}

	public ManagementEventWatcher(ManagementScope scope, EventQuery query, EventWatcherOptions options)
	{
	}

	public ManagementEventWatcher(string query)
	{
	}

	public ManagementEventWatcher(string scope, string query)
	{
	}

	public ManagementEventWatcher(string scope, string query, EventWatcherOptions options)
	{
	}

	~ManagementEventWatcher()
	{
	}

	public void Start()
	{
	}

	public void Stop()
	{
	}

	public ManagementBaseObject WaitForNextEvent()
	{
		throw null;
	}
}
