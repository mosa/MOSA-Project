using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.Versioning;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace System.Diagnostics;

[Designer("System.Diagnostics.Design.ProcessDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
public class Process : Component, IDisposable
{
	public int BasePriority
	{
		get
		{
			throw null;
		}
	}

	public bool EnableRaisingEvents
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int ExitCode
	{
		get
		{
			throw null;
		}
	}

	public DateTime ExitTime
	{
		get
		{
			throw null;
		}
	}

	public IntPtr Handle
	{
		get
		{
			throw null;
		}
	}

	public int HandleCount
	{
		get
		{
			throw null;
		}
	}

	public bool HasExited
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

	public string MachineName
	{
		get
		{
			throw null;
		}
	}

	public ProcessModule? MainModule
	{
		get
		{
			throw null;
		}
	}

	public IntPtr MainWindowHandle
	{
		get
		{
			throw null;
		}
	}

	public string MainWindowTitle
	{
		get
		{
			throw null;
		}
	}

	public IntPtr MaxWorkingSet
	{
		[UnsupportedOSPlatform("ios")]
		[UnsupportedOSPlatform("tvos")]
		[SupportedOSPlatform("maccatalyst")]
		get
		{
			throw null;
		}
		[SupportedOSPlatform("freebsd")]
		[SupportedOSPlatform("macos")]
		[SupportedOSPlatform("maccatalyst")]
		[SupportedOSPlatform("windows")]
		set
		{
		}
	}

	public IntPtr MinWorkingSet
	{
		[UnsupportedOSPlatform("ios")]
		[UnsupportedOSPlatform("tvos")]
		[SupportedOSPlatform("maccatalyst")]
		get
		{
			throw null;
		}
		[SupportedOSPlatform("freebsd")]
		[SupportedOSPlatform("macos")]
		[SupportedOSPlatform("maccatalyst")]
		[SupportedOSPlatform("windows")]
		set
		{
		}
	}

	public ProcessModuleCollection Modules
	{
		get
		{
			throw null;
		}
	}

	[Obsolete("Process.NonpagedSystemMemorySize has been deprecated because the type of the property can't represent all valid results. Use System.Diagnostics.Process.NonpagedSystemMemorySize64 instead.")]
	public int NonpagedSystemMemorySize
	{
		get
		{
			throw null;
		}
	}

	public long NonpagedSystemMemorySize64
	{
		get
		{
			throw null;
		}
	}

	[Obsolete("Process.PagedMemorySize has been deprecated because the type of the property can't represent all valid results. Use System.Diagnostics.Process.PagedMemorySize64 instead.")]
	public int PagedMemorySize
	{
		get
		{
			throw null;
		}
	}

	public long PagedMemorySize64
	{
		get
		{
			throw null;
		}
	}

	[Obsolete("Process.PagedSystemMemorySize has been deprecated because the type of the property can't represent all valid results. Use System.Diagnostics.Process.PagedSystemMemorySize64 instead.")]
	public int PagedSystemMemorySize
	{
		get
		{
			throw null;
		}
	}

	public long PagedSystemMemorySize64
	{
		get
		{
			throw null;
		}
	}

	[Obsolete("Process.PeakPagedMemorySize has been deprecated because the type of the property can't represent all valid results. Use System.Diagnostics.Process.PeakPagedMemorySize64 instead.")]
	public int PeakPagedMemorySize
	{
		get
		{
			throw null;
		}
	}

	public long PeakPagedMemorySize64
	{
		get
		{
			throw null;
		}
	}

	[Obsolete("Process.PeakVirtualMemorySize has been deprecated because the type of the property can't represent all valid results. Use System.Diagnostics.Process.PeakVirtualMemorySize64 instead.")]
	public int PeakVirtualMemorySize
	{
		get
		{
			throw null;
		}
	}

	public long PeakVirtualMemorySize64
	{
		get
		{
			throw null;
		}
	}

	[Obsolete("Process.PeakWorkingSet has been deprecated because the type of the property can't represent all valid results. Use System.Diagnostics.Process.PeakWorkingSet64 instead.")]
	public int PeakWorkingSet
	{
		get
		{
			throw null;
		}
	}

	public long PeakWorkingSet64
	{
		get
		{
			throw null;
		}
	}

	public bool PriorityBoostEnabled
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ProcessPriorityClass PriorityClass
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[Obsolete("Process.PrivateMemorySize has been deprecated because the type of the property can't represent all valid results. Use System.Diagnostics.Process.PrivateMemorySize64 instead.")]
	public int PrivateMemorySize
	{
		get
		{
			throw null;
		}
	}

	public long PrivateMemorySize64
	{
		get
		{
			throw null;
		}
	}

	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	[SupportedOSPlatform("maccatalyst")]
	public TimeSpan PrivilegedProcessorTime
	{
		get
		{
			throw null;
		}
	}

	public string ProcessName
	{
		get
		{
			throw null;
		}
	}

	[SupportedOSPlatform("windows")]
	[SupportedOSPlatform("linux")]
	public IntPtr ProcessorAffinity
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool Responding
	{
		get
		{
			throw null;
		}
	}

	public SafeProcessHandle SafeHandle
	{
		get
		{
			throw null;
		}
	}

	public int SessionId
	{
		get
		{
			throw null;
		}
	}

	public StreamReader StandardError
	{
		get
		{
			throw null;
		}
	}

	public StreamWriter StandardInput
	{
		get
		{
			throw null;
		}
	}

	public StreamReader StandardOutput
	{
		get
		{
			throw null;
		}
	}

	public ProcessStartInfo StartInfo
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	[SupportedOSPlatform("maccatalyst")]
	public DateTime StartTime
	{
		get
		{
			throw null;
		}
	}

	public ISynchronizeInvoke? SynchronizingObject
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ProcessThreadCollection Threads
	{
		get
		{
			throw null;
		}
	}

	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	[SupportedOSPlatform("maccatalyst")]
	public TimeSpan TotalProcessorTime
	{
		get
		{
			throw null;
		}
	}

	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	[SupportedOSPlatform("maccatalyst")]
	public TimeSpan UserProcessorTime
	{
		get
		{
			throw null;
		}
	}

	[Obsolete("Process.VirtualMemorySize has been deprecated because the type of the property can't represent all valid results. Use System.Diagnostics.Process.VirtualMemorySize64 instead.")]
	public int VirtualMemorySize
	{
		get
		{
			throw null;
		}
	}

	public long VirtualMemorySize64
	{
		get
		{
			throw null;
		}
	}

	[Obsolete("Process.WorkingSet has been deprecated because the type of the property can't represent all valid results. Use System.Diagnostics.Process.WorkingSet64 instead.")]
	public int WorkingSet
	{
		get
		{
			throw null;
		}
	}

	public long WorkingSet64
	{
		get
		{
			throw null;
		}
	}

	public event DataReceivedEventHandler? ErrorDataReceived
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventHandler Exited
	{
		add
		{
		}
		remove
		{
		}
	}

	public event DataReceivedEventHandler? OutputDataReceived
	{
		add
		{
		}
		remove
		{
		}
	}

	public void BeginErrorReadLine()
	{
	}

	public void BeginOutputReadLine()
	{
	}

	public void CancelErrorRead()
	{
	}

	public void CancelOutputRead()
	{
	}

	public void Close()
	{
	}

	public bool CloseMainWindow()
	{
		throw null;
	}

	protected override void Dispose(bool disposing)
	{
	}

	public static void EnterDebugMode()
	{
	}

	public static Process GetCurrentProcess()
	{
		throw null;
	}

	public static Process GetProcessById(int processId)
	{
		throw null;
	}

	public static Process GetProcessById(int processId, string machineName)
	{
		throw null;
	}

	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	[SupportedOSPlatform("maccatalyst")]
	public static Process[] GetProcesses()
	{
		throw null;
	}

	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	[SupportedOSPlatform("maccatalyst")]
	public static Process[] GetProcesses(string machineName)
	{
		throw null;
	}

	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	[SupportedOSPlatform("maccatalyst")]
	public static Process[] GetProcessesByName(string? processName)
	{
		throw null;
	}

	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	[SupportedOSPlatform("maccatalyst")]
	public static Process[] GetProcessesByName(string? processName, string machineName)
	{
		throw null;
	}

	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	[SupportedOSPlatform("maccatalyst")]
	public void Kill()
	{
	}

	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	[SupportedOSPlatform("maccatalyst")]
	public void Kill(bool entireProcessTree)
	{
	}

	public static void LeaveDebugMode()
	{
	}

	protected void OnExited()
	{
	}

	public void Refresh()
	{
	}

	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	[SupportedOSPlatform("maccatalyst")]
	public bool Start()
	{
		throw null;
	}

	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	[SupportedOSPlatform("maccatalyst")]
	public static Process? Start(ProcessStartInfo startInfo)
	{
		throw null;
	}

	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	[SupportedOSPlatform("maccatalyst")]
	public static Process Start(string fileName)
	{
		throw null;
	}

	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	[SupportedOSPlatform("maccatalyst")]
	public static Process Start(string fileName, string arguments)
	{
		throw null;
	}

	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	[SupportedOSPlatform("maccatalyst")]
	public static Process Start(string fileName, IEnumerable<string> arguments)
	{
		throw null;
	}

	[CLSCompliant(false)]
	[SupportedOSPlatform("windows")]
	public static Process? Start(string fileName, string userName, SecureString password, string domain)
	{
		throw null;
	}

	[CLSCompliant(false)]
	[SupportedOSPlatform("windows")]
	public static Process? Start(string fileName, string arguments, string userName, SecureString password, string domain)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}

	public void WaitForExit()
	{
	}

	public bool WaitForExit(int milliseconds)
	{
		throw null;
	}

	public bool WaitForExit(TimeSpan timeout)
	{
		throw null;
	}

	public Task WaitForExitAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public bool WaitForInputIdle()
	{
		throw null;
	}

	public bool WaitForInputIdle(int milliseconds)
	{
		throw null;
	}

	public bool WaitForInputIdle(TimeSpan timeout)
	{
		throw null;
	}
}
