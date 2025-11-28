using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace System.Security.Cryptography;

public sealed class SafeEvpPKeyHandle : SafeHandle
{
	public override bool IsInvalid
	{
		get
		{
			throw null;
		}
	}

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	[UnsupportedOSPlatform("windows")]
	public static long OpenSslVersion
	{
		get
		{
			throw null;
		}
	}

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	[UnsupportedOSPlatform("windows")]
	public SafeEvpPKeyHandle()
		: base((IntPtr)0, ownsHandle: false)
	{
	}

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	[UnsupportedOSPlatform("windows")]
	public SafeEvpPKeyHandle(IntPtr handle, bool ownsHandle)
		: base((IntPtr)0, ownsHandle: false)
	{
	}

	public SafeEvpPKeyHandle DuplicateHandle()
	{
		throw null;
	}

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	[UnsupportedOSPlatform("windows")]
	public static SafeEvpPKeyHandle OpenPrivateKeyFromEngine(string engineName, string keyId)
	{
		throw null;
	}

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	[UnsupportedOSPlatform("windows")]
	public static SafeEvpPKeyHandle OpenPublicKeyFromEngine(string engineName, string keyId)
	{
		throw null;
	}

	protected override bool ReleaseHandle()
	{
		throw null;
	}
}
