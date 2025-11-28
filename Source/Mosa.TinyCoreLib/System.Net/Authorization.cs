namespace System.Net;

public class Authorization
{
	public bool Complete
	{
		get
		{
			throw null;
		}
	}

	public string? ConnectionGroupId
	{
		get
		{
			throw null;
		}
	}

	public string? Message
	{
		get
		{
			throw null;
		}
	}

	public bool MutuallyAuthenticated
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string[]? ProtectionRealm
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Authorization(string? token)
	{
	}

	public Authorization(string? token, bool finished)
	{
	}

	public Authorization(string? token, bool finished, string? connectionGroupId)
	{
	}
}
