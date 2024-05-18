namespace System.Security.Cryptography;

public sealed class CngUIPolicy
{
	public string? CreationTitle
	{
		get
		{
			throw null;
		}
	}

	public string? Description
	{
		get
		{
			throw null;
		}
	}

	public string? FriendlyName
	{
		get
		{
			throw null;
		}
	}

	public CngUIProtectionLevels ProtectionLevel
	{
		get
		{
			throw null;
		}
	}

	public string? UseContext
	{
		get
		{
			throw null;
		}
	}

	public CngUIPolicy(CngUIProtectionLevels protectionLevel)
	{
	}

	public CngUIPolicy(CngUIProtectionLevels protectionLevel, string? friendlyName)
	{
	}

	public CngUIPolicy(CngUIProtectionLevels protectionLevel, string? friendlyName, string? description)
	{
	}

	public CngUIPolicy(CngUIProtectionLevels protectionLevel, string? friendlyName, string? description, string? useContext)
	{
	}

	public CngUIPolicy(CngUIProtectionLevels protectionLevel, string? friendlyName, string? description, string? useContext, string? creationTitle)
	{
	}
}
