using System.Runtime.Versioning;

namespace System.Security.Cryptography;

public sealed class ECDsaOpenSsl : ECDsa
{
	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	[UnsupportedOSPlatform("windows")]
	public ECDsaOpenSsl()
	{
	}

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	[UnsupportedOSPlatform("windows")]
	public ECDsaOpenSsl(int keySize)
	{
	}

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	[UnsupportedOSPlatform("windows")]
	public ECDsaOpenSsl(IntPtr handle)
	{
	}

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	[UnsupportedOSPlatform("windows")]
	public ECDsaOpenSsl(ECCurve curve)
	{
	}

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	[UnsupportedOSPlatform("windows")]
	public ECDsaOpenSsl(SafeEvpPKeyHandle pkeyHandle)
	{
	}

	public SafeEvpPKeyHandle DuplicateKeyHandle()
	{
		throw null;
	}

	public override byte[] SignHash(byte[] hash)
	{
		throw null;
	}

	public override bool VerifyHash(byte[] hash, byte[] signature)
	{
		throw null;
	}
}
