namespace System.Text;

public abstract class Encoder
{
	public EncoderFallback? Fallback
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public EncoderFallbackBuffer FallbackBuffer
	{
		get
		{
			throw null;
		}
	}

	[CLSCompliant(false)]
	public unsafe virtual void Convert(char* chars, int charCount, byte* bytes, int byteCount, bool flush, out int charsUsed, out int bytesUsed, out bool completed)
	{
		throw null;
	}

	public virtual void Convert(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex, int byteCount, bool flush, out int charsUsed, out int bytesUsed, out bool completed)
	{
		throw null;
	}

	public virtual void Convert(ReadOnlySpan<char> chars, Span<byte> bytes, bool flush, out int charsUsed, out int bytesUsed, out bool completed)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe virtual int GetByteCount(char* chars, int count, bool flush)
	{
		throw null;
	}

	public abstract int GetByteCount(char[] chars, int index, int count, bool flush);

	public virtual int GetByteCount(ReadOnlySpan<char> chars, bool flush)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe virtual int GetBytes(char* chars, int charCount, byte* bytes, int byteCount, bool flush)
	{
		throw null;
	}

	public abstract int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex, bool flush);

	public virtual int GetBytes(ReadOnlySpan<char> chars, Span<byte> bytes, bool flush)
	{
		throw null;
	}

	public virtual void Reset()
	{
	}
}
