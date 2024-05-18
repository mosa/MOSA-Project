namespace System.IO.Hashing;

public sealed class Crc64 : NonCryptographicHashAlgorithm
{
	public Crc64()
		: base(0)
	{
	}

	public override void Append(ReadOnlySpan<byte> source)
	{
	}

	[CLSCompliant(false)]
	public ulong GetCurrentHashAsUInt64()
	{
		throw null;
	}

	protected override void GetCurrentHashCore(Span<byte> destination)
	{
	}

	protected override void GetHashAndResetCore(Span<byte> destination)
	{
	}

	public static byte[] Hash(byte[] source)
	{
		throw null;
	}

	public static byte[] Hash(ReadOnlySpan<byte> source)
	{
		throw null;
	}

	public static int Hash(ReadOnlySpan<byte> source, Span<byte> destination)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ulong HashToUInt64(ReadOnlySpan<byte> source)
	{
		throw null;
	}

	public override void Reset()
	{
	}

	public static bool TryHash(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}
}
