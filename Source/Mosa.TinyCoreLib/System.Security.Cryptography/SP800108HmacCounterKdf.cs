namespace System.Security.Cryptography;

public sealed class SP800108HmacCounterKdf : IDisposable
{
	public SP800108HmacCounterKdf(byte[] key, HashAlgorithmName hashAlgorithm)
	{
	}

	public SP800108HmacCounterKdf(ReadOnlySpan<byte> key, HashAlgorithmName hashAlgorithm)
	{
	}

	public static byte[] DeriveBytes(byte[] key, HashAlgorithmName hashAlgorithm, byte[] label, byte[] context, int derivedKeyLengthInBytes)
	{
		throw null;
	}

	public static byte[] DeriveBytes(byte[] key, HashAlgorithmName hashAlgorithm, string label, string context, int derivedKeyLengthInBytes)
	{
		throw null;
	}

	public static byte[] DeriveBytes(ReadOnlySpan<byte> key, HashAlgorithmName hashAlgorithm, ReadOnlySpan<byte> label, ReadOnlySpan<byte> context, int derivedKeyLengthInBytes)
	{
		throw null;
	}

	public static void DeriveBytes(ReadOnlySpan<byte> key, HashAlgorithmName hashAlgorithm, ReadOnlySpan<byte> label, ReadOnlySpan<byte> context, Span<byte> destination)
	{
	}

	public static byte[] DeriveBytes(ReadOnlySpan<byte> key, HashAlgorithmName hashAlgorithm, ReadOnlySpan<char> label, ReadOnlySpan<char> context, int derivedKeyLengthInBytes)
	{
		throw null;
	}

	public static void DeriveBytes(ReadOnlySpan<byte> key, HashAlgorithmName hashAlgorithm, ReadOnlySpan<char> label, ReadOnlySpan<char> context, Span<byte> destination)
	{
	}

	public byte[] DeriveKey(byte[] label, byte[] context, int derivedKeyLengthInBytes)
	{
		throw null;
	}

	public byte[] DeriveKey(ReadOnlySpan<byte> label, ReadOnlySpan<byte> context, int derivedKeyLengthInBytes)
	{
		throw null;
	}

	public void DeriveKey(ReadOnlySpan<byte> label, ReadOnlySpan<byte> context, Span<byte> destination)
	{
	}

	public byte[] DeriveKey(ReadOnlySpan<char> label, ReadOnlySpan<char> context, int derivedKeyLengthInBytes)
	{
		throw null;
	}

	public void DeriveKey(ReadOnlySpan<char> label, ReadOnlySpan<char> context, Span<byte> destination)
	{
	}

	public byte[] DeriveKey(string label, string context, int derivedKeyLengthInBytes)
	{
		throw null;
	}

	public void Dispose()
	{
	}
}
