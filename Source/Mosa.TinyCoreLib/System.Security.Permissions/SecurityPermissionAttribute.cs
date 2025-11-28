namespace System.Security.Permissions;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
public sealed class SecurityPermissionAttribute : CodeAccessSecurityAttribute
{
	public bool Assertion
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool BindingRedirects
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool ControlAppDomain
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool ControlDomainPolicy
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool ControlEvidence
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool ControlPolicy
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool ControlPrincipal
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool ControlThread
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool Execution
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SecurityPermissionFlag Flags
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool Infrastructure
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool RemotingConfiguration
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool SerializationFormatter
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool SkipVerification
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool UnmanagedCode
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SecurityPermissionAttribute(SecurityAction action)
		: base((SecurityAction)0)
	{
	}

	public override IPermission? CreatePermission()
	{
		throw null;
	}
}
