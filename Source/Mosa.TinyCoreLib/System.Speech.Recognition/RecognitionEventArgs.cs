namespace System.Speech.Recognition;

public abstract class RecognitionEventArgs : EventArgs
{
	public RecognitionResult Result
	{
		get
		{
			throw null;
		}
	}

	internal RecognitionEventArgs()
	{
	}
}
