namespace System.DirectoryServices.Protocols;

public class SearchOptionsControl : DirectoryControl
{
	public SearchOption SearchOption
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SearchOptionsControl()
		: base(null, null, isCritical: false, serverSide: false)
	{
	}

	public SearchOptionsControl(SearchOption flags)
		: base(null, null, isCritical: false, serverSide: false)
	{
	}

	public override byte[] GetValue()
	{
		throw null;
	}
}
