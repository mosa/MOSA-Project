using System.ComponentModel;

namespace System.Speech.Recognition;

public class RecognizeCompletedEventArgs : AsyncCompletedEventArgs
{
	public TimeSpan AudioPosition
	{
		get
		{
			throw null;
		}
	}

	public bool BabbleTimeout
	{
		get
		{
			throw null;
		}
	}

	public bool InitialSilenceTimeout
	{
		get
		{
			throw null;
		}
	}

	public bool InputStreamEnded
	{
		get
		{
			throw null;
		}
	}

	public RecognitionResult Result
	{
		get
		{
			throw null;
		}
	}

	internal RecognizeCompletedEventArgs()
		: base(null, cancelled: false, null)
	{
	}
}
