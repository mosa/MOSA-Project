using System.Collections;
using System.Security.Policy;

namespace System.Security;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public static class SecurityManager
{
	[Obsolete]
	public static bool CheckExecutionRights
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[Obsolete]
	public static bool SecurityEnabled
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public static bool CurrentThreadRequiresSecurityContextCapture()
	{
		throw null;
	}

	public static PermissionSet GetStandardSandbox(Evidence evidence)
	{
		throw null;
	}

	public static void GetZoneAndOrigin(out ArrayList zone, out ArrayList origin)
	{
		throw null;
	}

	[Obsolete]
	public static bool IsGranted(IPermission perm)
	{
		throw null;
	}

	[Obsolete]
	public static PolicyLevel LoadPolicyLevelFromFile(string path, PolicyLevelType type)
	{
		throw null;
	}

	[Obsolete]
	public static PolicyLevel LoadPolicyLevelFromString(string str, PolicyLevelType type)
	{
		throw null;
	}

	[Obsolete]
	public static IEnumerator PolicyHierarchy()
	{
		throw null;
	}

	[Obsolete]
	public static PermissionSet ResolvePolicy(Evidence evidence)
	{
		throw null;
	}

	[Obsolete]
	public static PermissionSet ResolvePolicy(Evidence evidence, PermissionSet reqdPset, PermissionSet optPset, PermissionSet denyPset, out PermissionSet denied)
	{
		throw null;
	}

	[Obsolete]
	public static PermissionSet ResolvePolicy(Evidence[] evidences)
	{
		throw null;
	}

	[Obsolete]
	public static IEnumerator ResolvePolicyGroups(Evidence evidence)
	{
		throw null;
	}

	[Obsolete]
	public static PermissionSet ResolveSystemPolicy(Evidence evidence)
	{
		throw null;
	}

	[Obsolete]
	public static void SavePolicy()
	{
	}

	[Obsolete]
	public static void SavePolicyLevel(PolicyLevel level)
	{
	}
}
