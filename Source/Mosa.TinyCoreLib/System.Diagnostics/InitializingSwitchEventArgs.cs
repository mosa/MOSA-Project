namespace System.Diagnostics;

public sealed class InitializingSwitchEventArgs : EventArgs
{
	public Switch Switch
	{
		get
		{
			throw null;
		}
	}

	public InitializingSwitchEventArgs(Switch @switch)
	{
	}
}
