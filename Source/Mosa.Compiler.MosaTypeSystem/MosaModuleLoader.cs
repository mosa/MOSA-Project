/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Ki (kiootic) <kiootic@gmail.com>
 */

using dnlib.DotNet;
using Mosa.Compiler.MosaTypeSystem.Metadata;
using System.Collections.Generic;
using System.IO;

namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaModuleLoader : IModuleLoader
	{
		public AssemblyResolver Resolver { get; private set; }

		internal IList<ModuleDefMD> Modules { get; private set; }

		private List<string> privatePaths = new List<string>();

		public MosaModuleLoader()
		{
			Modules = new List<ModuleDefMD>();
			Resolver = new AssemblyResolver(null, false, false);
			var typeResolver = new dnlib.DotNet.Resolver(Resolver);
			Resolver.DefaultModuleContext = new ModuleContext(Resolver, typeResolver);
			Resolver.EnableTypeDefCache = true;
		}

		#region Internal methods

		private List<string> seenModules = new List<string>();

		private void LoadDependencies(ModuleDefMD module)
		{
			if (seenModules.Contains(module.Location))
				return;
			seenModules.Add(module.Location);
			Modules.Add(module);
			Resolver.AddToCache(module);

			foreach (var assemblyRef in module.GetAssemblyRefs())
			{
				AssemblyDef assembly = Resolver.ResolveThrow(assemblyRef, null);
				foreach (var moduleRef in assembly.Modules)
					LoadDependencies((ModuleDefMD)moduleRef);
			}
		}

		#endregion Internal methods

		/// <summary>
		/// Appends the given path to the assembly search path.
		/// </summary>
		/// <param name="path">The path to append to the assembly search path.</param>
		public void AddPrivatePath(string path)
		{
			if (!Resolver.PostSearchPaths.Contains(path))
			{
				Resolver.PostSearchPaths.Add(path);
			}
		}

		/// <summary>
		/// Appends the given paths to the assembly search path.
		/// </summary>
		/// <param name="assemblyPaths">The assembly paths.</param>
		public void AddPrivatePath(IEnumerable<string> assemblyPaths)
		{
			foreach (string path in assemblyPaths)
				AddPrivatePath(Path.GetDirectoryName(path));
		}

		/// <summary>
		/// Loads the module.
		/// </summary>
		/// <param name="file">The file path of the module to load.</param>
		public void LoadModuleFromFile(string file)
		{
			ModuleDefMD module = ModuleDefMD.Load(file, Resolver.DefaultModuleContext);
			module.EnableTypeDefFindCache = true;

			LoadDependencies(module);
		}

		void IModuleLoader.LoadModuleFromFile(string file)
		{
			LoadModuleFromFile(file);
		}

		public IMetadata CreateMetadata()
		{
			return new CLRMetadata(this);
		}
	}
}