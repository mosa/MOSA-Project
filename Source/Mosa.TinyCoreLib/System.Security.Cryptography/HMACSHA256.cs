using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Security.Cryptography;

public class HMACSHA256 : HMAC
{
	public const int HashSizeInBits = 256;

	public const int HashSizeInBytes = 32;

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

	public HMACSHA256()
	{
	}

	public HMACSHA256(byte[] key)
	{
	}

	protected override void Dispose(bool disposing)
	{
	}

	protected override void HashCore(byte[] rgb, int ib, int cb)
	{
	}

	protected override void HashCore(ReadOnlySpan<byte> source)
	{
	}

	public static byte[] HashData(byte[] key, byte[] source)
	{
		throw null;
	}

	public static byte[] HashData(byte[] key, Stream source)
	{
		throw null;
	}

	public static byte[] HashData(ReadOnlySpan<byte> key, Stream source)
	{
		throw null;
	}

	public static int HashData(ReadOnlySpan<byte> key, Stream source, Span<byte> destination)
	{
		throw null;
	}

	public static byte[] HashData(ReadOnlySpan<byte> key, ReadOnlySpan<byte> source)
	{
		throw null;
	}

	public static int HashData(ReadOnlySpan<byte> key, ReadOnlySpan<byte> source, Span<byte> destination)
	{
		throw null;
	}

	public static ValueTask<byte[]> HashDataAsync(byte[] key, Stream source, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static ValueTask<int> HashDataAsync(ReadOnlyMemory<byte> key, Stream source, Memory<byte> destination, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static ValueTask<byte[]> HashDataAsync(ReadOnlyMemory<byte> key, Stream source, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	protected override byte[] HashFinal()
	{
		throw null;
	}

	public override void Initialize()
	{
	}

	public static bool TryHashData(ReadOnlySpan<byte> key, ReadOnlySpan<byte> source, Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	protected override bool TryHashFinal(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}
}
