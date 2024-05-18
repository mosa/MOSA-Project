namespace System.Management;

public class CompletedEventArgs : ManagementEventArgs
{
	public ManagementStatus Status
	{
		get
		{
			throw null;
		}
	}

	public ManagementBaseObject StatusObject
	{
		get
		{
			throw null;
		}
	}

	internal CompletedEventArgs()
	{
	}
}
