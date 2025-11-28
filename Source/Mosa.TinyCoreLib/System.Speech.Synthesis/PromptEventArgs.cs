using System.ComponentModel;

namespace System.Speech.Synthesis;

public abstract class PromptEventArgs : AsyncCompletedEventArgs
{
	public Prompt Prompt
	{
		get
		{
			throw null;
		}
	}

	internal PromptEventArgs()
		: base(null, cancelled: false, null)
	{
	}
}
