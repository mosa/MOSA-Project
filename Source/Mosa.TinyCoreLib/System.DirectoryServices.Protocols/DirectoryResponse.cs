namespace System.DirectoryServices.Protocols;

public abstract class DirectoryResponse : DirectoryOperation
{
	public virtual DirectoryControl[] Controls
	{
		get
		{
			throw null;
		}
	}

	public virtual string ErrorMessage
	{
		get
		{
			throw null;
		}
	}

	public virtual string MatchedDN
	{
		get
		{
			throw null;
		}
	}

	public virtual Uri[] Referral
	{
		get
		{
			throw null;
		}
	}

	public string RequestId
	{
		get
		{
			throw null;
		}
	}

	public virtual ResultCode ResultCode
	{
		get
		{
			throw null;
		}
	}

	internal DirectoryResponse()
	{
	}
}
