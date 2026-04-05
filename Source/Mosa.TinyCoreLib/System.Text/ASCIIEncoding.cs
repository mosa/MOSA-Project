namespace System.Text;

public class ASCIIEncoding : Encoding
{
	public override bool IsSingleByte => throw new NotImplementedException();

	[CLSCompliant(false)]
	public override unsafe int GetByteCount(char* chars, int count) => throw new NotImplementedException();

	public override int GetByteCount(char[] chars, int index, int count) => throw new NotImplementedException();

	public override int GetByteCount(ReadOnlySpan<char> chars) => throw new NotImplementedException();

	public override int GetByteCount(string chars) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public override unsafe int GetBytes(char* chars, int charCount, byte* bytes, int byteCount) => throw new NotImplementedException();

	public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex) => throw new NotImplementedException();

	public override int GetBytes(ReadOnlySpan<char> chars, Span<byte> bytes) => throw new NotImplementedException();

	public override int GetBytes(string chars, int charIndex, int charCount, byte[] bytes, int byteIndex) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public override unsafe int GetCharCount(byte* bytes, int count) => throw new NotImplementedException();

	public override int GetCharCount(byte[] bytes, int index, int count) => throw new NotImplementedException();

	public override int GetCharCount(ReadOnlySpan<byte> bytes) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public override unsafe int GetChars(byte* bytes, int byteCount, char* chars, int charCount) => throw new NotImplementedException();

	public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex) => throw new NotImplementedException();

	public override int GetChars(ReadOnlySpan<byte> bytes, Span<char> chars) => throw new NotImplementedException();

	public override Decoder GetDecoder() => throw new NotImplementedException();

	public override Encoder GetEncoder() => throw new NotImplementedException();

	public override int GetMaxByteCount(int charCount) => throw new NotImplementedException();

	public override int GetMaxCharCount(int byteCount) => throw new NotImplementedException();

	public override string GetString(byte[] bytes, int byteIndex, int byteCount)
	{
		ArgumentNullException.ThrowIfNull(bytes);
		if (byteIndex < 0 || byteIndex > bytes.Length)
			Internal.Exceptions.Generic.ParameterOutOfRange(nameof(byteIndex));
		if (byteCount < 0 || byteIndex + byteCount > bytes.Length)
			Internal.Exceptions.Generic.ParameterOutOfRange(nameof(byteCount));

		var result = string.Empty;

		for (var index = byteIndex; index < byteIndex + byteCount; index++)
		{
			var b = bytes[index];
			result += b <= 0x7F ? (char)b : '?';
		}

		return result;
	}

	public override bool TryGetBytes(ReadOnlySpan<char> chars, Span<byte> bytes, out int bytesWritten) => throw new NotImplementedException();

	public override bool TryGetChars(ReadOnlySpan<byte> bytes, Span<char> chars, out int charsWritten) => throw new NotImplementedException();
}
