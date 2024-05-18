namespace System.Speech.Synthesis;

public class SpeakProgressEventArgs : PromptEventArgs
{
	public TimeSpan AudioPosition
	{
		get
		{
			throw null;
		}
	}

	public int CharacterCount
	{
		get
		{
			throw null;
		}
	}

	public int CharacterPosition
	{
		get
		{
			throw null;
		}
	}

	public string Text
	{
		get
		{
			throw null;
		}
	}

	internal SpeakProgressEventArgs()
	{
	}
}
