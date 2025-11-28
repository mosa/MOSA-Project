namespace System.Security.Cryptography;

public sealed class IncrementalHash : IDisposable
{
	public HashAlgorithmName AlgorithmName
	{
		get
		{
			throw null;
		}
	}

	public int HashLengthInBytes
	{
		get
		{
			throw null;
		}
	}

	internal IncrementalHash()
	{
	}

	public void AppendData(byte[] data)
	{
	}

	public void AppendData(byte[] data, int offset, int count)
	{
	}

	public void AppendData(ReadOnlySpan<byte> data)
	{
	}

	public static IncrementalHash CreateHash(HashAlgorithmName hashAlgorithm)
	{
		throw null;
	}

	public static IncrementalHash CreateHMAC(HashAlgorithmName hashAlgorithm, byte[] key)
	{
		throw null;
	}

	public static IncrementalHash CreateHMAC(HashAlgorithmName hashAlgorithm, ReadOnlySpan<byte> key)
	{
		throw null;
	}

	public void Dispose()
	{
	}

	public byte[] GetCurrentHash()
	{
		throw null;
	}

	public int GetCurrentHash(Span<byte> destination)
	{
		throw null;
	}

	public byte[] GetHashAndReset()
	{
		throw null;
	}

	public int GetHashAndReset(Span<byte> destination)
	{
		throw null;
	}

	public bool TryGetCurrentHash(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	public bool TryGetHashAndReset(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}
}
