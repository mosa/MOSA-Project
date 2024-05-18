namespace System.Management;

public class ObjectReadyEventArgs : ManagementEventArgs
{
	public ManagementBaseObject NewObject
	{
		get
		{
			throw null;
		}
	}

	internal ObjectReadyEventArgs()
	{
	}
}
