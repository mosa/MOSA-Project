namespace System.DirectoryServices.Protocols;

public abstract class DirectoryRequest : DirectoryOperation
{
	public DirectoryControlCollection Controls
	{
		get
		{
			throw null;
		}
	}

	public string RequestId
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	internal DirectoryRequest()
	{
	}
}
