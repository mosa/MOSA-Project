namespace System.Management;

public class EventWatcherOptions : ManagementOptions
{
	public int BlockSize
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public EventWatcherOptions()
	{
	}

	public EventWatcherOptions(ManagementNamedValueCollection context, TimeSpan timeout, int blockSize)
	{
	}

	public override object Clone()
	{
		throw null;
	}
}
