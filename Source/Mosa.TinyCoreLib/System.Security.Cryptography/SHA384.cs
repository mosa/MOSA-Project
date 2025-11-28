using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Security.Cryptography;

public abstract class SHA384 : HashAlgorithm
{
	public const int HashSizeInBits = 384;

	public const int HashSizeInBytes = 48;

	public new static SHA384 Create()
	{
		throw null;
	}

	[RequiresUnreferencedCode("The default algorithm implementations might be removed, use strong type references like 'RSA.Create()' instead.")]
	[Obsolete("Cryptographic factory methods accepting an algorithm name are obsolete. Use the parameterless Create factory method on the algorithm type instead.", DiagnosticId = "SYSLIB0045", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public new static SHA384? Create(string hashName)
	{
		throw null;
	}

	public static byte[] HashData(byte[] source)
	{
		throw null;
	}

	public static byte[] HashData(Stream source)
	{
		throw null;
	}

	public static int HashData(Stream source, Span<byte> destination)
	{
		throw null;
	}

	public static byte[] HashData(ReadOnlySpan<byte> source)
	{
		throw null;
	}

	public static int HashData(ReadOnlySpan<byte> source, Span<byte> destination)
	{
		throw null;
	}

	public static ValueTask<int> HashDataAsync(Stream source, Memory<byte> destination, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static ValueTask<byte[]> HashDataAsync(Stream source, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static bool TryHashData(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}
}
