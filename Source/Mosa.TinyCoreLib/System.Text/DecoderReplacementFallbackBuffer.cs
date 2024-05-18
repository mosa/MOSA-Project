namespace System.Text;

public sealed class DecoderReplacementFallbackBuffer : DecoderFallbackBuffer
{
	public override int Remaining
	{
		get
		{
			throw null;
		}
	}

	public DecoderReplacementFallbackBuffer(DecoderReplacementFallback fallback)
	{
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

	public override void Reset()
	{
	}
}
