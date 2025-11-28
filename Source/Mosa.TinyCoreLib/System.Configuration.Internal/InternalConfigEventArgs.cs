namespace System.Configuration.Internal;

public sealed class InternalConfigEventArgs : EventArgs
{
	public string ConfigPath
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public InternalConfigEventArgs(string configPath)
	{
	}
}
