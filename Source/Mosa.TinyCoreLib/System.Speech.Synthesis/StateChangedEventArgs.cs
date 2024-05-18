namespace System.Speech.Synthesis;

public class StateChangedEventArgs : EventArgs
{
	public SynthesizerState PreviousState
	{
		get
		{
			throw null;
		}
	}

	public SynthesizerState State
	{
		get
		{
			throw null;
		}
	}

	internal StateChangedEventArgs()
	{
	}
}
