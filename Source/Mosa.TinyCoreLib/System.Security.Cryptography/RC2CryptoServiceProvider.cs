using System.ComponentModel;
using System.Runtime.Versioning;

namespace System.Security.Cryptography;

[EditorBrowsable(EditorBrowsableState.Never)]
[Obsolete("Derived cryptographic types are obsolete. Use the Create method on the base type instead.", DiagnosticId = "SYSLIB0021", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public sealed class RC2CryptoServiceProvider : RC2
{
	public override int EffectiveKeySize
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool UseSalt
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

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	public RC2CryptoServiceProvider()
	{
	}

	public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[]? rgbIV)
	{
		throw null;
	}

	public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[]? rgbIV)
	{
		throw null;
	}

	public override void GenerateIV()
	{
	}

	public override void GenerateKey()
	{
	}
}
