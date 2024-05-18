namespace System.Security.Permissions;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
public sealed class ReflectionPermissionAttribute : CodeAccessSecurityAttribute
{
	public ReflectionPermissionFlag Flags
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool MemberAccess
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[Obsolete("ReflectionPermissionAttribute.ReflectionEmit has been deprecated and is not supported.")]
	public bool ReflectionEmit
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool RestrictedMemberAccess
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[Obsolete("ReflectionPermissionAttribute.TypeInformation has been deprecated and is not supported.")]
	public bool TypeInformation
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ReflectionPermissionAttribute(SecurityAction action)
		: base((SecurityAction)0)
	{
	}

	public override IPermission CreatePermission()
	{
		throw null;
	}
}
