namespace System.Security.Permissions;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
public sealed class RegistryPermissionAttribute : CodeAccessSecurityAttribute
{
	[Obsolete("RegistryPermissionAttribute.Add has been deprecated. Use ViewAndModify instead.")]
	public string All
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string ChangeAccessControl
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Create
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Read
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string ViewAccessControl
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string ViewAndModify
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Write
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public RegistryPermissionAttribute(SecurityAction action)
		: base((SecurityAction)0)
	{
	}

	public override IPermission CreatePermission()
	{
		throw null;
	}
}
