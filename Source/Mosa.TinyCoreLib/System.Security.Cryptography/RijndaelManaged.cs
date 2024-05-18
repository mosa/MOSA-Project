using System.ComponentModel;
using System.Runtime.Versioning;

namespace System.Security.Cryptography;

[EditorBrowsable(EditorBrowsableState.Never)]
[Obsolete("The Rijndael and RijndaelManaged types are obsolete. Use Aes instead.", DiagnosticId = "SYSLIB0022", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
[UnsupportedOSPlatform("browser")]
public sealed class RijndaelManaged : Rijndael
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
