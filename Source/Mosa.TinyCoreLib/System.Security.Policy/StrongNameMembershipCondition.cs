using System.Security.Permissions;

namespace System.Security.Policy;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public sealed class StrongNameMembershipCondition : ISecurityEncodable, ISecurityPolicyEncodable, IMembershipCondition
{
	public string Name
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public StrongNamePublicKeyBlob PublicKey
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Version Version
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public StrongNameMembershipCondition(StrongNamePublicKeyBlob blob, string name, Version version)
	{
	}

	public bool Check(Evidence evidence)
	{
		throw null;
	}

	public IMembershipCondition Copy()
	{
		throw null;
	}

	public override bool Equals(object o)
	{
		throw null;
	}

	public void FromXml(SecurityElement e)
	{
	}

	public void FromXml(SecurityElement e, PolicyLevel level)
	{
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}

	public SecurityElement ToXml()
	{
		throw null;
	}

	public SecurityElement ToXml(PolicyLevel level)
	{
		throw null;
	}
}
