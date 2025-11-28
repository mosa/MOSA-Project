using System.ComponentModel;
using System.Diagnostics;

namespace System.ServiceProcess;

public class ServiceBase : Component
{
	public const int MaxNameLength = 80;

	[DefaultValue(true)]
	public bool AutoLog
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue(false)]
	public bool CanHandlePowerEvent
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue(false)]
	public bool CanHandleSessionChangeEvent
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue(false)]
	public bool CanPauseAndContinue
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue(false)]
	public bool CanShutdown
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue(true)]
	public bool CanStop
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual EventLog EventLog
	{
		get
		{
			throw null;
		}
	}

	public int ExitCode
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	protected IntPtr ServiceHandle
	{
		get
		{
			throw null;
		}
	}

	public string ServiceName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public void RequestAdditionalTime(TimeSpan time)
	{
	}

	protected override void Dispose(bool disposing)
	{
	}

	protected virtual void OnContinue()
	{
	}

	protected virtual void OnCustomCommand(int command)
	{
	}

	protected virtual void OnPause()
	{
	}

	protected virtual bool OnPowerEvent(PowerBroadcastStatus powerStatus)
	{
		throw null;
	}

	protected virtual void OnSessionChange(SessionChangeDescription changeDescription)
	{
	}

	protected virtual void OnShutdown()
	{
	}

	protected virtual void OnStart(string[] args)
	{
	}

	protected virtual void OnStop()
	{
	}

	public void RequestAdditionalTime(int milliseconds)
	{
	}

	public static void Run(ServiceBase service)
	{
	}

	public static void Run(ServiceBase[] services)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ServiceMainCallback(int argCount, IntPtr argPointer)
	{
	}

	public void Stop()
	{
	}
}
