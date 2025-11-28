namespace System.DirectoryServices.Protocols;

public class VlvRequestControl : DirectoryControl
{
	public int AfterCount
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int BeforeCount
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public byte[] ContextId
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int EstimateCount
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int Offset
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public byte[] Target
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public VlvRequestControl()
		: base(null, null, isCritical: false, serverSide: false)
	{
	}

	public VlvRequestControl(int beforeCount, int afterCount, byte[] target)
		: base(null, null, isCritical: false, serverSide: false)
	{
	}

	public VlvRequestControl(int beforeCount, int afterCount, int offset)
		: base(null, null, isCritical: false, serverSide: false)
	{
	}

	public VlvRequestControl(int beforeCount, int afterCount, string target)
		: base(null, null, isCritical: false, serverSide: false)
	{
	}

	public override byte[] GetValue()
	{
		throw null;
	}
}
