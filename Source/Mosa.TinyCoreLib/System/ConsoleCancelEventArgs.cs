namespace System;

public sealed class ConsoleCancelEventArgs : EventArgs
{
	public bool Cancel
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ConsoleSpecialKey SpecialKey
	{
		get
		{
			throw null;
		}
	}

	internal ConsoleCancelEventArgs()
	{
	}
}
