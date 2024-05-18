namespace System.Security.Permissions;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public sealed class PrincipalPermission : IPermission, ISecurityEncodable, IUnrestrictedPermission
{
	public PrincipalPermission(PermissionState state)
	{
	}

	public PrincipalPermission(string name, string role)
	{
	}

	public PrincipalPermission(string name, string role, bool isAuthenticated)
	{
	}

	public IPermission Copy()
	{
		throw null;
	}

	public void Demand()
	{
	}

	public override bool Equals(object obj)
	{
		throw null;
	}

	public void FromXml(SecurityElement elem)
	{
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public IPermission Intersect(IPermission target)
	{
		throw null;
	}

	public bool IsSubsetOf(IPermission target)
	{
		throw null;
	}

	public bool IsUnrestricted()
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

	public IPermission Union(IPermission other)
	{
		throw null;
	}
}
