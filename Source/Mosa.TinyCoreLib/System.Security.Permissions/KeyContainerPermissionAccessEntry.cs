using System.Security.Cryptography;

namespace System.Security.Permissions;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public sealed class KeyContainerPermissionAccessEntry
{
	public KeyContainerPermissionFlags Flags
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string KeyContainerName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int KeySpec
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string KeyStore
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string ProviderName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int ProviderType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public KeyContainerPermissionAccessEntry(CspParameters parameters, KeyContainerPermissionFlags flags)
	{
	}

	public KeyContainerPermissionAccessEntry(string keyContainerName, KeyContainerPermissionFlags flags)
	{
	}

	public KeyContainerPermissionAccessEntry(string keyStore, string providerName, int providerType, string keyContainerName, int keySpec, KeyContainerPermissionFlags flags)
	{
	}

	public override bool Equals(object o)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}
}
