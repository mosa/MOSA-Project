using System.Security.Permissions;

namespace System.Security;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public sealed class NamedPermissionSet : PermissionSet
{
	public string Description
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

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

	public NamedPermissionSet(NamedPermissionSet permSet)
		: base(PermissionState.None)
	{
	}

	public NamedPermissionSet(string name)
		: base(PermissionState.None)
	{
	}

	public NamedPermissionSet(string name, PermissionState state)
		: base(PermissionState.None)
	{
	}

	public NamedPermissionSet(string name, PermissionSet permSet)
		: base(PermissionState.None)
	{
	}

	public override PermissionSet Copy()
	{
		throw null;
	}

	public NamedPermissionSet Copy(string name)
	{
		throw null;
	}

	public override bool Equals(object o)
	{
		throw null;
	}

	public override void FromXml(SecurityElement et)
	{
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public override SecurityElement ToXml()
	{
		throw null;
	}
}
