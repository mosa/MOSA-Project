namespace System.DirectoryServices.Protocols;

public class AsqRequestControl : DirectoryControl
{
	public string AttributeName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public AsqRequestControl()
		: base(null, null, isCritical: false, serverSide: false)
	{
	}

	public AsqRequestControl(string attributeName)
		: base(null, null, isCritical: false, serverSide: false)
	{
	}

	public override byte[] GetValue()
	{
		throw null;
	}
}
