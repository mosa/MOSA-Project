using System.Collections.Generic;
using System.IO;

namespace System.Reflection;

public sealed class MetadataLoadContext : IDisposable
{
	public Assembly? CoreAssembly
	{
		get
		{
			throw null;
		}
	}

	public MetadataLoadContext(MetadataAssemblyResolver resolver, string? coreAssemblyName = null)
	{
	}

	public void Dispose()
	{
	}

	public IEnumerable<Assembly> GetAssemblies()
	{
		throw null;
	}

	public Assembly LoadFromAssemblyName(AssemblyName assemblyName)
	{
		throw null;
	}

	public Assembly LoadFromAssemblyName(string assemblyName)
	{
		throw null;
	}

	public Assembly LoadFromAssemblyPath(string assemblyPath)
	{
		throw null;
	}

	public Assembly LoadFromByteArray(byte[] assembly)
	{
		throw null;
	}

	public Assembly LoadFromStream(Stream assembly)
	{
		throw null;
	}
}
