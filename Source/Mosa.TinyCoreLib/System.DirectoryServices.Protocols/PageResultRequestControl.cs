namespace System.DirectoryServices.Protocols;

public class PageResultRequestControl : DirectoryControl
{
	public byte[] Cookie
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int PageSize
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public PageResultRequestControl()
		: base(null, null, isCritical: false, serverSide: false)
	{
	}

	public PageResultRequestControl(byte[] cookie)
		: base(null, null, isCritical: false, serverSide: false)
	{
	}

	public PageResultRequestControl(int pageSize)
		: base(null, null, isCritical: false, serverSide: false)
	{
	}

	public override byte[] GetValue()
	{
		throw null;
	}
}
