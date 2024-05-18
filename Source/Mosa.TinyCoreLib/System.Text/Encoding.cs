using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace System.Text;

public abstract class Encoding : ICloneable
{
	public static Encoding ASCII
	{
		get
		{
			throw null;
		}
	}

	public static Encoding BigEndianUnicode
	{
		get
		{
			throw null;
		}
	}

	public virtual string BodyName
	{
		get
		{
			throw null;
		}
	}

	public virtual int CodePage
	{
		get
		{
			throw null;
		}
	}

	public DecoderFallback DecoderFallback
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public static Encoding Default
	{
		get
		{
			throw null;
		}
	}

	public EncoderFallback EncoderFallback
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual string EncodingName
	{
		get
		{
			throw null;
		}
	}

	public virtual string HeaderName
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsBrowserDisplay
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsBrowserSave
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsMailNewsDisplay
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsMailNewsSave
	{
		get
		{
			throw null;
		}
	}

	public bool IsReadOnly
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsSingleByte
	{
		get
		{
			throw null;
		}
	}

	public static Encoding Latin1
	{
		get
		{
			throw null;
		}
	}

	public virtual ReadOnlySpan<byte> Preamble
	{
		get
		{
			throw null;
		}
	}

	public static Encoding Unicode
	{
		get
		{
			throw null;
		}
	}

	public static Encoding UTF32
	{
		get
		{
			throw null;
		}
	}

	[Obsolete("The UTF-7 encoding is insecure and should not be used. Consider using UTF-8 instead.", DiagnosticId = "SYSLIB0001", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public static Encoding UTF7
	{
		get
		{
			throw null;
		}
	}

	public static Encoding UTF8
	{
		get
		{
			throw null;
		}
	}

	public virtual string WebName
	{
		get
		{
			throw null;
		}
	}

	public virtual int WindowsCodePage
	{
		get
		{
			throw null;
		}
	}

	protected Encoding()
	{
	}

	protected Encoding(int codePage)
	{
	}

	protected Encoding(int codePage, EncoderFallback? encoderFallback, DecoderFallback? decoderFallback)
	{
	}

	public virtual object Clone()
	{
		throw null;
	}

	public static byte[] Convert(Encoding srcEncoding, Encoding dstEncoding, byte[] bytes)
	{
		throw null;
	}

	public static byte[] Convert(Encoding srcEncoding, Encoding dstEncoding, byte[] bytes, int index, int count)
	{
		throw null;
	}

	public static Stream CreateTranscodingStream(Stream innerStream, Encoding innerStreamEncoding, Encoding outerStreamEncoding, bool leaveOpen = false)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe virtual int GetByteCount(char* chars, int count)
	{
		throw null;
	}

	public virtual int GetByteCount(char[] chars)
	{
		throw null;
	}

	public abstract int GetByteCount(char[] chars, int index, int count);

	public virtual int GetByteCount(ReadOnlySpan<char> chars)
	{
		throw null;
	}

	public virtual int GetByteCount(string s)
	{
		throw null;
	}

	public int GetByteCount(string s, int index, int count)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe virtual int GetBytes(char* chars, int charCount, byte* bytes, int byteCount)
	{
		throw null;
	}

	public virtual byte[] GetBytes(char[] chars)
	{
		throw null;
	}

	public virtual byte[] GetBytes(char[] chars, int index, int count)
	{
		throw null;
	}

	public abstract int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex);

	public virtual int GetBytes(ReadOnlySpan<char> chars, Span<byte> bytes)
	{
		throw null;
	}

	public virtual byte[] GetBytes(string s)
	{
		throw null;
	}

	public byte[] GetBytes(string s, int index, int count)
	{
		throw null;
	}

	public virtual int GetBytes(string s, int charIndex, int charCount, byte[] bytes, int byteIndex)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe virtual int GetCharCount(byte* bytes, int count)
	{
		throw null;
	}

	public virtual int GetCharCount(byte[] bytes)
	{
		throw null;
	}

	public abstract int GetCharCount(byte[] bytes, int index, int count);

	public virtual int GetCharCount(ReadOnlySpan<byte> bytes)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe virtual int GetChars(byte* bytes, int byteCount, char* chars, int charCount)
	{
		throw null;
	}

	public virtual char[] GetChars(byte[] bytes)
	{
		throw null;
	}

	public virtual char[] GetChars(byte[] bytes, int index, int count)
	{
		throw null;
	}

	public abstract int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex);

	public virtual int GetChars(ReadOnlySpan<byte> bytes, Span<char> chars)
	{
		throw null;
	}

	public virtual Decoder GetDecoder()
	{
		throw null;
	}

	public virtual Encoder GetEncoder()
	{
		throw null;
	}

	public static Encoding GetEncoding(int codepage)
	{
		throw null;
	}

	public static Encoding GetEncoding(int codepage, EncoderFallback encoderFallback, DecoderFallback decoderFallback)
	{
		throw null;
	}

	public static Encoding GetEncoding(string name)
	{
		throw null;
	}

	public static Encoding GetEncoding(string name, EncoderFallback encoderFallback, DecoderFallback decoderFallback)
	{
		throw null;
	}

	public static EncodingInfo[] GetEncodings()
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public abstract int GetMaxByteCount(int charCount);

	public abstract int GetMaxCharCount(int byteCount);

	public virtual byte[] GetPreamble()
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe string GetString(byte* bytes, int byteCount)
	{
		throw null;
	}

	public virtual string GetString(byte[] bytes)
	{
		throw null;
	}

	public virtual string GetString(byte[] bytes, int index, int count)
	{
		throw null;
	}

	public string GetString(ReadOnlySpan<byte> bytes)
	{
		throw null;
	}

	public bool IsAlwaysNormalized()
	{
		throw null;
	}

	public virtual bool IsAlwaysNormalized(NormalizationForm form)
	{
		throw null;
	}

	public static void RegisterProvider(EncodingProvider provider)
	{
	}

	public virtual bool TryGetBytes(ReadOnlySpan<char> chars, Span<byte> bytes, out int bytesWritten)
	{
		throw null;
	}

	public virtual bool TryGetChars(ReadOnlySpan<byte> bytes, Span<char> chars, out int charsWritten)
	{
		throw null;
	}
}
