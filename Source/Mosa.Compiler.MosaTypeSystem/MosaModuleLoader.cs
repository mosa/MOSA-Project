// Copyright (c) MOSA Project. Licensed under the New BSD License.

using dnlib.DotNet;
using Mosa.Compiler.MosaTypeSystem.Metadata;
using System;
using System.Collections.Generic;
using System.IO;

namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaModuleLoader : IDisposable
	{
		public AssemblyResolver Resolver { get; }

		internal List<ModuleDefMD> Modules { get; }

		private readonly List<string> seenModules = new List<string>();

		public MosaModuleLoader()
		{
			Modules = new List<ModuleDefMD>();
			Resolver = new AssemblyResolver(null) { UseGAC = false };
			var typeResolver = new Resolver(Resolver);
			Resolver.DefaultModuleContext = new ModuleContext(Resolver, typeResolver);
			Resolver.EnableTypeDefCache = true;
		}

		#region Internal methods

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
				// (maybe this has todo with linux / specific mono versions?)
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

		/// <summary>
		/// Appends the given path to the assembly search path.
		/// </summary>
		/// <param name="path">The path to append to the assembly search path.</param>
		private void AddSearchPath(string path)
		{
			if (!Resolver.PostSearchPaths.Contains(path))
			{
				Resolver.PostSearchPaths.Add(path);
			}
		}

		#endregion Internal methods

		public void AddSearchPaths(List<string> paths)
		{
			if (paths == null)
				return;

			foreach (var path in paths)
			{
				AddSearchPath(path);
			}
		}

		/// <summary>
		/// Appends the given path to the assembly search path.
		/// </summary>
		/// <param name="path">The path to append to the assembly search path.</param>
		public void __AddPath(string path)
		{
			if (!Resolver.PostSearchPaths.Contains(path))
			{
				Resolver.PostSearchPaths.Add(path);
			}
		}

		/// <summary>
		/// Appends the given paths to the assembly search path.
		/// </summary>
		/// <param name="paths">The assembly paths.</param>
		public void __AddPath(IEnumerable<string> paths)
		{
			foreach (string path in paths)
			{
				if (path == null)
					continue;

				__AddPath(Path.GetDirectoryName(path));
			}
		}

		/// <summary>
		/// Loads the module.
		/// </summary>
		/// <param name="file">The file path of the module to load.</param>
		public void LoadModuleFromFile(string file)
		{
			var module = ModuleDefMD.Load(file, Resolver.DefaultModuleContext);
			module.EnableTypeDefFindCache = true;

			LoadDependencies(module);
		}

		/// <summary>
		/// Loads the module from files.
		/// </summary>
		/// <param name="files">The files.</param>
		public void LoadModuleFromFiles(IList<string> files)
		{
			foreach (var file in files)
			{
				LoadModuleFromFile(file);
			}
		}

		public IMetadata CreateMetadata()
		{
			return new CLRMetadata(this);
		}

		public void Dispose()
		{
			foreach (var module in Modules)
			{
				module.Dispose();
			}

			Modules.Clear();
		}
	}
}
