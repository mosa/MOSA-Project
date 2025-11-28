namespace System.Security.Permissions;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public sealed class UIPermission : CodeAccessPermission, IUnrestrictedPermission
{
	public UIPermissionClipboard Clipboard
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public UIPermissionWindow Window
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public UIPermission(PermissionState state)
	{
	}

	public UIPermission(UIPermissionClipboard clipboardFlag)
	{
	}

	public UIPermission(UIPermissionWindow windowFlag)
	{
	}

	public UIPermission(UIPermissionWindow windowFlag, UIPermissionClipboard clipboardFlag)
	{
	}

	public override IPermission Copy()
	{
		throw null;
	}

	public override void FromXml(SecurityElement esd)
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

	public bool IsUnrestricted()
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
