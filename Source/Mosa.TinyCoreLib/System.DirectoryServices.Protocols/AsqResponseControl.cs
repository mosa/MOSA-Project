namespace System.DirectoryServices.Protocols;

public class AsqResponseControl : DirectoryControl
{
	public ResultCode Result
	{
		get
		{
			throw null;
		}
	}

	internal AsqResponseControl()
		: base(null, null, isCritical: false, serverSide: false)
	{
	}
}
