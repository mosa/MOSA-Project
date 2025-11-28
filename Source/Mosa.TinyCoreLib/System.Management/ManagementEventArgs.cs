namespace System.Management;

public abstract class ManagementEventArgs : EventArgs
{
	public object Context
	{
		get
		{
			throw null;
		}
	}

	internal ManagementEventArgs()
	{
	}
}
