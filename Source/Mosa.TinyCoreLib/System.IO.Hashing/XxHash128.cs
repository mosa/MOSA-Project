namespace System.IO.Hashing;

public sealed class XxHash128 : NonCryptographicHashAlgorithm
{
	public XxHash128()
		: base(0)
	{
	}

	public XxHash128(long seed)
		: base(0)
	{
	}

	public override void Append(ReadOnlySpan<byte> source)
	{
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

	public override void Reset()
	{
	}

	public static bool TryHash(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesWritten, long seed = 0L)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public UInt128 GetCurrentHashAsUInt128()
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static UInt128 HashToUInt128(ReadOnlySpan<byte> source, long seed = 0L)
	{
		throw null;
	}
}
