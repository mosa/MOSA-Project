using System.IO;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;

namespace System.Security.Cryptography;

public class HMACMD5 : HMAC
{
	public const int HashSizeInBits = 128;

	public const int HashSizeInBytes = 16;

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

	[UnsupportedOSPlatform("browser")]
	public HMACMD5()
	{
	}

	[UnsupportedOSPlatform("browser")]
	public HMACMD5(byte[] key)
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

	[UnsupportedOSPlatform("browser")]
	public static byte[] HashData(byte[] key, byte[] source)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public static byte[] HashData(byte[] key, Stream source)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public static byte[] HashData(ReadOnlySpan<byte> key, Stream source)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public static int HashData(ReadOnlySpan<byte> key, Stream source, Span<byte> destination)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public static byte[] HashData(ReadOnlySpan<byte> key, ReadOnlySpan<byte> source)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public static int HashData(ReadOnlySpan<byte> key, ReadOnlySpan<byte> source, Span<byte> destination)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public static ValueTask<byte[]> HashDataAsync(byte[] key, Stream source, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public static ValueTask<int> HashDataAsync(ReadOnlyMemory<byte> key, Stream source, Memory<byte> destination, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
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

	[UnsupportedOSPlatform("browser")]
	public static bool TryHashData(ReadOnlySpan<byte> key, ReadOnlySpan<byte> source, Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	protected override bool TryHashFinal(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}
}
