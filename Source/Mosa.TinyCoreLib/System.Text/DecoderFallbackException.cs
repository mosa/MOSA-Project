namespace System.Text;

public sealed class DecoderFallbackException : ArgumentException
{
	public byte[]? BytesUnknown
	{
		get
		{
			throw null;
		}
	}

	public int Index
	{
		get
		{
			throw null;
		}
	}

	public DecoderFallbackException()
	{
	}

	public DecoderFallbackException(string? message)
	{
	}

	public DecoderFallbackException(string? message, byte[]? bytesUnknown, int index)
	{
	}

	public DecoderFallbackException(string? message, Exception? innerException)
	{
	}
}
