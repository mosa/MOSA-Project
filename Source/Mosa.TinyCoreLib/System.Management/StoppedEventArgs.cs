namespace System.Management;

public class StoppedEventArgs : ManagementEventArgs
{
	public ManagementStatus Status
	{
		get
		{
			throw null;
		}
	}

	internal StoppedEventArgs()
	{
	}
}
