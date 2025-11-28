namespace System.Management;

public class ManagementOperationObserver
{
	public event CompletedEventHandler Completed
	{
		add
		{
		}
		remove
		{
		}
	}

	public event ObjectPutEventHandler ObjectPut
	{
		add
		{
		}
		remove
		{
		}
	}

	public event ObjectReadyEventHandler ObjectReady
	{
		add
		{
		}
		remove
		{
		}
	}

	public event ProgressEventHandler Progress
	{
		add
		{
		}
		remove
		{
		}
	}

	public void Cancel()
	{
	}
}
