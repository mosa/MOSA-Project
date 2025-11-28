namespace System.Text;

public sealed class DecoderExceptionFallbackBuffer : DecoderFallbackBuffer
{
	public override int Remaining
	{
		get
		{
			throw null;
		}
	}

	public override bool Fallback(byte[] bytesUnknown, int index)
	{
		throw null;
	}

	public override char GetNextChar()
	{
		throw null;
	}

	public override bool MovePrevious()
	{
		throw null;
	}
}
