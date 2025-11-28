using System.ComponentModel;

namespace System.Security.Cryptography;

[EditorBrowsable(EditorBrowsableState.Never)]
[Obsolete("Derived cryptographic types are obsolete. Use the Create method on the base type instead.", DiagnosticId = "SYSLIB0021", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public sealed class SHA512Managed : SHA512
{
	protected sealed override void Dispose(bool disposing)
	{
	}

	protected sealed override void HashCore(byte[] array, int ibStart, int cbSize)
	{
	}

	protected sealed override void HashCore(ReadOnlySpan<byte> source)
	{
	}

	protected sealed override byte[] HashFinal()
	{
		throw null;
	}

	public sealed override void Initialize()
	{
	}

	protected sealed override bool TryHashFinal(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}
}
