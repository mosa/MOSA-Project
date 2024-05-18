namespace System.Speech.Recognition;

public class AudioSignalProblemOccurredEventArgs : EventArgs
{
	public int AudioLevel
	{
		get
		{
			throw null;
		}
	}

	public TimeSpan AudioPosition
	{
		get
		{
			throw null;
		}
	}

	public AudioSignalProblem AudioSignalProblem
	{
		get
		{
			throw null;
		}
	}

	public TimeSpan RecognizerAudioPosition
	{
		get
		{
			throw null;
		}
	}

	internal AudioSignalProblemOccurredEventArgs()
	{
	}
}
