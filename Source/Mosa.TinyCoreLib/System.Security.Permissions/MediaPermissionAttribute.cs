namespace System.Security.Permissions;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
public sealed class MediaPermissionAttribute : CodeAccessSecurityAttribute
{
	public MediaPermissionAudio Audio
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public MediaPermissionImage Image
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public MediaPermissionVideo Video
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public MediaPermissionAttribute(SecurityAction action)
		: base((SecurityAction)0)
	{
	}

	public override IPermission CreatePermission()
	{
		throw null;
	}
}
