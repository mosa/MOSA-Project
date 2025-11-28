using System.Diagnostics.CodeAnalysis;

namespace System.Text;

public class UTF8Encoding : Encoding
{
	public override ReadOnlySpan<byte> Preamble
	{
		get
		{
			throw null;
		}
	}

	public UTF8Encoding()
	{
	}

	public UTF8Encoding(bool encoderShouldEmitUTF8Identifier)
	{
	}

	public UTF8Encoding(bool encoderShouldEmitUTF8Identifier, bool throwOnInvalidBytes)
	{
	}

	public override bool Equals([NotNullWhen(true)] object? value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe override int GetByteCount(char* chars, int count)
	{
		throw null;
	}

	public override int GetByteCount(char[] chars, int index, int count)
	{
		throw null;
	}

	public override int GetByteCount(ReadOnlySpan<char> chars)
	{
		throw null;
	}

	public override int GetByteCount(string chars)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe override int GetBytes(char* chars, int charCount, byte* bytes, int byteCount)
	{
		throw null;
	}

	public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
	{
		throw null;
	}

	public override int GetBytes(ReadOnlySpan<char> chars, Span<byte> bytes)
	{
		throw null;
	}

	public override int GetBytes(string s, int charIndex, int charCount, byte[] bytes, int byteIndex)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe override int GetCharCount(byte* bytes, int count)
	{
		throw null;
	}

	public override int GetCharCount(byte[] bytes, int index, int count)
	{
		throw null;
	}

	public override int GetCharCount(ReadOnlySpan<byte> bytes)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe override int GetChars(byte* bytes, int byteCount, char* chars, int charCount)
	{
		throw null;
	}

	public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
	{
		throw null;
	}

	public override int GetChars(ReadOnlySpan<byte> bytes, Span<char> chars)
	{
		throw null;
	}

	public override Decoder GetDecoder()
	{
		throw null;
	}

	public override Encoder GetEncoder()
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public override int GetMaxByteCount(int charCount)
	{
		throw null;
	}

	public override int GetMaxCharCount(int byteCount)
	{
		throw null;
	}

	public override byte[] GetPreamble()
	{
		throw null;
	}

	public override string GetString(byte[] bytes, int index, int count)
	{
		throw null;
	}

	public override bool TryGetBytes(ReadOnlySpan<char> chars, Span<byte> bytes, out int bytesWritten)
	{
		throw null;
	}

	public override bool TryGetChars(ReadOnlySpan<byte> bytes, Span<char> chars, out int charsWritten)
	{
		throw null;
	}
}
