using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Security.Cryptography;

public sealed class Shake128 : IDisposable
{
	public static bool IsSupported
	{
		get
		{
			throw null;
		}
	}

	public void AppendData(byte[] data)
	{
	}

	public void AppendData(ReadOnlySpan<byte> data)
	{
	}

	public void Dispose()
	{
	}

	public byte[] GetCurrentHash(int outputLength)
	{
		throw null;
	}

	public void GetCurrentHash(Span<byte> destination)
	{
	}

	public byte[] GetHashAndReset(int outputLength)
	{
		throw null;
	}

	public void GetHashAndReset(Span<byte> destination)
	{
	}

	public static byte[] HashData(byte[] source, int outputLength)
	{
		throw null;
	}

	public static byte[] HashData(Stream source, int outputLength)
	{
		throw null;
	}

	public static void HashData(Stream source, Span<byte> destination)
	{
	}

	public static byte[] HashData(ReadOnlySpan<byte> source, int outputLength)
	{
		throw null;
	}

	public static void HashData(ReadOnlySpan<byte> source, Span<byte> destination)
	{
	}

	public static ValueTask<byte[]> HashDataAsync(Stream source, int outputLength, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static ValueTask HashDataAsync(Stream source, Memory<byte> destination, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}
}
