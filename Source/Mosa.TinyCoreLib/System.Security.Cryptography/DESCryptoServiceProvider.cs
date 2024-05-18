using System.ComponentModel;

namespace System.Security.Cryptography;

[EditorBrowsable(EditorBrowsableState.Never)]
[Obsolete("Derived cryptographic types are obsolete. Use the Create method on the base type instead.", DiagnosticId = "SYSLIB0021", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public sealed class DESCryptoServiceProvider : DES
{
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

	public override void GenerateIV()
	{
	}

	public override void GenerateKey()
	{
	}
}
