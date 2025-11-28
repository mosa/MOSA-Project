namespace System.DirectoryServices.Protocols;

public class DirSyncResponseControl : DirectoryControl
{
	public byte[] Cookie
	{
		get
		{
			throw null;
		}
	}

	public bool MoreData
	{
		get
		{
			throw null;
		}
	}

	public int ResultSize
	{
		get
		{
			throw null;
		}
	}

	internal DirSyncResponseControl()
		: base(null, null, isCritical: false, serverSide: false)
	{
	}
}
