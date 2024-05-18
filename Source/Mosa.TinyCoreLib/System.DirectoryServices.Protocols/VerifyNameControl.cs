namespace System.DirectoryServices.Protocols;

public class VerifyNameControl : DirectoryControl
{
	public int Flag
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string ServerName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public VerifyNameControl()
		: base(null, null, isCritical: false, serverSide: false)
	{
	}

	public VerifyNameControl(string serverName)
		: base(null, null, isCritical: false, serverSide: false)
	{
	}

	public VerifyNameControl(string serverName, int flag)
		: base(null, null, isCritical: false, serverSide: false)
	{
	}

	public override byte[] GetValue()
	{
		throw null;
	}
}
