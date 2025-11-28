using System.ComponentModel;
using System.Runtime.Versioning;

namespace System.Diagnostics;

[Designer("System.Diagnostics.Design.ProcessThreadDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
public class ProcessThread : Component
{
	public int BasePriority
	{
		get
		{
			throw null;
		}
	}

	public int CurrentPriority
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

	public int IdealProcessor
	{
		set
		{
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

	public ThreadPriorityLevel PriorityLevel
	{
		[SupportedOSPlatform("windows")]
		[SupportedOSPlatform("linux")]
		[SupportedOSPlatform("freebsd")]
		get
		{
			throw null;
		}
		[SupportedOSPlatform("windows")]
		set
		{
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

	[SupportedOSPlatform("windows")]
	public IntPtr ProcessorAffinity
	{
		set
		{
		}
	}

	public IntPtr StartAddress
	{
		get
		{
			throw null;
		}
	}

	[SupportedOSPlatform("windows")]
	[SupportedOSPlatform("linux")]
	public DateTime StartTime
	{
		get
		{
			throw null;
		}
	}

	public ThreadState ThreadState
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

	public ThreadWaitReason WaitReason
	{
		get
		{
			throw null;
		}
	}

	internal ProcessThread()
	{
	}

	public void ResetIdealProcessor()
	{
	}
}
