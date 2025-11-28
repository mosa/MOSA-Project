namespace System.IO.Hashing;

public sealed class XxHash32 : NonCryptographicHashAlgorithm
{
	public XxHash32()
		: base(0)
	{
	}

	public XxHash32(int seed)
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

	public static byte[] Hash(byte[] source)
	{
		throw null;
	}

	public static byte[] Hash(byte[] source, int seed)
	{
		throw null;
	}

	public static byte[] Hash(ReadOnlySpan<byte> source, int seed = 0)
	{
		throw null;
	}

	public static int Hash(ReadOnlySpan<byte> source, Span<byte> destination, int seed = 0)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static uint HashToUInt32(ReadOnlySpan<byte> source, int seed = 0)
	{
		throw null;
	}

	public override void Reset()
	{
	}

	public static bool TryHash(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesWritten, int seed = 0)
	{
		throw null;
	}
}
