using System.Runtime.Versioning;

namespace System.Net;

public class HttpListenerTimeoutManager
{
	public TimeSpan DrainEntityBody
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public TimeSpan EntityBody
	{
		get
		{
			throw null;
		}
		[SupportedOSPlatform("windows")]
		set
		{
		}
	}

	public TimeSpan HeaderWait
	{
		get
		{
			throw null;
		}
		[SupportedOSPlatform("windows")]
		set
		{
		}
	}

	public TimeSpan IdleConnection
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public long MinSendBytesPerSecond
	{
		get
		{
			throw null;
		}
		[SupportedOSPlatform("windows")]
		set
		{
		}
	}

	public TimeSpan RequestQueue
	{
		get
		{
			throw null;
		}
		[SupportedOSPlatform("windows")]
		set
		{
		}
	}

	internal HttpListenerTimeoutManager()
	{
	}
}
