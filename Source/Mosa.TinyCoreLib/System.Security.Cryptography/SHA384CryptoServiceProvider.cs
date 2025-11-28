using System.ComponentModel;

namespace System.Security.Cryptography;

[EditorBrowsable(EditorBrowsableState.Never)]
[Obsolete("Derived cryptographic types are obsolete. Use the Create method on the base type instead.", DiagnosticId = "SYSLIB0021", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public sealed class SHA384CryptoServiceProvider : SHA384
{
	protected override void Dispose(bool disposing)
	{
	}

	protected override void HashCore(byte[] array, int ibStart, int cbSize)
	{
	}

	protected override void HashCore(ReadOnlySpan<byte> source)
	{
	}

	protected override byte[] HashFinal()
	{
		throw null;
	}

	public override void Initialize()
	{
	}

	protected override bool TryHashFinal(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}
}
