namespace System.DirectoryServices.Protocols;

public class SortRequestControl : DirectoryControl
{
	public SortKey[] SortKeys
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SortRequestControl(params SortKey[] sortKeys)
		: base(null, null, isCritical: false, serverSide: false)
	{
	}

	public SortRequestControl(string attributeName, bool reverseOrder)
		: base(null, null, isCritical: false, serverSide: false)
	{
	}

	public SortRequestControl(string attributeName, string matchingRule, bool reverseOrder)
		: base(null, null, isCritical: false, serverSide: false)
	{
	}

	public override byte[] GetValue()
	{
		throw null;
	}
}
