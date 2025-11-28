namespace System.Text;

public abstract class DecoderFallback
{
	public static DecoderFallback ExceptionFallback
	{
		get
		{
			throw null;
		}
	}

	public abstract int MaxCharCount { get; }

	public static DecoderFallback ReplacementFallback
	{
		get
		{
			throw null;
		}
	}

	public abstract DecoderFallbackBuffer CreateFallbackBuffer();
}
