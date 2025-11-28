using System.Runtime.Versioning;

namespace System.Security.Cryptography;

public sealed class RSAOpenSsl : RSA
{
	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	[UnsupportedOSPlatform("windows")]
	public RSAOpenSsl()
	{
	}

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	[UnsupportedOSPlatform("windows")]
	public RSAOpenSsl(int keySize)
	{
	}

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	[UnsupportedOSPlatform("windows")]
	public RSAOpenSsl(IntPtr handle)
	{
	}

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	[UnsupportedOSPlatform("windows")]
	public RSAOpenSsl(RSAParameters parameters)
	{
	}

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	[UnsupportedOSPlatform("windows")]
	public RSAOpenSsl(SafeEvpPKeyHandle pkeyHandle)
	{
	}

	public SafeEvpPKeyHandle DuplicateKeyHandle()
	{
		throw null;
	}

	public override RSAParameters ExportParameters(bool includePrivateParameters)
	{
		throw null;
	}

	public override void ImportParameters(RSAParameters parameters)
	{
	}
}
