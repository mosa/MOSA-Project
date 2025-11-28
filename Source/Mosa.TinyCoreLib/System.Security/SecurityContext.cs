using System.Threading;

namespace System.Security;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public sealed class SecurityContext : IDisposable
{
	internal SecurityContext()
	{
	}

	public static SecurityContext Capture()
	{
		throw null;
	}

	public SecurityContext CreateCopy()
	{
		throw null;
	}

	public void Dispose()
	{
	}

	public static bool IsFlowSuppressed()
	{
		throw null;
	}

	public static bool IsWindowsIdentityFlowSuppressed()
	{
		throw null;
	}

	public static void RestoreFlow()
	{
	}

	public static void Run(SecurityContext securityContext, ContextCallback callback, object state)
	{
	}

	public static AsyncFlowControl SuppressFlow()
	{
		throw null;
	}

	public static AsyncFlowControl SuppressFlowWindowsIdentity()
	{
		throw null;
	}
}
