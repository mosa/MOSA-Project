using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace System.Text;

public abstract class Encoding : ICloneable
{
	public static Encoding ASCII { get; } = new ASCIIEncoding();

	public static Encoding BigEndianUnicode => throw new NotImplementedException();

	public virtual string BodyName => throw new NotImplementedException();

	public virtual int CodePage => throw new NotImplementedException();

	public DecoderFallback DecoderFallback
	{
		get => throw new NotImplementedException();
		set => throw new NotImplementedException();
	}

	public static Encoding Default => throw new NotImplementedException();

	public EncoderFallback EncoderFallback
	{
		get => throw new NotImplementedException();
		set => throw new NotImplementedException();
	}

	public virtual string EncodingName => throw new NotImplementedException();

	public virtual string HeaderName => throw new NotImplementedException();

	public virtual bool IsBrowserDisplay => throw new NotImplementedException();

	public virtual bool IsBrowserSave => throw new NotImplementedException();

	public virtual bool IsMailNewsDisplay => throw new NotImplementedException();

	public virtual bool IsMailNewsSave => throw new NotImplementedException();

	public bool IsReadOnly => throw new NotImplementedException();

	public virtual bool IsSingleByte => throw new NotImplementedException();

	public static Encoding Latin1 => throw new NotImplementedException();

	public virtual ReadOnlySpan<byte> Preamble => throw new NotImplementedException();

	public static Encoding Unicode => throw new NotImplementedException();

	public static Encoding UTF32 => throw new NotImplementedException();

	[Obsolete("The UTF-7 encoding is insecure and should not be used. Consider using UTF-8 instead.", DiagnosticId = "SYSLIB0001", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public static Encoding UTF7 => throw new NotImplementedException();

	public static Encoding UTF8 => throw new NotImplementedException();

	public virtual string WebName => throw new NotImplementedException();

	public virtual int WindowsCodePage => throw new NotImplementedException();

	protected Encoding() => throw new NotImplementedException();

	protected Encoding(int codePage) => throw new NotImplementedException();

	protected Encoding(int codePage, EncoderFallback? encoderFallback, DecoderFallback? decoderFallback) => throw new NotImplementedException();

	public virtual object Clone() => throw new NotImplementedException();

	public static byte[] Convert(Encoding srcEncoding, Encoding dstEncoding, byte[] bytes) => throw new NotImplementedException();

	public static byte[] Convert(Encoding srcEncoding, Encoding dstEncoding, byte[] bytes, int index, int count) => throw new NotImplementedException();

	public static Stream CreateTranscodingStream(Stream innerStream, Encoding innerStreamEncoding, Encoding outerStreamEncoding, bool leaveOpen = false) => throw new NotImplementedException();

	public override bool Equals([NotNullWhen(true)] object? value) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public virtual unsafe int GetByteCount(char* chars, int count) => throw new NotImplementedException();

	public virtual int GetByteCount(char[] chars) => throw new NotImplementedException();

	public abstract int GetByteCount(char[] chars, int index, int count);

	public virtual int GetByteCount(ReadOnlySpan<char> chars) => throw new NotImplementedException();

	public virtual int GetByteCount(string s) => throw new NotImplementedException();

	public int GetByteCount(string s, int index, int count) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public virtual unsafe int GetBytes(char* chars, int charCount, byte* bytes, int byteCount) => throw new NotImplementedException();

	public virtual byte[] GetBytes(char[] chars) => throw new NotImplementedException();

	public virtual byte[] GetBytes(char[] chars, int index, int count) => throw new NotImplementedException();

	public abstract int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex);

	public virtual int GetBytes(ReadOnlySpan<char> chars, Span<byte> bytes) => throw new NotImplementedException();

	public virtual byte[] GetBytes(string s) => throw new NotImplementedException();

	public byte[] GetBytes(string s, int index, int count) => throw new NotImplementedException();

	public virtual int GetBytes(string s, int charIndex, int charCount, byte[] bytes, int byteIndex) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public virtual unsafe int GetCharCount(byte* bytes, int count) => throw new NotImplementedException();

	public virtual int GetCharCount(byte[] bytes) => throw new NotImplementedException();

	public abstract int GetCharCount(byte[] bytes, int index, int count);

	public virtual int GetCharCount(ReadOnlySpan<byte> bytes) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public virtual unsafe int GetChars(byte* bytes, int byteCount, char* chars, int charCount) => throw new NotImplementedException();

	public virtual char[] GetChars(byte[] bytes) => throw new NotImplementedException();

	public virtual char[] GetChars(byte[] bytes, int index, int count) => throw new NotImplementedException();

	public abstract int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex);

	public virtual int GetChars(ReadOnlySpan<byte> bytes, Span<char> chars) => throw new NotImplementedException();

	public virtual Decoder GetDecoder() => throw new NotImplementedException();

	public virtual Encoder GetEncoder() => throw new NotImplementedException();

	public static Encoding GetEncoding(int codepage) => throw new NotImplementedException();

	public static Encoding GetEncoding(int codepage, EncoderFallback encoderFallback, DecoderFallback decoderFallback) => throw new NotImplementedException();

	public static Encoding GetEncoding(string name) => throw new NotImplementedException();

	public static Encoding GetEncoding(string name, EncoderFallback encoderFallback, DecoderFallback decoderFallback) => throw new NotImplementedException();

	public static EncodingInfo[] GetEncodings() => throw new NotImplementedException();

	public override int GetHashCode() => throw new NotImplementedException();

	public abstract int GetMaxByteCount(int charCount);

	public abstract int GetMaxCharCount(int byteCount);

	public virtual byte[] GetPreamble() => throw new NotImplementedException();

	[CLSCompliant(false)]
	public unsafe string GetString(byte* bytes, int byteCount) => throw new NotImplementedException();

	public virtual string GetString(byte[] bytes) => GetString(bytes, 0, bytes.Length);

	public virtual string GetString(byte[] bytes, int index, int count) => throw new NotImplementedException();

	public string GetString(ReadOnlySpan<byte> bytes) => throw new NotImplementedException();

	public bool IsAlwaysNormalized() => throw new NotImplementedException();

	public virtual bool IsAlwaysNormalized(NormalizationForm form) => throw new NotImplementedException();

	public static void RegisterProvider(EncodingProvider provider) => throw new NotImplementedException();

	public virtual bool TryGetBytes(ReadOnlySpan<char> chars, Span<byte> bytes, out int bytesWritten) => throw new NotImplementedException();

	public virtual bool TryGetChars(ReadOnlySpan<byte> bytes, Span<char> chars, out int charsWritten) => throw new NotImplementedException();
}
