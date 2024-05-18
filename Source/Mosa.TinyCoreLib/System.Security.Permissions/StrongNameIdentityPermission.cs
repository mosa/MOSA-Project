namespace System.Security.Permissions;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public sealed class StrongNameIdentityPermission : CodeAccessPermission
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

	public StrongNameIdentityPermission(PermissionState state)
	{
	}

	public StrongNameIdentityPermission(StrongNamePublicKeyBlob blob, string name, Version version)
	{
	}

	public override IPermission Copy()
	{
		throw null;
	}

	public override void FromXml(SecurityElement e)
	{
	}

	public override IPermission Intersect(IPermission target)
	{
		throw null;
	}

	public override bool IsSubsetOf(IPermission target)
	{
		throw null;
	}

	public override SecurityElement ToXml()
	{
		throw null;
	}

	public override IPermission Union(IPermission target)
	{
		throw null;
	}
}
