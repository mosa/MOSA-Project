namespace System.DirectoryServices.AccountManagement;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class DirectoryRdnPrefixAttribute : Attribute
{
	public ContextType? Context
	{
		get
		{
			throw null;
		}
	}

	public string RdnPrefix
	{
		get
		{
			throw null;
		}
	}

	public DirectoryRdnPrefixAttribute(string rdnPrefix)
	{
	}
}
