namespace System.IO.Hashing;

public sealed class Crc32 : NonCryptographicHashAlgorithm
{
	public Crc32()
		: base(0)
	{
	}

	public override void Append(ReadOnlySpan<byte> source)
	{
	}

	[CLSCompliant(false)]
	public uint GetCurrentHashAsUInt32()
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
	public static uint HashToUInt32(ReadOnlySpan<byte> source)
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
