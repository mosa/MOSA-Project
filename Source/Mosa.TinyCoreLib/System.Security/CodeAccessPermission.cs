namespace System.Security;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public abstract class CodeAccessPermission : IPermission, ISecurityEncodable, IStackWalk
{
	public void Assert()
	{
	}

	public abstract IPermission Copy();

	public void Demand()
	{
	}

	[Obsolete]
	public void Deny()
	{
	}

	public override bool Equals(object obj)
	{
		throw null;
	}

	public abstract void FromXml(SecurityElement elem);

	public override int GetHashCode()
	{
		throw null;
	}

	public abstract IPermission Intersect(IPermission target);

	public abstract bool IsSubsetOf(IPermission target);

	public void PermitOnly()
	{
	}

	public static void RevertAll()
	{
	}

	public static void RevertAssert()
	{
	}

	[Obsolete]
	public static void RevertDeny()
	{
	}

	public static void RevertPermitOnly()
	{
	}

	public override string ToString()
	{
		throw null;
	}

	public abstract SecurityElement ToXml();

	public virtual IPermission Union(IPermission other)
	{
		throw null;
	}
}
