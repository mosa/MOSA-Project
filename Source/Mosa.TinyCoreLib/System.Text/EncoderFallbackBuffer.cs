namespace System.Text;

public abstract class EncoderFallbackBuffer
{
	public abstract int Remaining { get; }

	public abstract bool Fallback(char charUnknownHigh, char charUnknownLow, int index);

	public abstract bool Fallback(char charUnknown, int index);

	public abstract char GetNextChar();

	public abstract bool MovePrevious();

	public virtual void Reset()
	{
	}
}
