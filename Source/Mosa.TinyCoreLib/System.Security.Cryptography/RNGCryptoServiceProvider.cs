using System.ComponentModel;

namespace System.Security.Cryptography;

[EditorBrowsable(EditorBrowsableState.Never)]
[Obsolete("RNGCryptoServiceProvider is obsolete. To generate a random number, use one of the RandomNumberGenerator static methods instead.", DiagnosticId = "SYSLIB0023", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public sealed class RNGCryptoServiceProvider : RandomNumberGenerator
{
	public RNGCryptoServiceProvider()
	{
	}

	public RNGCryptoServiceProvider(byte[] rgb)
	{
	}

	public RNGCryptoServiceProvider(CspParameters? cspParams)
	{
	}

	public RNGCryptoServiceProvider(string str)
	{
	}

	protected override void Dispose(bool disposing)
	{
	}

	public override void GetBytes(byte[] data)
	{
	}

	public override void GetBytes(byte[] data, int offset, int count)
	{
	}

	public override void GetBytes(Span<byte> data)
	{
	}

	public override void GetNonZeroBytes(byte[] data)
	{
	}

	public override void GetNonZeroBytes(Span<byte> data)
	{
	}
}
