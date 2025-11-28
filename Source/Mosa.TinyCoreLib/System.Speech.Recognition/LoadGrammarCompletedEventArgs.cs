using System.ComponentModel;

namespace System.Speech.Recognition;

public class LoadGrammarCompletedEventArgs : AsyncCompletedEventArgs
{
	public Grammar Grammar
	{
		get
		{
			throw null;
		}
	}

	internal LoadGrammarCompletedEventArgs()
		: base(null, cancelled: false, null)
	{
	}
}
