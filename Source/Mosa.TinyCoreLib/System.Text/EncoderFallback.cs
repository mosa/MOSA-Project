namespace System.Text;

public abstract class EncoderFallback
{
	public static EncoderFallback ExceptionFallback
	{
		get
		{
			throw null;
		}
	}

	public abstract int MaxCharCount { get; }

	public static EncoderFallback ReplacementFallback
	{
		get
		{
			throw null;
		}
	}

	public abstract EncoderFallbackBuffer CreateFallbackBuffer();
}
