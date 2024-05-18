using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Security.Cryptography;

public abstract class SHA3_256 : HashAlgorithm
{
	public const int HashSizeInBits = 256;

	public const int HashSizeInBytes = 32;

	public static bool IsSupported
	{
		get
		{
			throw null;
		}
	}

	public new static SHA3_256 Create()
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
