using System.Collections.Generic;

namespace System.Reflection;

public class PathAssemblyResolver : MetadataAssemblyResolver
{
	public PathAssemblyResolver(IEnumerable<string> assemblyPaths)
	{
	}

	public override Assembly? Resolve(MetadataLoadContext context, AssemblyName assemblyName)
	{
		throw null;
	}
}
