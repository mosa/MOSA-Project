namespace System.DirectoryServices.Protocols;

public class SortResponseControl : DirectoryControl
{
	public string AttributeName
	{
		get
		{
			throw null;
		}
	}

	public ResultCode Result
	{
		get
		{
			throw null;
		}
	}

	internal SortResponseControl()
		: base(null, null, isCritical: false, serverSide: false)
	{
	}
}
