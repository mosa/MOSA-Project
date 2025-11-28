namespace System.Text;

public abstract class Decoder
{
	public DecoderFallback? Fallback
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public DecoderFallbackBuffer FallbackBuffer
	{
		get
		{
			throw null;
		}
	}

	[CLSCompliant(false)]
	public unsafe virtual void Convert(byte* bytes, int byteCount, char* chars, int charCount, bool flush, out int bytesUsed, out int charsUsed, out bool completed)
	{
		throw null;
	}

	public virtual void Convert(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex, int charCount, bool flush, out int bytesUsed, out int charsUsed, out bool completed)
	{
		throw null;
	}

	public virtual void Convert(ReadOnlySpan<byte> bytes, Span<char> chars, bool flush, out int bytesUsed, out int charsUsed, out bool completed)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe virtual int GetCharCount(byte* bytes, int count, bool flush)
	{
		throw null;
	}

	public abstract int GetCharCount(byte[] bytes, int index, int count);

	public virtual int GetCharCount(byte[] bytes, int index, int count, bool flush)
	{
		throw null;
	}

	public virtual int GetCharCount(ReadOnlySpan<byte> bytes, bool flush)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe virtual int GetChars(byte* bytes, int byteCount, char* chars, int charCount, bool flush)
	{
		throw null;
	}

	public abstract int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex);

	public virtual int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex, bool flush)
	{
		throw null;
	}

	public virtual int GetChars(ReadOnlySpan<byte> bytes, Span<char> chars, bool flush)
	{
		throw null;
	}

	public virtual void Reset()
	{
	}
}
