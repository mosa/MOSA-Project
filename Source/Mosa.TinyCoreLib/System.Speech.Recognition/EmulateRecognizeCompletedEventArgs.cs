using System.ComponentModel;

namespace System.Speech.Recognition;

public class EmulateRecognizeCompletedEventArgs : AsyncCompletedEventArgs
{
	public RecognitionResult Result
	{
		get
		{
			throw null;
		}
	}

	internal EmulateRecognizeCompletedEventArgs()
		: base(null, cancelled: false, null)
	{
	}
}
