using System.ComponentModel;
using System.Runtime.Versioning;

namespace System.Security.Cryptography;

[EditorBrowsable(EditorBrowsableState.Never)]
[Obsolete("Derived cryptographic types are obsolete. Use the Create method on the base type instead.", DiagnosticId = "SYSLIB0021", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public sealed class AesCryptoServiceProvider : Aes
{
	public override int BlockSize
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override int FeedbackSize
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override byte[] IV
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override byte[] Key
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override int KeySize
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override KeySizes[] LegalBlockSizes
	{
		get
		{
			throw null;
		}
	}

	public override KeySizes[] LegalKeySizes
	{
		get
		{
			throw null;
		}
	}

	public override CipherMode Mode
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override PaddingMode Padding
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[UnsupportedOSPlatform("browser")]
	public AesCryptoServiceProvider()
	{
	}

	public override ICryptoTransform CreateDecryptor()
	{
		throw null;
	}

	public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[]? rgbIV)
	{
		throw null;
	}

	public override ICryptoTransform CreateEncryptor()
	{
		throw null;
	}

	public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[]? rgbIV)
	{
		throw null;
	}

	protected override void Dispose(bool disposing)
	{
	}

	public override void GenerateIV()
	{
	}

	public override void GenerateKey()
	{
	}
}
