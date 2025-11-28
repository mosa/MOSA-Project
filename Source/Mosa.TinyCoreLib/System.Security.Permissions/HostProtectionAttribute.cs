namespace System.Security.Permissions;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Delegate, AllowMultiple = true, Inherited = false)]
public sealed class HostProtectionAttribute : CodeAccessSecurityAttribute
{
	public bool ExternalProcessMgmt
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool ExternalThreading
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool MayLeakOnAbort
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public HostProtectionResource Resources
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool SecurityInfrastructure
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool SelfAffectingProcessMgmt
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool SelfAffectingThreading
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool SharedState
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool Synchronization
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool UI
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public HostProtectionAttribute()
		: base((SecurityAction)0)
	{
	}

	public HostProtectionAttribute(SecurityAction action)
		: base((SecurityAction)0)
	{
	}

	public override IPermission CreatePermission()
	{
		throw null;
	}
}
