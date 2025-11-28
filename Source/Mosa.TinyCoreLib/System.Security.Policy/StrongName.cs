using System.Security.Permissions;

namespace System.Security.Policy;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public sealed class StrongName : EvidenceBase, IIdentityPermissionFactory
{
	public string Name
	{
		get
		{
			throw null;
		}
	}

	public StrongNamePublicKeyBlob PublicKey
	{
		get
		{
			throw null;
		}
	}

	public Version Version
	{
		get
		{
			throw null;
		}
	}

	public StrongName(StrongNamePublicKeyBlob blob, string name, Version version)
	{
	}

	public object Copy()
	{
		throw null;
	}

	public IPermission CreateIdentityPermission(Evidence evidence)
	{
		throw null;
	}

	public override bool Equals(object o)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
