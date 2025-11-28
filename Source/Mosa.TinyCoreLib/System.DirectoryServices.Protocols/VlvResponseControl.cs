namespace System.DirectoryServices.Protocols;

public class VlvResponseControl : DirectoryControl
{
	public int ContentCount
	{
		get
		{
			throw null;
		}
	}

	public byte[] ContextId
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

	public int TargetPosition
	{
		get
		{
			throw null;
		}
	}

	internal VlvResponseControl()
		: base(null, null, isCritical: false, serverSide: false)
	{
	}
}
