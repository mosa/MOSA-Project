namespace System.Security.Policy;

public sealed class PolicyStatement : ISecurityEncodable, ISecurityPolicyEncodable
{
	public PolicyStatementAttribute Attributes
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string AttributeString
	{
		get
		{
			throw null;
		}
	}

	[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public PermissionSet PermissionSet
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public PolicyStatement(PermissionSet permSet)
	{
	}

	[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public PolicyStatement(PermissionSet permSet, PolicyStatementAttribute attributes)
	{
	}

	public PolicyStatement Copy()
	{
		throw null;
	}

	public override bool Equals(object o)
	{
		throw null;
	}

	public void FromXml(SecurityElement et)
	{
	}

	public void FromXml(SecurityElement et, PolicyLevel level)
	{
	}

	public override int GetHashCode()
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
