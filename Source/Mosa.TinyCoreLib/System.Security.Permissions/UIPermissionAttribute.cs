namespace System.Security.Permissions;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
public sealed class UIPermissionAttribute : CodeAccessSecurityAttribute
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

	public UIPermissionAttribute(SecurityAction action)
		: base((SecurityAction)0)
	{
	}

	public override IPermission CreatePermission()
	{
		throw null;
	}
}
