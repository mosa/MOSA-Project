using System.Diagnostics.CodeAnalysis;

namespace System.Text;

public class UTF7Encoding : Encoding
{
	[Obsolete("The UTF-7 encoding is insecure and should not be used. Consider using UTF-8 instead.", DiagnosticId = "SYSLIB0001", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public UTF7Encoding()
	{
	}

	[Obsolete("The UTF-7 encoding is insecure and should not be used. Consider using UTF-8 instead.", DiagnosticId = "SYSLIB0001", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public UTF7Encoding(bool allowOptionals)
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

	public override int GetByteCount(string s)
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

	[CLSCompliant(false)]
	public unsafe override int GetChars(byte* bytes, int byteCount, char* chars, int charCount)
	{
		throw null;
	}

	public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
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

	public override string GetString(byte[] bytes, int index, int count)
	{
		throw null;
	}
}
