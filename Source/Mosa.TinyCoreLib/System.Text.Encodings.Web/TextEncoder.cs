using System.Buffers;
using System.ComponentModel;
using System.IO;

namespace System.Text.Encodings.Web;

public abstract class TextEncoder
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public abstract int MaxOutputCharactersPerInputCharacter { get; }

	public virtual void Encode(TextWriter output, char[] value, int startIndex, int characterCount)
	{
	}

	public void Encode(TextWriter output, string value)
	{
	}

	public virtual void Encode(TextWriter output, string value, int startIndex, int characterCount)
	{
	}

	public virtual OperationStatus Encode(ReadOnlySpan<char> source, Span<char> destination, out int charsConsumed, out int charsWritten, bool isFinalBlock = true)
	{
		throw null;
	}

	public virtual string Encode(string value)
	{
		throw null;
	}

	public virtual OperationStatus EncodeUtf8(ReadOnlySpan<byte> utf8Source, Span<byte> utf8Destination, out int bytesConsumed, out int bytesWritten, bool isFinalBlock = true)
	{
		throw null;
	}

	[CLSCompliant(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public unsafe abstract int FindFirstCharacterToEncode(char* text, int textLength);

	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual int FindFirstCharacterToEncodeUtf8(ReadOnlySpan<byte> utf8Text)
	{
		throw null;
	}

	[CLSCompliant(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public unsafe abstract bool TryEncodeUnicodeScalar(int unicodeScalar, char* buffer, int bufferLength, out int numberOfCharactersWritten);

	[EditorBrowsable(EditorBrowsableState.Never)]
	public abstract bool WillEncode(int unicodeScalar);
}
