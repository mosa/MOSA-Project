namespace System.Reflection;

public abstract class MetadataAssemblyResolver
{
	public abstract Assembly? Resolve(MetadataLoadContext context, AssemblyName assemblyName);
}
