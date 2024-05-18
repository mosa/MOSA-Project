using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;

namespace System.Runtime.Loader;

public class AssemblyLoadContext
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public struct ContextualReflectionScope : IDisposable
	{
		private object _dummy;

		private int _dummyPrimitive;

		public void Dispose()
		{
		}
	}

	public static IEnumerable<AssemblyLoadContext> All
	{
		get
		{
			throw null;
		}
	}

	public IEnumerable<Assembly> Assemblies
	{
		get
		{
			throw null;
		}
	}

	public static AssemblyLoadContext? CurrentContextualReflectionContext
	{
		get
		{
			throw null;
		}
	}

	public static AssemblyLoadContext Default
	{
		get
		{
			throw null;
		}
	}

	public bool IsCollectible
	{
		get
		{
			throw null;
		}
	}

	public string? Name
	{
		get
		{
			throw null;
		}
	}

	public event Func<AssemblyLoadContext, AssemblyName, Assembly?>? Resolving
	{
		add
		{
		}
		remove
		{
		}
	}

	public event Func<Assembly, string, IntPtr>? ResolvingUnmanagedDll
	{
		add
		{
		}
		remove
		{
		}
	}

	public event Action<AssemblyLoadContext>? Unloading
	{
		add
		{
		}
		remove
		{
		}
	}

	protected AssemblyLoadContext()
	{
	}

	protected AssemblyLoadContext(bool isCollectible)
	{
	}

	public AssemblyLoadContext(string? name, bool isCollectible = false)
	{
	}

	public ContextualReflectionScope EnterContextualReflection()
	{
		throw null;
	}

	public static ContextualReflectionScope EnterContextualReflection(Assembly? activating)
	{
		throw null;
	}

	~AssemblyLoadContext()
	{
	}

	public static AssemblyName GetAssemblyName(string assemblyPath)
	{
		throw null;
	}

	public static AssemblyLoadContext? GetLoadContext(Assembly assembly)
	{
		throw null;
	}

	protected virtual Assembly? Load(AssemblyName assemblyName)
	{
		throw null;
	}

	public Assembly LoadFromAssemblyName(AssemblyName assemblyName)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Types and members the loaded assembly depends on might be removed")]
	public Assembly LoadFromAssemblyPath(string assemblyPath)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Types and members the loaded assembly depends on might be removed")]
	public Assembly LoadFromNativeImagePath(string nativeImagePath, string? assemblyPath)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Types and members the loaded assembly depends on might be removed")]
	public Assembly LoadFromStream(Stream assembly)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Types and members the loaded assembly depends on might be removed")]
	public Assembly LoadFromStream(Stream assembly, Stream? assemblySymbols)
	{
		throw null;
	}

	protected virtual IntPtr LoadUnmanagedDll(string unmanagedDllName)
	{
		throw null;
	}

	protected IntPtr LoadUnmanagedDllFromPath(string unmanagedDllPath)
	{
		throw null;
	}

	public void SetProfileOptimizationRoot(string directoryPath)
	{
	}

	public void StartProfileOptimization(string? profile)
	{
	}

	public override string ToString()
	{
		throw null;
	}

	public void Unload()
	{
	}
}
