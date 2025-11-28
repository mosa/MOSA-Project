namespace System.DirectoryServices.Protocols;

public class PageResultResponseControl : DirectoryControl
{
	public byte[] Cookie
	{
		get
		{
			throw null;
		}
	}

	public int TotalCount
	{
		get
		{
			throw null;
		}
	}

	internal PageResultResponseControl()
		: base(null, null, isCritical: false, serverSide: false)
	{
	}
}
