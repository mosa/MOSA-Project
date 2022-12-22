using dnlib.DotNet;
using Mosa.Compiler.MosaTypeSystem.CLR.Metadata;

namespace Mosa.Compiler.MosaTypeSystem.CLR;

public class ClrModuleLoader : IModuleLoader
{
	internal AssemblyResolver Resolver { get; }

	internal List<ModuleDefMD> Modules { get; }

	private readonly List<string> seenModules = new();

	public ClrModuleLoader()
	{
		Modules = new List<ModuleDefMD>();
		Resolver = new AssemblyResolver(null) { UseGAC = false };
		var typeResolver = new Resolver(Resolver);
		Resolver.DefaultModuleContext = new ModuleContext(Resolver, typeResolver);
		Resolver.EnableTypeDefCache = true;
	}

	public void LoadModuleFromFile(string file)
	{
		var module = ModuleDefMD.Load(file, Resolver.DefaultModuleContext);
		module.EnableTypeDefFindCache = true;

		LoadDependencies(module);
	}

	/// <summary>
	/// Appends the given path to the assembly search path.
	/// </summary>
	/// <param name="path">The path to append to the assembly search path.</param>
	public void AddSearchPath(string path)
	{
		if (!Resolver.PostSearchPaths.Contains(path))
		{
			Resolver.PostSearchPaths.Add(path);
		}
	}

	public IMetadata CreateMetadata()
	{
		return new ClrMetadata(this);
	}

	public void Dispose()
	{
		foreach (var module in Modules)
		{
			module.Dispose();
		}

		Modules.Clear();
	}

	private void LoadDependencies(ModuleDefMD module)
	{
		if (seenModules.Contains(module.Location))
			return;

		seenModules.Add(module.Location);
		Modules.Add(module);
		Resolver.AddToCache(module);

		foreach (var assemblyRef in module.GetAssemblyRefs())
		{
			// There are cases, where the Resolver will not be able to resolve the assemblies
			// automatically, even if they are in the same directory.
			// (maybe this has to do with linux / specific mono versions?)
			// So, try to load them manually recursively first.
			var subModuleFile = Path.Combine(Path.GetDirectoryName(module.Location), assemblyRef.Name + ".dll");
			if (File.Exists(subModuleFile))
			{
				var subModule = ModuleDefMD.Load(subModuleFile, Resolver.DefaultModuleContext);
				if (subModule != null)
					LoadDependencies(subModule);
			}

			var assembly = Resolver.ResolveThrow(assemblyRef, null);

			foreach (var moduleRef in assembly.Modules)
			{
				LoadDependencies((ModuleDefMD)moduleRef);
			}
		}
	}
}
