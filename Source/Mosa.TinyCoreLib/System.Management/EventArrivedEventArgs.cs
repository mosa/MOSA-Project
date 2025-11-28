namespace System.Management;

public class EventArrivedEventArgs : ManagementEventArgs
{
	public ManagementBaseObject NewEvent
	{
		get
		{
			throw null;
		}
	}

	internal EventArrivedEventArgs()
	{
	}
}
