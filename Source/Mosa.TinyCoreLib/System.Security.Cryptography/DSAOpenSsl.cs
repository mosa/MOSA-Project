using System.Runtime.Versioning;

namespace System.Security.Cryptography;

public sealed class DSAOpenSsl : DSA
{
	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	[UnsupportedOSPlatform("windows")]
	public DSAOpenSsl()
	{
	}

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	[UnsupportedOSPlatform("windows")]
	public DSAOpenSsl(int keySize)
	{
	}

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	[UnsupportedOSPlatform("windows")]
	public DSAOpenSsl(IntPtr handle)
	{
	}

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	[UnsupportedOSPlatform("windows")]
	public DSAOpenSsl(DSAParameters parameters)
	{
	}

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	[UnsupportedOSPlatform("windows")]
	public DSAOpenSsl(SafeEvpPKeyHandle pkeyHandle)
	{
	}

	public override byte[] CreateSignature(byte[] rgbHash)
	{
		throw null;
	}

	public SafeEvpPKeyHandle DuplicateKeyHandle()
	{
		throw null;
	}

	public override DSAParameters ExportParameters(bool includePrivateParameters)
	{
		throw null;
	}

	public override void ImportParameters(DSAParameters parameters)
	{
	}

	public override bool VerifySignature(byte[] rgbHash, byte[] rgbSignature)
	{
		throw null;
	}
}
