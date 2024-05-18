namespace System.Text;

public abstract class DecoderFallbackBuffer
{
	public abstract int Remaining { get; }

	public abstract bool Fallback(byte[] bytesUnknown, int index);

	public abstract char GetNextChar();

	public abstract bool MovePrevious();

	public virtual void Reset()
	{
	}
}
