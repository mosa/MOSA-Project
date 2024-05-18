namespace System.DirectoryServices.Protocols;

public class SearchResponse : DirectoryResponse
{
	public override DirectoryControl[] Controls
	{
		get
		{
			throw null;
		}
	}

	public SearchResultEntryCollection Entries
	{
		get
		{
			throw null;
		}
	}

	public override string ErrorMessage
	{
		get
		{
			throw null;
		}
	}

	public override string MatchedDN
	{
		get
		{
			throw null;
		}
	}

	public SearchResultReferenceCollection References
	{
		get
		{
			throw null;
		}
	}

	public override Uri[] Referral
	{
		get
		{
			throw null;
		}
	}

	public override ResultCode ResultCode
	{
		get
		{
			throw null;
		}
	}

	internal SearchResponse()
	{
	}
}
