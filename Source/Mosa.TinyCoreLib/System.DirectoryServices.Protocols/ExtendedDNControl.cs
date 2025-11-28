namespace System.DirectoryServices.Protocols;

public class ExtendedDNControl : DirectoryControl
{
	public ExtendedDNFlag Flag
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ExtendedDNControl()
		: base(null, null, isCritical: false, serverSide: false)
	{
	}

	public ExtendedDNControl(ExtendedDNFlag flag)
		: base(null, null, isCritical: false, serverSide: false)
	{
	}

	public override byte[] GetValue()
	{
		throw null;
	}
}
