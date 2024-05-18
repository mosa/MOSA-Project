using System.Diagnostics.CodeAnalysis;

namespace System.Reflection;

public static class AssemblyExtensions
{
	[RequiresUnreferencedCode("Types might be removed")]
	public static Type[] GetExportedTypes(this Assembly assembly)
	{
		throw null;
	}

	public static Module[] GetModules(this Assembly assembly)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Types might be removed")]
	public static Type[] GetTypes(this Assembly assembly)
	{
		throw null;
	}
}
