using System.Runtime.Versioning;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Cryptography.X509Certificates;

public class X509Chain : IDisposable
{
	public IntPtr ChainContext
	{
		get
		{
			throw null;
		}
	}

	public X509ChainElementCollection ChainElements
	{
		get
		{
			throw null;
		}
	}

	public X509ChainPolicy ChainPolicy
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public X509ChainStatus[] ChainStatus
	{
		get
		{
			throw null;
		}
	}

	public SafeX509ChainHandle? SafeHandle
	{
		get
		{
			throw null;
		}
	}

	public X509Chain()
	{
	}

	public X509Chain(bool useMachineContext)
	{
	}

	[SupportedOSPlatform("windows")]
	public X509Chain(IntPtr chainContext)
	{
	}

	[UnsupportedOSPlatform("browser")]
	public bool Build(X509Certificate2 certificate)
	{
		throw null;
	}

	public static X509Chain Create()
	{
		throw null;
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public void Reset()
	{
	}
}
