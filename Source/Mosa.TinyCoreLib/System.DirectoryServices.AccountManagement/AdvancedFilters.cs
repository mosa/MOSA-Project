namespace System.DirectoryServices.AccountManagement;

public class AdvancedFilters
{
	protected internal AdvancedFilters(Principal p)
	{
	}

	public void AccountExpirationDate(DateTime expirationTime, MatchType match)
	{
	}

	public void AccountLockoutTime(DateTime lockoutTime, MatchType match)
	{
	}

	protected void AdvancedFilterSet(string attribute, object value, Type objectType, MatchType mt)
	{
	}

	public void BadLogonCount(int badLogonCount, MatchType match)
	{
	}

	public void LastBadPasswordAttempt(DateTime lastAttempt, MatchType match)
	{
	}

	public void LastLogonTime(DateTime logonTime, MatchType match)
	{
	}

	public void LastPasswordSetTime(DateTime passwordSetTime, MatchType match)
	{
	}
}
