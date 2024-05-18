namespace System.IO.Hashing;

public sealed class XxHash64 : NonCryptographicHashAlgorithm
{
	public XxHash64()
		: base(0)
	{
	}

	public XxHash64(long seed)
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

	public static byte[] Hash(byte[] source)
	{
		throw null;
	}

	public static byte[] Hash(byte[] source, long seed)
	{
		throw null;
	}

	public static byte[] Hash(ReadOnlySpan<byte> source, long seed = 0L)
	{
		throw null;
	}

	public static int Hash(ReadOnlySpan<byte> source, Span<byte> destination, long seed = 0L)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ulong HashToUInt64(ReadOnlySpan<byte> source, long seed = 0L)
	{
		throw null;
	}

	public override void Reset()
	{
	}

	public static bool TryHash(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesWritten, long seed = 0L)
	{
		throw null;
	}
}
