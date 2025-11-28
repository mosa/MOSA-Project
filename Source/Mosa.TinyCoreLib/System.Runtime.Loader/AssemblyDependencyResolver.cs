using System.Reflection;
using System.Runtime.Versioning;

namespace System.Runtime.Loader;

[UnsupportedOSPlatform("android")]
[UnsupportedOSPlatform("browser")]
[UnsupportedOSPlatform("ios")]
[UnsupportedOSPlatform("tvos")]
public sealed class AssemblyDependencyResolver
{
	public AssemblyDependencyResolver(string componentAssemblyPath)
	{
	}

	public string? ResolveAssemblyToPath(AssemblyName assemblyName)
	{
		throw null;
	}

	public string? ResolveUnmanagedDllToPath(string unmanagedDllName)
	{
		throw null;
	}
}
