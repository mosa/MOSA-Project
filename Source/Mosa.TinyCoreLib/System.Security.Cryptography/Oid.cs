namespace System.Security.Cryptography;

public sealed class Oid
{
	public string? FriendlyName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? Value
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Oid()
	{
	}

	public Oid(Oid oid)
	{
	}

	public Oid(string oid)
	{
	}

	public Oid(string? value, string? friendlyName)
	{
	}

	public static Oid FromFriendlyName(string friendlyName, OidGroup group)
	{
		throw null;
	}

	public static Oid FromOidValue(string oidValue, OidGroup group)
	{
		throw null;
	}
}
