namespace System.Security.Permissions;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
public sealed class StorePermissionAttribute : CodeAccessSecurityAttribute
{
	public bool AddToStore
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool CreateStore
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool DeleteStore
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool EnumerateCertificates
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool EnumerateStores
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public StorePermissionFlags Flags
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool OpenStore
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool RemoveFromStore
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public StorePermissionAttribute(SecurityAction action)
		: base((SecurityAction)0)
	{
	}

	public override IPermission CreatePermission()
	{
		throw null;
	}
}
