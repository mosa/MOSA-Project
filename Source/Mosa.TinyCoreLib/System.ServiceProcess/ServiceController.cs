using System.ComponentModel;
using System.Runtime.InteropServices;

namespace System.ServiceProcess;

[Designer("System.ServiceProcess.Design.ServiceControllerDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
public class ServiceController : Component
{
	public bool CanPauseAndContinue
	{
		get
		{
			throw null;
		}
	}

	public bool CanShutdown
	{
		get
		{
			throw null;
		}
	}

	public bool CanStop
	{
		get
		{
			throw null;
		}
	}

	public ServiceController[] DependentServices
	{
		get
		{
			throw null;
		}
	}

	public string DisplayName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string MachineName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SafeHandle ServiceHandle
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

	public ServiceController[] ServicesDependedOn
	{
		get
		{
			throw null;
		}
	}

	public ServiceType ServiceType
	{
		get
		{
			throw null;
		}
	}

	public ServiceStartMode StartType
	{
		get
		{
			throw null;
		}
	}

	public ServiceControllerStatus Status
	{
		get
		{
			throw null;
		}
	}

	public void Stop(bool stopDependentServices)
	{
	}

	public ServiceController()
	{
	}

	public ServiceController(string name)
	{
	}

	public ServiceController(string name, string machineName)
	{
	}

	public void Close()
	{
	}

	public void Continue()
	{
	}

	protected override void Dispose(bool disposing)
	{
	}

	public void ExecuteCommand(int command)
	{
	}

	public static ServiceController[] GetDevices()
	{
		throw null;
	}

	public static ServiceController[] GetDevices(string machineName)
	{
		throw null;
	}

	public static ServiceController[] GetServices()
	{
		throw null;
	}

	public static ServiceController[] GetServices(string machineName)
	{
		throw null;
	}

	public void Pause()
	{
	}

	public void Refresh()
	{
	}

	public void Start()
	{
	}

	public void Start(string[] args)
	{
	}

	public void Stop()
	{
	}

	public void WaitForStatus(ServiceControllerStatus desiredStatus)
	{
	}

	public void WaitForStatus(ServiceControllerStatus desiredStatus, TimeSpan timeout)
	{
	}
}
