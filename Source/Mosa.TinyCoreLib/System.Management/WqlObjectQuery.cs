namespace System.Management;

public class WqlObjectQuery : ObjectQuery
{
	public override string QueryLanguage
	{
		get
		{
			throw null;
		}
	}

	public WqlObjectQuery()
	{
	}

	public WqlObjectQuery(string query)
	{
	}

	public override object Clone()
	{
		throw null;
	}
}
