namespace System.Text;

public sealed class EncoderExceptionFallbackBuffer : EncoderFallbackBuffer
{
	public override int Remaining
	{
		get
		{
			throw null;
		}
	}

	public override bool Fallback(char charUnknownHigh, char charUnknownLow, int index)
	{
		throw null;
	}

	public override bool Fallback(char charUnknown, int index)
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
