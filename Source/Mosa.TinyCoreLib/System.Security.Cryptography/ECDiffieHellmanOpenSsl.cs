using System.Runtime.Versioning;

namespace System.Security.Cryptography;

public sealed class ECDiffieHellmanOpenSsl : ECDiffieHellman
{
	public override ECDiffieHellmanPublicKey PublicKey
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
	public ECDiffieHellmanOpenSsl()
	{
	}

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	[UnsupportedOSPlatform("windows")]
	public ECDiffieHellmanOpenSsl(int keySize)
	{
	}

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	[UnsupportedOSPlatform("windows")]
	public ECDiffieHellmanOpenSsl(IntPtr handle)
	{
	}

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	[UnsupportedOSPlatform("windows")]
	public ECDiffieHellmanOpenSsl(ECCurve curve)
	{
	}

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	[UnsupportedOSPlatform("windows")]
	public ECDiffieHellmanOpenSsl(SafeEvpPKeyHandle pkeyHandle)
	{
	}

	public SafeEvpPKeyHandle DuplicateKeyHandle()
	{
		throw null;
	}

	public override ECParameters ExportParameters(bool includePrivateParameters)
	{
		throw null;
	}

	public override void ImportParameters(ECParameters parameters)
	{
	}
}
